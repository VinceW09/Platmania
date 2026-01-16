using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private Transform playerPosition;
    [SerializeField] private SpriteRenderer face;
    [SerializeField] private SpriteRenderer color;
    [SerializeField] private SpriteListSO playerColors;
    [SerializeField] private SpriteListSO playerFaces;

    private int startingHealth = 100;
    private int health;

    private string playerColor;
    private string playerFace;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = startingHealth;

        playerColor = ProfileData.Singleton.GetLocalUserData().ColorId;
        playerFace = ProfileData.Singleton.GetLocalUserData().FaceId;

        color.sprite = GetPlayerColorSprite(playerColor);
        face.sprite = GetPlayerFaceSprite(playerFace);
    }

    private void Update()
    {
        if (playerPosition.position.y <= -10)
        {
            health = 0;
        }

        if (health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Damage(int damageAmount)
    {
        health = health - damageAmount;
    }

    public int GetHealth()
    {
        return health;
    }

    private Sprite GetPlayerColorSprite(string colorId)
    {
        foreach (SpriteSO color in playerColors.spriteSOs)
        {
            if (color.id == colorId)
            {
                return color.sprite;
            }
        }
        throw new ArgumentException("The function 'GetPlayerColorSprite' couldn't return any sprite!");
    }

    private Sprite GetPlayerFaceSprite(string spriteId)
    {
        foreach (SpriteSO face in playerFaces.spriteSOs)
        {
            if (face.id == spriteId)
            {
                return face.sprite;
            }
        }
        throw new ArgumentException("The function 'GetPlayerFaceSprite' couldn't return any sprite!");
    }
}
