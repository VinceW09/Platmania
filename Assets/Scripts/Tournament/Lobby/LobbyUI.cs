using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Singleton {  get; private set; }

    [SerializeField] private GameObject ConfirmationDialogue;
    private ConfirmationDialogue confirmation;

    [SerializeField] private GameObject playerList;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject levelSelectButton;
    [SerializeField] private GameObject joinVisual;
    [SerializeField] private GameObject leaderboard;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private GameObject timePrompt;
    [SerializeField] private TextMeshProUGUI time;

    private void Awake()
    {
        Singleton = this;
        Hide(joinVisual);
    }

    private void Update()
    {
        if (leaderboard != null)
        {
            time.text = "TIME LEFT: " + TimeFormatter.FormatTime(GameManager.Singleton.GetRemainingTimeInSeconds());
        }
    }

    public void LeaveLobbyButton()
    {
        confirmation = Instantiate(ConfirmationDialogue, transform).GetComponent<ConfirmationDialogue>();
        confirmation.SetupConfirmation("Are you sure you want to leave?", () =>
        {
            ServerManager.Singleton.Disconnect();
        }, () =>
        {
            // Does nothing
        });
    }

    public void StartButton()
    {
        Show(timePrompt);
    }

    public void TimePromptClick()
    {
        Hide(timePrompt);

        confirmation = Instantiate(ConfirmationDialogue, transform).GetComponent<ConfirmationDialogue>();
        confirmation.SetupConfirmation("This will start the tournament, are you ready?", () =>
        {
            Hide(playerList);
            Hide(title);
            Hide(levelSelectButton);
            Destroy(startButton);

            leaderboard.GetComponent<CanvasGroup>().alpha = 1;

            GameManager.Singleton.StartTournament();
        }, () =>
        {
            // Does nothing
        });
    }

    public void SelectLevelButton()
    {
        Hide(startButton);
        Hide(levelSelectButton);
        Hide(playerList);

        Show(levelSelection);
    }

    public void LevelSelectBackButton()
    {
        Show(startButton);
        Show(levelSelectButton);
        Show(playerList);

        Hide(levelSelection);
    }

    public void SelectLevel(LevelIdentificationSO level)
    {
        ServerManager.Singleton.SetLevelNumber(level.levelNumber);

        LevelSelectBackButton();
    }

    public void SetupClient(Profile.PlayerData playerData)
    {
        Destroy(playerList);
        Destroy(leaderboard);
        Destroy(startButton);
        Destroy(levelSelectButton);
        Destroy(GameManager.Singleton.gameObject);
        Show(title);
        Show(joinVisual);
        Destroy(timePrompt);
        joinVisual.GetComponent<PlayerProfileUI>().SetPlayerInfo(playerData, true);
    }

    public void SetupServer()
    {
        Destroy(joinVisual);
        
        Show(leaderboard);
        leaderboard.GetComponent<CanvasGroup>().alpha = 0;

        Show(playerList);
        Show(title);
        Hide(levelSelection);
        Hide(timePrompt);
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
