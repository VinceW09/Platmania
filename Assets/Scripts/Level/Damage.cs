using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 25;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == true) return;
        if (collision.GetComponentInParent<Player>() != null)
        {
            Player.Instance.Damage(damageAmount);
        }
    }
}
