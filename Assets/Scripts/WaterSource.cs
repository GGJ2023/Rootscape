using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WaterSource : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc;
        bool isRoot = collision.gameObject.TryGetComponent<PlayerController>(out pc);
        if (isRoot)
        {
            pc.ResetLifetime();
        }
    }
}
