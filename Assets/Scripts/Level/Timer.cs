using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }
    private float timer;
    private const string BEST_TIME_SAVE_NAME = "Best_Time_Level_";
    [SerializeField] private LevelIdentificationSO level;

    private void Start()
    {
        Instance = this;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public string GetThisTimeAsString()
    {
        return TimeFormatter.FormatTime(timer);
    }

    public float GetThisTime()
    {
        return timer;
    }

    public void SaveBestTime()
    {
        if (PlayerPrefs.HasKey(BEST_TIME_SAVE_NAME + level.levelNumber))
        {
            if (PlayerPrefs.GetFloat(BEST_TIME_SAVE_NAME + level.levelNumber) > timer)
            {
                PlayerPrefs.SetFloat(BEST_TIME_SAVE_NAME + level.levelNumber, timer);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(BEST_TIME_SAVE_NAME + level.levelNumber, timer);
        }

        PlayerPrefs.Save();
    }
}
