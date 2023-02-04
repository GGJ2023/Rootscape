using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float smoothTime = 0.3f;
    private Vector2 velocity = Vector2.zero;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        transform.position = Vector2.SmoothDamp(transform.position,
                                                transform.position + (Vector3)move,
                                                ref velocity,
                                                smoothTime);
    }
}
