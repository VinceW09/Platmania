using System;
using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nicknamePreviewText;
    [SerializeField] private TextMeshProUGUI namePreviewText;
    [SerializeField] private TextMeshProUGUI playerIdText;
    [SerializeField] private Image playerPreviewFace;
    [SerializeField] private Image playerPreviewColor;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject textEnter;
    [SerializeField] private TextMeshProUGUI inputFieldPlaceholder;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject playerEditTab;
    [SerializeField] private GameObject faces;
    [SerializeField] private GameObject colors;
    [SerializeField] private Image editPlayerFace;
    [SerializeField] private Image editPlayerColor;

    public struct PlayerData : INetworkSerializable
    {
        public string name;
        public string nickName;
        public string faceId;
        public string colorId;
        public string playerId;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            string safeName = name ?? string.Empty;
            string safeNickName = nickName ?? string.Empty;
            string safeFaceId = faceId ?? string.Empty;
            string safeColorId = colorId ?? string.Empty;
            string safePlayerId = playerId ?? string.Empty;

            serializer.SerializeValue(ref safeName);
            serializer.SerializeValue(ref safeNickName);
            serializer.SerializeValue(ref safeFaceId);
            serializer.SerializeValue(ref safeColorId);
            serializer.SerializeValue(ref safePlayerId);

            name = safeName;
            nickName = safeNickName;
            faceId = safeFaceId;
            colorId = safeColorId;
            playerId = safePlayerId;
        }
    }

    private PlayerData playerData;

    [SerializeField] private SpriteListSO playerColors;
    [SerializeField] private SpriteListSO playerFaces;

    private void OnEnable()
    {
        playerData = new PlayerData();
        StartupProcedure();
    }

    public void OnEditNickameClick()
    {
        buttons.SetActive(false);
        textEnter.SetActive(true);

        inputFieldPlaceholder.text = "Enter nickname...";
    }

    public void OnEditNameClick()
    {
        buttons.SetActive(false);
        textEnter.SetActive(true);

        inputFieldPlaceholder.text = "Enter name...";
    }

    public void Confirm()
    {
        if (inputFieldPlaceholder.text == "Enter nickname...")
        {
            playerData.nickName = inputField.text;
            PlayerPrefs.SetString("player_nickname", playerData.nickName);
        }

        if (inputFieldPlaceholder.text == "Enter name...")
        {
            playerData.name = inputField.text;
            PlayerPrefs.SetString("player_name", playerData.name);
        }

        PlayerPrefs.Save();

        inputField.text = "";

        buttons.SetActive(true);
        textEnter.SetActive(false);

        StartupProcedure();
    }

    public void Cancel()
    {
        buttons.SetActive(true);
        textEnter.SetActive(false);
    }

    private void StartupProcedure()
    {
        if (PlayerPrefs.GetString("player_nickname") != "")
        {
            playerData.nickName = PlayerPrefs.GetString("player_nickname");
        }
        else
        {
            PlayerPrefs.SetString("player_nickname", "NICKNAME");
            playerData.nickName = "NICKNAME";
        }

        if (PlayerPrefs.GetString("player_name") != "")
        {
            playerData.name = PlayerPrefs.GetString("player_name");
        }
        else
        {
            PlayerPrefs.SetString("player_name", "REAL NAME");
            playerData.name = "REAL NAME";
        }


        if (PlayerPrefs.GetString("player_face_id") != "")
        {
            playerData.faceId = PlayerPrefs.GetString("player_face_id");
        }
        else
        {
            PlayerPrefs.SetString("player_face_id", "happy");
            playerData.faceId = "happy";
        }

        if (PlayerPrefs.GetString("player_color_id") != "")
        {
            playerData.colorId = PlayerPrefs.GetString("player_color_id");
        }
        else
        {
            PlayerPrefs.SetString("player_color_id", "yellow");
            playerData.colorId = "yellow";
        }

        if (PlayerPrefs.GetString("player_id") != "")
        {
            playerData.playerId = PlayerPrefs.GetString("player_id");
        }
        else
        {
            playerData.playerId = GenerateRandomPlayerId(20);
            PlayerPrefs.SetString("player_id", playerData.playerId);
        }


        nicknamePreviewText.text = playerData.nickName.ToUpper();
        namePreviewText.text = playerData.name.ToUpper();

        playerIdText.text = "PLAYER ID: " + playerData.playerId;

        playerPreviewColor.sprite = GetPlayerColorSprite(playerData.colorId);
        editPlayerColor.sprite = GetPlayerColorSprite(playerData.colorId);

        playerPreviewFace.sprite = GetPlayerFaceSprite(playerData.faceId);
        editPlayerFace.sprite = GetPlayerFaceSprite(playerData.faceId);

        PlayerPrefs.Save();
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

    public void SetPlayerColorButton(SpriteSO sprite)
    {
        PlayerPrefs.SetString("player_color_id", sprite.id);
        StartupProcedure();
    }

    public void SetPlayerFaceButton(SpriteSO sprite)
    {
        PlayerPrefs.SetString("player_face_id", sprite.id);
        StartupProcedure();
    }

    private string GenerateRandomPlayerId(int idLenght)
    {
        StringBuilder stringBuilder = new StringBuilder();
        const string authorizedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        for (int i = 0; i < idLenght; i++)
        {
            stringBuilder.Append(authorizedCharacters[UnityEngine.Random.Range(0, authorizedCharacters.Length)]);
        }

        string randomPlayerId = stringBuilder.ToString();

        return randomPlayerId;
    }

    public void PlayerEditButton()
    {
        buttons.SetActive(false);
        playerEditTab.SetActive(true);
    }

    public void PlayerEditBackButton()
    {
        playerEditTab.SetActive(false);
        buttons.SetActive(true);
    }

    public void FacesButton()
    {
        faces.SetActive(true);
        colors.SetActive(false);
    }

    public void ColorsButton()
    {
        faces.SetActive(false);
        colors.SetActive(true);
    }
}
