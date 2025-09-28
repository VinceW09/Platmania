using UnityEngine;

public class TimePromptUI : MonoBehaviour
{
    public void OnButtonClick(int time)
    {
        GameManager.Singleton.SetPlayingTimeMinutes(time);
        LobbyUI.Singleton.TimePromptClick();
    }
}
