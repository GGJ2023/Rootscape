using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Bug : MonoBehaviour
{
    [SerializeField]
    private float moveRange = 10.0f;

    [SerializeField, Tooltip("Units/sec")]
    private float moveSpeed = 5.0f;

    private Vector2 dest = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private float timeToDest = 0.0f;

    #region Gizmo
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, moveRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, dest);
    }*/
    #endregion

    private void Start()
    {
        SetNewRandDestination();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        Vector2 newPos;
        newPos = Vector2.SmoothDamp(transform.position, dest, ref velocity, timeToDest);
        transform.position = newPos;

        timeToDest -= Time.deltaTime;

        if (Vector2.Distance(newPos, dest) < 0.01f)
        {
            SetNewRandDestination();
        }

    }

    private void SetNewRandDestination()
    {
        Vector2 offset = Random.insideUnitCircle * moveRange;
        dest = (Vector2)transform.position + offset;

        timeToDest = offset.magnitude / moveSpeed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc;
        bool isRoot = collision.gameObject.TryGetComponent<PlayerController>(out pc);
        if (isRoot)
        {
            FindObjectOfType<SFXManager>().Play("rockSFX");
            pc.KillRoot();
        }
    }
}
