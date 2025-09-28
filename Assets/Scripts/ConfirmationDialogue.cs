using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private GameObject okButton;

    private bool showSuccessScreen;
    private bool onlyInfo;
    private string successText;
    private string confirmationText;
    private Action mainAction;
    private Action cancelAction;

    private void Start()
    {
        StartupProcedure();
    }

    public void DoAction()
    {
        mainAction();

        if (showSuccessScreen == true)
        {
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
            okButton.SetActive(true);

            text.text = successText;
        }
        else
        {
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
            gameObject.SetActive(false);
            cancelAction();
        }

        Destroy(gameObject);
    }

    public void Cancel() // Doubles as ok button
    {
        cancelAction();
        gameObject.SetActive(false);

        Destroy(gameObject);
    }

    public void SetupConfirmation(string text, Action callback, Action closeCallback)
    {
        showSuccessScreen = false;
        onlyInfo = false;

        successText = "";
        confirmationText = text;

        mainAction = callback;
        cancelAction = closeCallback;

    }

    public void SetupConfirmation(string text, string secondText, Action callback, Action closeCallback)
    {
        showSuccessScreen = true;
        onlyInfo = false;

        successText = secondText;
        confirmationText = text;

        mainAction = callback;
        cancelAction = closeCallback;
    }

    public void SetupConfirmation(string text, Action closeCallback)
    {
        showSuccessScreen = false;
        onlyInfo = true;

        successText = "";
        confirmationText = text;

        mainAction = () => { };
        cancelAction = closeCallback;
    }

    private void StartupProcedure()
    {
        if (onlyInfo)
        {
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
            okButton.SetActive(true);
        }
        else
        {
            confirmButton.SetActive(true);
            cancelButton.SetActive(true);
            okButton.SetActive(false);
        }

        text.text = confirmationText;
    }
}
