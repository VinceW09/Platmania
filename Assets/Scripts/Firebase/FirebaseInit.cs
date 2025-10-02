using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseInit Singleton { get; private set; }

    public delegate void FirestoreAvailableEvent(FirebaseFirestore database);
    public event FirestoreAvailableEvent OnFirestoreAvailable;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                if (OnFirestoreAvailable != null)
                {
                    OnFirestoreAvailable(FirebaseFirestore.DefaultInstance);
                }

                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }

        });
    }
}
