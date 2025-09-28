using UnityEngine;

public class PlayerLeaderboardPanel : MonoBehaviour
{
    public static PlayerLeaderboardPanel Instance;

    private void Start()
    {
        Instance = this;
    }
}
