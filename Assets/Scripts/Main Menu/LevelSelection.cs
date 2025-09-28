using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject levelConfirmation;
    [SerializeField] private GameObject levelSelection;

    public void ShowLevelConfirmation(LevelIdentificationSO level)
    {
        levelConfirmation.GetComponent<LevelConfirmation>().OnLevelConfirmation(level);
        Show(levelConfirmation);
        Hide(levelSelection);
    }

    public void StartTutorial()
    {
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.TUTORIAL].name);
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
