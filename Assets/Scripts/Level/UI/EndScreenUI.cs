using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    public static EndScreenUI Instance { get; private set; }

    [SerializeField] private GameObject leftRightMoveButtons;
    [SerializeField] private GameObject rightSideButtons;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject levelEndScreen;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private Button menuButton;
    [SerializeField] private Button nextLevelButton;

    private void Start()
    {
        Instance = this;

        if (FindAnyObjectByType<ServerManager>() != null)
        {
            menuButton.interactable = false;
            nextLevelButton.interactable = false;
        }
        else
        {
            menuButton.interactable = true;
            nextLevelButton.interactable = true;
        }

        Show(leftRightMoveButtons);
        Show(rightSideButtons);
        Show(healthBar);
        Show(timer);
        Hide(levelEndScreen);
    }

    public void LevelEnd()
    {
        Hide(leftRightMoveButtons);
        Hide(rightSideButtons);
        Hide(healthBar);
        Hide(timer);
        Show(levelEndScreen);

        timerText.text = "LEVEL COMPLETE!\nYOUR TIME: " + Timer.Instance.GetThisTimeAsString();

        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        if (Loader.ReturnCurrentSceneIndex() + 1 > Loader.ReturnSceneCount() - 1)
        {
            Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.MAINMENU].name);  // Do an error or something in future
            return;
        }
        Loader.LoadScene(Loader.ReturnCurrentSceneIndex() + 1);
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.MAINMENU].name);
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        Loader.LoadScene(Loader.ReturnCurrentSceneIndex());
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
