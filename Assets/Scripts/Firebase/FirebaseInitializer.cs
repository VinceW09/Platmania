using Firebase;
using Firebase.Analytics;
using Firebase.AppCheck;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.IO;
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

#if UNITY_EDITOR
        string debugToken = LoadDebugToken();

        //Debug.Log($"Loading debug token: {debugToken}");

        if (!string.IsNullOrEmpty(debugToken))
        {
            DebugAppCheckProviderFactory.Instance.SetDebugToken(debugToken);
            FirebaseAppCheck.SetAppCheckProviderFactory(DebugAppCheckProviderFactory.Instance);
        }
#else
        FirebaseAppCheck.SetAppCheckProviderFactory(PlayIntegrityProviderFactory.Instance);
#endif

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseAppCheck.DefaultInstance.GetAppCheckTokenAsync(false).ContinueWithOnMainThread(task2 => {

                // Check token
                if (task2.IsCompleted && !task2.IsFaulted)
                {
                    //Debug.Log($"App Check Token: {task2.Result.Token}");
                }
                else
                {
                    Debug.LogError("Failed to get App Check token: " + task2.Exception);
                }

                //Continue
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

            
        });
    }

    private string LoadDebugToken()
    {
        string path = Path.Combine(Application.dataPath, "../appcheck_debug_token.txt");

        if (File.Exists(path))
        {
            string token = File.ReadAllText(path).Trim();
            Debug.Log("App Check: Debug token loaded successfully.");
            return token;
        }

        Debug.LogWarning("App Check: No debug token file found at " + path);
        return null;
    }
}
