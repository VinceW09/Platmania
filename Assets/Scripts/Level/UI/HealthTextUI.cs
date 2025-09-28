using TMPro;
using UnityEngine;

public class HealthTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    void Update()
    {
        healthText.text = "HEALTH: " + Player.Instance.GetHealth();
    }
}
