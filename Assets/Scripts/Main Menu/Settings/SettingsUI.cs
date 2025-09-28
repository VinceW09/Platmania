using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject settingButtons1;
    [SerializeField] private GameObject settingButtons2;
    [SerializeField] private GameObject confirmation;
    [SerializeField] private Transform settings;

    private ConfirmationDialogue currentConfirmation;

    public void OnResetDataClick()
    {
        currentConfirmation = Instantiate(confirmation, settings).GetComponent<ConfirmationDialogue>();
        settingButtons1.SetActive(false);
        settingButtons2.SetActive(false);
        
        currentConfirmation.SetupConfirmation("Are you really sure?", "Reseted all data successfully!", () =>
        {
            PlayerPrefs.DeleteAll();
        }, () =>
        {
            settingButtons1.SetActive(true);
            settingButtons2.SetActive(true);
        });
    }

    public void OnCreditsClick()
    {
        currentConfirmation = Instantiate(confirmation, settings).GetComponent<ConfirmationDialogue>();
        settingButtons1.SetActive(false);
        settingButtons2.SetActive(false);

        currentConfirmation.SetupConfirmation("This game was made entirely by Vincent Wikand using Unity", () =>
        {
            settingButtons1.SetActive(true);
            settingButtons2.SetActive(true);
        });
    }
}
