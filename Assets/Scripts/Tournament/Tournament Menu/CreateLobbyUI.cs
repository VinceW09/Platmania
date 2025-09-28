using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject main;
    private string lobbyName = "";

    public void OnCreateLobbyConfirm()
    {
        lobbyName = inputField.text;
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.LOBBY].name, lobbyName, false);
    }

    public void OnCreateLobbyCancel()
    {
        Hide(gameObject);
        Show(main);
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
