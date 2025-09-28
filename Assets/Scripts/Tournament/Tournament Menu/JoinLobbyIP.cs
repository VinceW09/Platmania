using TMPro;
using UnityEngine;

public class JoinLobbyIP : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private string ip = string.Empty;

    public void OnJoinLobbyConfirm()
    {
        ip = inputField.text;
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.LOBBY].name, ip, true);
    }

    public void OnJoinLobbyCancel()
    {
        Hide(gameObject);
    }

    public void OnButtonClick()
    {
        Show(gameObject);
    }

    public void Show(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void Hide(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
