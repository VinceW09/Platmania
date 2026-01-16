using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseInitializer Singleton { get; private set; }

    public delegate void FirestoreAvailableEvent(FirebaseFirestore database, FirebaseUser user);
    public event FirestoreAvailableEvent OnFirestoreAvailable;

    private FirebaseApp app;
    private FirebaseAuth auth;
    private FirebaseUser user;

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
                auth = FirebaseAuth.GetAuth(app);

                // Sign in
                if (auth.CurrentUser != null)
                {
                    user = auth.CurrentUser;
                    Debug.Log($"User already signed in: ({user.UserId})");
                }
                else
                {
                    auth.SignInAnonymouslyAsync().ContinueWith((authTask) =>
                    {
                        if (authTask.IsCanceled)
                        {
                            Debug.Log("Sign-in canceled.");
                        }
                        else if (authTask.IsFaulted)
                        {
                            Debug.Log("Sign-in encountered an error.");
                            Debug.Log(authTask.Exception.ToString());
                        }
                        else if (authTask.IsCompleted)
                        {
                            AuthResult result = authTask.Result;
                            user = result.User;
                            Debug.Log($"User signed in successfully: ({user.UserId})");
                        }
                    });
                } 

                //Analytics
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                //Give 'GO' for all other scripts
                if (OnFirestoreAvailable != null)
                {
                    OnFirestoreAvailable(FirebaseFirestore.DefaultInstance, user);
                }
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }

        });
    }
}
