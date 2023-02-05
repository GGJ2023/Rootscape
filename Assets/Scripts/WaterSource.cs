using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WaterSource : MonoBehaviour
{
    [SerializeField]
    private float deathTimer = 2.0f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc;
        bool isRoot = collision.gameObject.TryGetComponent<PlayerController>(out pc);
        if (isRoot)
        {
            pc.ResetLifetime();
            pc.Split();
            StartCoroutine(Shrivel());
        }
    }

    private IEnumerator Shrivel()
    {
        // Disable the collider
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;

        Vector3 vel = Vector3.zero;

        // Shrink the water over time
        while (deathTimer > 0.0f)
        {
            deathTimer -= Time.deltaTime;
            Vector3 scale = transform.localScale;
            scale = Vector3.SmoothDamp(scale, Vector3.zero, ref vel, deathTimer);
            transform.localScale = scale;
            yield return null;
        }

        Destroy(gameObject);
    }
}
