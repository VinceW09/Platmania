using Unity.Netcode;
using UnityEngine;

public class EndPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EndScreenUI.Instance.LevelEnd();
        Timer.Instance.SaveBestTime();

        if (FindAnyObjectByType<ServerManager>() != null)
        {
            Debug.Log("TIME SET: " + Timer.Instance.GetThisTime());
            Debug.Log(NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<TournamentPlayer>());

            NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<TournamentPlayer>().SetNewTime(Timer.Instance.GetThisTime());
        }
    }
}
