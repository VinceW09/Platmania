using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public static ButtonHandler Instance { get; private set; }

    public bool rightPress;
    public bool leftPress;
    public bool jumpPress;
    public bool dashPress;

    private void Start()
    {
        Instance = this;
    }

    // Right
    public void OnRightPress()
    {
        rightPress = true;
    }

    public void OnRightRelease()
    {
        rightPress = false;
    }

    // Left
    public void OnLeftPress()
    {
        leftPress = true;
    }

    public void OnLeftRelease()
    {
        leftPress = false;
    }

    // Jump
    public void OnJumpPress()
    {
        jumpPress = true;
    }

    public void OnJumpRelease()
    {
        jumpPress = false;
    }

    // Dash
    public void OnDashPress()
    {
        dashPress = true;
    }
    public void OnDashRelease()
    {
        dashPress = false;
    }
}
