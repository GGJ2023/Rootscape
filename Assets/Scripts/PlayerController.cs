using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0.0f, 20.0f)]
    private float speed = 0.0f;

    [SerializeField, Range(40.0f, 180.0f)]
    private float rotationSpeed = 40.0f;

    [SerializeField, Range(1.0f, 3.0f)]
    private float accel = 1.0f;

    [SerializeField]
    private GameObject rootPrefab = null;

    [SerializeField, Range(10.0f, 90.0f)]
    private float minSplitAngleLeft = 30.0f;

    [SerializeField, Range(10.0f, 90.0f)]
    private float maxSplitAngleLeft = 90.0f;

    [SerializeField, Range(-10.0f, -90.0f)]
    private float maxSplitAngleRight = -30.0f;

    [SerializeField, Range(-10.0f, -90.0f)]
    private float minSplitAngleRight = -90.0f;

    [SerializeField]
    private float maxLifetime = 8.0f;

    private Vector2 direction = Vector2.down;

    private bool canSplit = true;
    
    private bool alive = true;

    private float lifetime = 0.0f;

    private void Awake()
    {
#if UNITY_EDITOR
        if (rootPrefab == null)
        {
            Debug.LogError("Root prefab not set");
        }
#endif
        lifetime = maxLifetime;
    }

    void Update()
    {
        if (alive)
        {
            handleInput();
            decrementLifetime();
        }
    }

    private void handleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate how much to rotate this frame
        float rotationAmount = horizontal * rotationSpeed * Time.deltaTime;
        Quaternion rotation =
            Quaternion.Euler(Vector3.forward * -rotationAmount);
        RotateDirection(rotation);

        // Calculate how much to accelerate this frame
        ModifySpeed(vertical * accel * Time.deltaTime);

        // Change the position based on the direction (normalized) and speed
        Vector2 move = direction * speed * Time.deltaTime;
        transform.position = transform.position + (Vector3)move;


        // Debug for split
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Split();
        }
    }

    private void decrementLifetime()
    {
        lifetime -= Time.deltaTime;

        if (lifetime < 0.0f)
        {
            KillRoot();
        }
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ModifySpeed(float delta)
    {
        speed += delta;
    }

    public void SetDirection(Vector2 newDir)
    {
        direction = newDir;
    }

    public void RotateDirection(Quaternion rot)
    {
        direction = rot * direction;
    }

    public void ResetLifetime()
    {
        lifetime = maxLifetime;
    }

    public void SetCanSplit(bool val)
    {
        canSplit = val;
    }

    public void Split()
    {
        // Don't split if you can't
        if (!canSplit)
        {
            return;
        }

        float rand = Random.Range(-1, 1);
        float rotationAmount;

        // Get a random rotation angle between the bounds
        if (rand < 0)
        {
            rotationAmount = Random.Range(minSplitAngleLeft, maxSplitAngleLeft);
        }
        else
        {
            rotationAmount =
                Random.Range(minSplitAngleRight, maxSplitAngleRight);
        }
        Debug.Log (rand);
        Quaternion rotation =
            Quaternion.Euler(Vector3.forward * rotationAmount);

        // Instatiate the new root
        GameObject newRoot = Instantiate(rootPrefab);
        newRoot.transform.position = transform.position;
        newRoot.transform.rotation = transform.rotation;

        // Set the direction properly for the new root
        PlayerController newPC = newRoot.GetComponent<PlayerController>();
        newPC.SetDirection(rotation * direction);

        // Set the speed to be equal
        newPC.SetSpeed (speed);

        // Don't allow the new root to split
        //newPC.SetCanSplit(false);
    }

    public void KillRoot()
    {
        alive = false;
        canSplit = false;
    }
}
