using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfilePanel : MonoBehaviour
{
    public static PlayerProfilePanel Instance;

    private void Start()
    {
        Instance = this;
    }
}
