using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseInitializer Singleton { get; private set; }

    public delegate void FirestoreAvailableEvent(FirebaseFirestore database);
    public event FirestoreAvailableEvent OnFirestoreAvailable;

    private FirebaseApp app;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log(task.Result);

            if (task.Result == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;

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
