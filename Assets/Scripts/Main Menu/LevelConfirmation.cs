using TMPro;
using UnityEngine;

public class LevelConfirmation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelInfoText;
    [SerializeField] private GameObject LevelSelection;
    private LevelIdentificationSO level;
    private const string BEST_TIME_SAVE_NAME = "Best_Time_Level_";

    public void OnLevelConfirmation(LevelIdentificationSO levelIdentification)
    {
        level = levelIdentification;
        levelInfoText.text = "LEVEL " + levelIdentification.levelNumber + "\nBEST TIME: " + GetBestTimeAsString(levelIdentification);
    }

    public void BackButton()
    {
        gameObject.SetActive(false);
        LevelSelection.SetActive(true);
    }

    public void PlayButton()
    {
        Loader.LoadScene(level.sceneName);
    }

    private string GetBestTimeAsString(LevelIdentificationSO level)
    {
        if (PlayerPrefs.HasKey(BEST_TIME_SAVE_NAME + level.levelNumber))
        {
            return TimeFormatter.FormatTime(PlayerPrefs.GetFloat(BEST_TIME_SAVE_NAME + level.levelNumber));
        }
        else
        {
            return "N/A";
        }

    }
}
