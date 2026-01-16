using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.Collections.Unicode;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Singleton {  get; private set; }

    private FirebaseFirestore database;
    private FirebaseUser user;

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

    public void SetLocalUserData(UserData userData)
    {
        DocumentReference userRef = database.Collection("users").Document(user.UserId);
        userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Updated userdata of user ({user.UserId})");
            }
            else
            {
                Debug.LogWarning("Failed to set data");
            }
        });
    }

    public void GetLocalUserData(System.Action<UserData> callback) 
    {
        DocumentReference userRef = database.Collection("users").Document(user.UserId);
        
        Debug.Log("Getting local user data");
        UserData userData = new UserData();

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                callback?.Invoke(new UserData());
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            userData = snapshot.Exists ? snapshot.ConvertTo<UserData>() : new UserData();

            if (snapshot.Exists)
            {
                Debug.Log("User data found:");
                Debug.Log($"Userdata for user {userData.Username} found:\n" +
                          $"UserId: {user.UserId}\n" +
                          $"Name: {userData.Name}\n" +
                          $"ColorId: {userData.ColorId}\n" +
                          $"FaceId: {userData.FaceId}");

            }
            else
            {
                Debug.LogWarning("User document does not exist.");
            }

            callback?.Invoke(userData);
        });
    }

    public bool IsUsernameAvailable(string username) // Not perfect
    {
        Query query = database.Collection("users").WhereEqualTo(USERNAME, username);
        QuerySnapshot snapshot;

        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Checking if username '{username}' is taken.");
            snapshot = task.Result;

            bool isAvailable = true;
            GetLocalUserData(userData =>
            {
                bool isAvailable = snapshot.Count == ((userData.Username == username) ? 1 : 0);
            });

            return isAvailable;
        });

        return true; // SHOULD NOT HAPPEN
    }

    private void OnFirebaseAvailable_Callback(FirebaseFirestore database, FirebaseUser user)
    {
        this.database = database;
        this.user = user;
    }

    public FirebaseUser GetCurrentUser()
    {
        return user;
    }
}
