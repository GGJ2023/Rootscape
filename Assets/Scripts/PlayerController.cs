using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0.0f, 20.0f)] private float speed = 0.0f;
    [SerializeField, Range(40.0f, 180.0f)] private float rotationSpeed = 40.0f;
    [SerializeField, Range(1.0f, 3.0f)] private float accel = 1.0f;
    
    private Vector2 direction = Vector2.down;


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate how much to rotate this frame
        float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(Vector3.forward * -rotationAmount);
        RotateDirection(rotation);

        // Calculate how much to accelerate this frame
        ModifySpeed(vertical * accel * Time.deltaTime);

        // Change the position based on the direction (normalized) and speed
        Vector2 move = direction * speed * Time.deltaTime;
        transform.position = transform.position + (Vector3)move;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ModifySpeed(float delta)
    {
        speed += delta;
    }

    public void RotateDirection(Quaternion rot)
    {
        direction = rot * direction;
    }
}
