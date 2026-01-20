using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] private GameObject profilePage;
    [SerializeField] private GameObject editProfileInfoPage;
    [SerializeField] private GameObject editPlayerPage;

    [SerializeField] private GameObject editFacePage;
    [SerializeField] private GameObject editColorPage;

    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField nameField;

    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI playerId;

    [SerializeField] private List<PlayerPreview> playerPreviews;

    private UserData previewUserData;

    private void Awake()
    {
        previewUserData = ProfileData.Singleton.GetLocalUserData();
        Debug.Log(previewUserData);

        ShowProfilePage();
        errorText.gameObject.SetActive(false);

        UpdatePreviewData();
    }

    public void SetPreviewData(UserData userData)
    {
        previewUserData = userData;
        UpdatePreviewData();
    }

    private void UpdatePreviewData()
    {
        usernameField.text = previewUserData.Username;
        nameField.text = previewUserData.Name;

        usernameText.text = previewUserData.Username;
        nameText.text = previewUserData.Name;

        playerId.text = "PLAYER ID: " + (previewUserData.UserId ?? "FAILED TO FETCH ID [OFFLINE]");

        foreach (PlayerPreview playerPreview in playerPreviews)
        {
            playerPreview.SetPlayer(previewUserData.ColorId, previewUserData.FaceId);
        }
    }

    public void SetPlayerColorButton(SpriteSO spriteSO)
    {
        previewUserData.ColorId = spriteSO.id;
        UpdatePreviewData();
    }

    public void SetPlayerFaceButton(SpriteSO spriteSO)
    {
        previewUserData.FaceId = spriteSO.id;
        UpdatePreviewData();
    }

    public void ShowProfilePage()
    {
        profilePage.SetActive(true);
        editProfileInfoPage.SetActive(false);
        editPlayerPage.SetActive(false);
    }

    public void ShowEditProfileInfoPage()
    {
        usernameField.text = previewUserData.Username;
        nameField.text = previewUserData.Name;

        errorText.gameObject.SetActive(false);
        profilePage.SetActive(false);
        editProfileInfoPage.SetActive(true);
        editPlayerPage.SetActive(false);
    }

    public void OnBackEditProfileInfoPage()
    {
        previewUserData.Username = usernameField.text;
        previewUserData.Name = nameField.text;

        ResultResponse result = ProfileData.Singleton.SaveUserData(previewUserData);

        if (result.isSuccessfull)
        {
            UpdatePreviewData();
            ShowProfilePage();
        }
        else
        {
            errorText.text = result.error;
            errorText.gameObject.SetActive(true);
        }
    }

    public void ShowEditPlayerPage()
    {
        previewUserData.Username = usernameField.text;
        previewUserData.Name = nameField.text;

        profilePage.SetActive(false);
        editProfileInfoPage.SetActive(false);
        editPlayerPage.SetActive(true);
    }

    public void ShowFacesPage()
    {
        editFacePage.SetActive(true);
        editColorPage.SetActive(false);
    }

    public void ShowColorsPage()
    {
        editFacePage.SetActive(false);
        editColorPage.SetActive(true);
    }
}
