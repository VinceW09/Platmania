using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ProfileData : MonoBehaviour
{
    public static ProfileData Singleton { get; private set; }

    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField nameField;

    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private List<PlayerPreview> playerPreviews;

    private UserData userData;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        FirebaseInitializer.Singleton.OnFirestoreAvailable += OnFirestoreAvailable_Callback;
    }

    private void OnFirestoreAvailable_Callback(Firebase.Firestore.FirebaseFirestore database)
    {
        Debug.Log("Got firestore");
        LoadUserData();
    }

    private async void LoadUserData()
    {
        var (isSuccessful, userData) = await FirestoreManager.Singleton.GetLocalUserDataAsync();

        this.userData = userData;

        if (this.userData.Name == null) this.userData.Name = "Name";
        if (this.userData.ColorId == null) this.userData.ColorId = "yellow";
        if (this.userData.FaceId == null) this.userData.FaceId = "happy";
        if (this.userData.Username == null)
        {
            this.userData.Username = GenerateUniqueUsername();
            await FirestoreManager.Singleton.SetLocalUserData(this.userData);
        }

        Debug.Log(this.userData);

        UpdatePreviewData();
    }

    private void UpdatePreviewData()
    {
        usernameField.text = userData.Username;
        nameField.text = userData.Name;

        usernameText.text = userData.Username;
        nameText.text = userData.Name;

        foreach (PlayerPreview playerPreview in playerPreviews)
        {
            playerPreview.SetPlayer(userData.ColorId, userData.FaceId);
        }
    }

    public async Task<ResultResponse> SaveProfileInfo()
    {
        ResultResponse result = new ResultResponse();

        if (usernameField.text.Length > 2 && usernameField.text.Length <= 15)
        {
            if (await FirestoreManager.Singleton.IsUsernameAvailable(userData.Username) == true)
            {
                userData.Username = usernameField.text;
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

        userData.Name = nameField.text;

        UpdatePreviewData();
        await FirestoreManager.Singleton.SetLocalUserData(userData);

        result.isSuccessfull = true;
        return result;
    }

    public void SetPlayerColorButton(SpriteSO spriteSO)
    {
        userData.ColorId = spriteSO.id;
        UpdatePreviewData();
    }

    public void SetPlayerFaceButton(SpriteSO spriteSO)
    {
        userData.FaceId = spriteSO.id;
        UpdatePreviewData();
    }

    private string GenerateUniqueUsername()
    {
        StringBuilder stringBuilder = new StringBuilder();
        const string authorizedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        stringBuilder.Append("Platman_");

        for (int i = 0; i < 6; i++)
        {
            stringBuilder.Append(authorizedCharacters[UnityEngine.Random.Range(0, authorizedCharacters.Length)]);
        }

        string uniqueUsername = stringBuilder.ToString();

        if (FirestoreManager.Singleton.IsUsernameAvailable(uniqueUsername).Result == false)
        {
            uniqueUsername = GenerateUniqueUsername();
        }

        return uniqueUsername;
    }
}