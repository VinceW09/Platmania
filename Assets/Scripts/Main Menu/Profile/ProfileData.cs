using Firebase.Auth;
using Firebase.Firestore;
using System.Text;
using UnityEngine;

public class ProfileData : MonoBehaviour
{
    public static ProfileData Singleton { get; private set; }

    [SerializeField] private ProfileUI profileUI;

    private UserData userData;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        FirebaseInitializer.Singleton.OnFirestoreAvailable += OnFirestoreAvailable_Callback;
    }

    private void OnFirestoreAvailable_Callback(FirebaseFirestore database, FirebaseUser user)
    {
        Debug.Log("ProfileData getting firestore");
        LoadUserData();
    }

    private void LoadUserData()
    {
        FirestoreManager.Singleton.GetLocalUserData(userData =>
        {
            if (userData.UserId == null)
            {
                this.userData = GeneratePlaceholderData(true);
                profileUI.SetPreviewData(this.userData);
                return;
            }

            if (userData.Username == null)
            {
                this.userData = GeneratePlaceholderData(false, userData.UserId);
                FirestoreManager.Singleton.SetLocalUserData(this.userData);
            }
            else
            {
                this.userData = userData;
                Debug.Log(this.userData.Username);
            }

            profileUI.SetPreviewData(this.userData);
        });
    }

    public ResultResponse SaveUserData(UserData newUserData)
    {
        ResultResponse result = new ResultResponse();

        if (newUserData.Username != userData.Username)
        {
            if (newUserData.Username.Length > 2 && newUserData.Username.Length <= 15)
            {
                if (FirestoreManager.Singleton.IsUsernameAvailable(newUserData.Username) == true)
                {
                    Debug.Log("Username change is valid.");
                }
                else
                {
                    result.isSuccessfull = false;
                    result.error = "Username taken";
                    return result;
                }
            }
            else
            {
                result.isSuccessfull = false;
                result.error = "Must be 3-15 characters long";
                return result;
            }
        }

        userData = newUserData;
        FirestoreManager.Singleton.SetLocalUserData(userData);

        result.isSuccessfull = true;
        return result;
    }

    private UserData GeneratePlaceholderData(bool isOffline, string UserId = null)
    {
        UserData userData = new UserData();

        userData.UserId = UserId;
        userData.Name = "Name";
        userData.ColorId = "yellow";
        userData.FaceId = "happy";
        userData.Username = GenerateUniqueUsername(isOffline);

        return userData;
    }

    private string GenerateUniqueUsername(bool isOffline)
    {
        StringBuilder stringBuilder = new StringBuilder();
        const string authorizedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        stringBuilder.Append("Platman_");

        for (int i = 0; i < 6; i++)
        {
            stringBuilder.Append(authorizedCharacters[UnityEngine.Random.Range(0, authorizedCharacters.Length)]);
        }

        string uniqueUsername = stringBuilder.ToString();

        if (isOffline) return uniqueUsername;

        if (FirestoreManager.Singleton.IsUsernameAvailable(uniqueUsername) == false)
        {
            uniqueUsername = GenerateUniqueUsername(isOffline);
        }

        return uniqueUsername;
    }

    public UserData GetLocalUserData()
    {
        if (userData.UserId == null)
        {
            Debug.LogWarning("No connection -> Offline mode");
            userData = GeneratePlaceholderData(true);
        }

        return userData;
    }
}