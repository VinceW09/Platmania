using System;
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
    [SerializeField] private TextMeshProUGUI playerId;

    private void OnEnable()
    {
        ShowProfilePage();
        errorText.gameObject.SetActive(false);
        playerId.text = "PLAYER ID: " + FirestoreManager.Singleton.GetCurrentUser().UserId;
    }

    public void ShowProfilePage()
    {
        profilePage.SetActive(true);
        editProfileInfoPage.SetActive(false);
        editPlayerPage.SetActive(false);
    }

    public void ShowEditProfileInfoPage()
    {
        errorText.gameObject.SetActive(false);
        profilePage.SetActive(false);
        editProfileInfoPage.SetActive(true);
        editPlayerPage.SetActive(false);
    }

    public void OnBackEditProfileInfoPage()
    {
        ResultResponse result = ProfileData.Singleton.SaveProfileInfo();

        if (result.isSuccessfull)
        {
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
