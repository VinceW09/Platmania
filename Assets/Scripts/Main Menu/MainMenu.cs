using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject levelSelectScreen;
    [SerializeField] private GameObject levelConfirmation;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject profileScreen;
    [SerializeField] private GameObject changelog;
    [SerializeField] private GameObject changeLogButton;
    [SerializeField] private Button tournamentButton;
 
    void Awake()
    {
        Show(mainScreen);
        Show(changeLogButton);
        Hide(levelSelectScreen);
        Hide(levelConfirmation);
        Hide(settingsScreen);
        Hide(profileScreen);
        Hide(changelog);

        if (Application.platform == RuntimePlatform.WebGLPlayer) tournamentButton.interactable = false;
    }

    public void OnLevelSelectClick()
    {
        Hide(changeLogButton);
        Hide(mainScreen);
        Show(levelSelectScreen);
    }

    public void OnSettingsClick()
    {
        Hide(changeLogButton);
        Hide(mainScreen);
        Show(settingsScreen);
    }

    public void OnProfileClick()
    {
        Hide(changeLogButton);
        Hide(mainScreen);
        Show(profileScreen);
    }

    public void OnTournamentClick()
    {
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.TOURNAMENTMENU].name);
    }

    public void OnLevelSelectBackClick()
    {
        Show(changeLogButton);
        Show(mainScreen);
        Hide(levelSelectScreen);
    }

    public void OnSettingsBackClick()
    {
        Show(changeLogButton);
        Show(mainScreen);
        Hide(settingsScreen);
    }

    public void OnProfileBackClick()
    {
        Show(changeLogButton);
        Show(mainScreen);
        Hide(profileScreen);
    }

    public void OnChangelogClick()
    {
        Hide(mainScreen);
        Show(changelog);
    }

    public void OnChangelogBackClick()
    {
        Show(changeLogButton);
        Show(mainScreen);
        Hide(changelog);
    }

    private void Show(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void Hide(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
