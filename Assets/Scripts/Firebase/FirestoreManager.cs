using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Singleton {  get; private set; }

    private FirebaseFirestore database;

    private const string COLOR_ID = "ColorId";
    private const string FACE_ID = "FaceId";
    private const string USERNAME = "Username";
    private const string NAME = "Name";

    private void Start()
    {
        FirebaseInitializer.Singleton.OnFirestoreAvailable += OnFirebaseAvailable_Callback;
    }

    public void SetLocalUserData(UserData userData)
    {
        DocumentReference userRef = database.Collection("users").Document(SystemInfo.deviceUniqueIdentifier);
        userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Updated userdata of user ({SystemInfo.deviceUniqueIdentifier})");
        });
    }

    public UserData GetLocalUserData()
    {
        UserData userData = new UserData();

        DocumentReference userRef = database.Collection("users").Document(SystemInfo.deviceUniqueIdentifier);
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result.Exists)
            {
                foreach (var item in task.Result.ToDictionary())
                {
                    switch (item.Key)
                    {
                        case USERNAME:
                            userData.Username = item.Value.ToString();
                            break;
                        case NAME:
                            userData.Name = item.Value.ToString();
                            break;
                        case COLOR_ID:
                            userData.ColorId = item.Value.ToString();
                            break;
                        case FACE_ID:
                            userData.FaceId = item.Value.ToString();
                            break;
                        default:
                            Debug.LogError($"{item.Key} does not exist!");
                            break;
                    }
                }

                Debug.Log($"Userdata for user {userData.Username}:\n" +
                    $"Name: {userData.Name}\n" +
                    $"ColorId: {userData.ColorId}\n" +
                    $"FaceId: {userData.FaceId}");
            }
        });

        return userData;
    }

    public async Task<bool> IsUsernameAvailable(string username)
    {
        Query query = database.Collection("users").WhereEqualTo(USERNAME, username);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        return snapshot.Count == 0;

        //needs to await when called
    }

    private void OnFirebaseAvailable_Callback(FirebaseFirestore database)
    {
        this.database = database;
    }
}
