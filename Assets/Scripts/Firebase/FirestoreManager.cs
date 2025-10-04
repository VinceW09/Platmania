using Firebase.Extensions;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Singleton {  get; private set; }

    private FirebaseFirestore database;

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

    public void GetLocalUserData()
    {
        DocumentReference userRef = database.Collection("users").Document(SystemInfo.deviceUniqueIdentifier);
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(snapshot =>
        {
            if (snapshot.Result.Exists)
            {
                Debug.Log($"Userata for user: {snapshot.Result.Id}");
                foreach (var item in snapshot.Result.ToDictionary())
                {
                    Debug.Log($"{item.Key}: {item.Value}");
                } 
            }
        });
    }

    private void OnFirebaseAvailable_Callback(FirebaseFirestore database)
    {
        this.database = database;
        GetLocalUserData();
    }
}
