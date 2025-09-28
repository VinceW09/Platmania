using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject leftRightMoveButtons;
    [SerializeField] private GameObject rightSideButtons;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Button menuButton;

    private void Start()
    {
        if (FindAnyObjectByType<ServerManager>() != null)
        {
            menuButton.interactable = false;
        }
        else
        {
            menuButton.interactable = true;
        }

        Show(leftRightMoveButtons);
        Show(rightSideButtons);
        Show(healthBar);
        Show(timer);
        Hide(pauseMenu);
    }

    private void Update()
    {
        // Keyboard
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                PauseButton();
            }
            else
            {
                ResumeButton();
            }
        }
    }

    public void PauseButton()
    {
        Hide(leftRightMoveButtons);
        Hide(rightSideButtons);
        Hide(healthBar);
        Hide(timer);
        Show(pauseMenu);

        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        Show(leftRightMoveButtons);
        Show(rightSideButtons);
        Show(healthBar);
        Show(timer);
        Hide(pauseMenu);

        Time.timeScale = 1f;
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
