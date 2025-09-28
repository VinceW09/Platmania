using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu()]
public class LevelIdentificationSO : ScriptableObject
{
    public int campaignNumber;
    public int levelNumber;
    public string sceneName;
}
