using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BugTrain : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;

    [SerializeField, Tooltip("Sort of like follow distance, but not really. Used in smooth damp")]
    private float followTime = 1.0f;
    private Vector2 velocity = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos;
        newPos = Vector2.SmoothDamp(transform.position, target.position, ref velocity, followTime);
        transform.position = newPos;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc;
        bool isRoot = collision.gameObject.TryGetComponent<PlayerController>(out pc);
        if (isRoot)
        {
            pc.KillRoot();
        }
    }
}
