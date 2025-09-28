using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static string tempMessage;
    private static bool tempBool;

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public static void LoadScene(string sceneName, string message, bool isIp)
    {
        SceneManager.LoadScene(sceneName);
        tempMessage = message;
        tempBool = isIp;
    }

    public static int ReturnCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static int ReturnSceneCount()
    {
        return SceneManager.sceneCountInBuildSettings;
    }

    public static string ReturnLatestMessage()
    {
        return tempMessage;
    }

    public static bool ReturnLatestBool()
    {
        return tempBool;
    }
}
