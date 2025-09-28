using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TournamentMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject createLobby;
    [SerializeField] private GameObject joinLobby;
    [SerializeField] private GameObject main;

    private void Start()
    {
        Show(main);
        Hide(joinLobby);
        Hide(createLobby);
    }

    public void OnBackClick()
    {
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.MAINMENU].name);
    }

    public void OnCreateLobbyClick()
    {
        Hide(joinLobby);
        Hide(main);
        Show(createLobby);
    }

    public void OnJoinLobbyClick()
    {
        Show(joinLobby);
        Hide(main);
        Hide(createLobby);
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
