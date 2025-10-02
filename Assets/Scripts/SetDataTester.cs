using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class SetDataTester : MonoBehaviour
{
    private void Start()
    {
        FirebaseInit.Singleton.OnFirestoreAvailable += OnFirebaseAvailable_Callback;
    }

    private void OnFirebaseAvailable_Callback(FirebaseFirestore database)
    {
        var userData = new UserData
        {
            UserName = "VinceW",
            RealName = "Vincent Wikand",
            FaceId = "HappyFace",
            ColorId = "Yellow"
        };

        DocumentReference userRef = database.Collection("users").Document(SystemInfo.deviceUniqueIdentifier);
        userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Updated: {userData}");
        });
    }
}
