using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Singleton {  get; private set; }

    private FirebaseFirestore database;

    private const string COLOR_ID = "ColorId";
    private const string FACE_ID = "FaceId";
    private const string USERNAME = "Username";
    private const string NAME = "Name";

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        FirebaseInitializer.Singleton.OnFirestoreAvailable += OnFirebaseAvailable_Callback;
    }

    public async Task SetLocalUserData(UserData userData)
    {
        DocumentReference userRef = database.Collection("users").Document(SystemInfo.deviceUniqueIdentifier);
        await userRef.SetAsync(userData);
        
        Debug.Log($"Updated userdata of user ({SystemInfo.deviceUniqueIdentifier})");
        
    }

    public async Task<(bool wasSuccessful, UserData userData)> GetLocalUserDataAsync()
    {
        DocumentReference userRef = database.Collection("users").Document(SystemInfo.deviceUniqueIdentifier);
        var snapshot = await userRef.GetSnapshotAsync();
        
        Debug.Log("Getting local user data");

        if (snapshot.Exists)
        {
            Debug.Log("User data found.");
            UserData userData = snapshot.ConvertTo<UserData>();

            Debug.Log($"Userdata for user {userData.Username}:\n" +
                      $"Name: {userData.Name}\n" +
                      $"ColorId: {userData.ColorId}\n" +
                      $"FaceId: {userData.FaceId}");

            return (true, userData);
        }
        else
        {
            Debug.LogWarning("User document does not exist.");
            return (false, new UserData());
        }
    }

    public async Task<bool> IsUsernameAvailable(string username)
    {
        Query query = database.Collection("users").WhereEqualTo(USERNAME, username);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        var (isSuccessful, userData) = await GetLocalUserDataAsync();

        return snapshot.Count == ((userData.Username == username) ? 1 : 0);

        //needs to await when called
    }

    private void OnFirebaseAvailable_Callback(FirebaseFirestore database)
    {
        this.database = database;
    }
}
