using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0.0f, 40.0f)]
    private float defaultSpeed = 0.0f;

    [SerializeField, Range(40.0f, 180.0f)]
    private float defaultRotationSpeed = 40.0f;

    [SerializeField, Range(0.0f, 60.0f)]
    private float fastSpeed = 0.0f;

    [SerializeField, Range(40.0f, 180.0f)]
    private float fastRotationSpeed = 40.0f;

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

    private TrailRenderer trailRenderer = null;

    private MaterialPropertyBlock mpb;

    private static readonly int
        hydrationProp = Shader.PropertyToID("_HydrationAmount");

    private static float z = 0.0f;

    private void Awake()
    {
        lifetime = maxLifetime;

        trailRenderer = GetComponent<TrailRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (alive)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                move(fastSpeed, fastRotationSpeed);
            }
            else
            {
                move(defaultSpeed, defaultRotationSpeed);
            }
            decrementLifetime();
            updateMaterial();
        }
    }

    public static void ResetZ()
    {
        z = 0.0f;
    }

    private void move(float currentSpeed, float currentRotationSpeed)
    {
        float horizontal = Input.GetAxis("Horizontal");

        // Calculate how much to rotate this frame
        float rotationAmount = -horizontal * currentRotationSpeed * Time.deltaTime;
        Quaternion rotation =
            Quaternion.Euler(Vector3.forward * -rotationAmount);
        RotateDirection (rotation);

        // Change the position based on the direction (normalized) and speed
        Vector2 move = direction * currentSpeed * Time.deltaTime;

        Globals.distanceTravelled += move.magnitude;

        transform.position = transform.position + (Vector3) move;
    }

    private void decrementLifetime()
    {
        lifetime -= Time.deltaTime;

        if (lifetime < 0.0f)
        {
            KillRoot();
        }
    }

    private void updateMaterial()
    {
        trailRenderer.GetPropertyBlock (mpb);
        mpb.SetFloat(hydrationProp, lifetime / maxLifetime);
        trailRenderer.SetPropertyBlock (mpb);
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

        Globals.numberOfSplits += 1;

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
        //Debug.Log (rand);
        Quaternion rotation =
            Quaternion.Euler(Vector3.forward * rotationAmount);

        // Instatiate the new root
        GameObject newRoot = Instantiate(rootPrefab);

        // Set the new root position to the parent
        Vector3 newPos = transform.position;

        // Decrement the zOffset
        z -= 1.0f;

        // To avoid z fighting, set new z to the global z and decrement z
        newPos.z = z;

        newRoot.transform.position = newPos;
        newRoot.transform.rotation = transform.rotation;


        // Set the direction properly for the new root
        PlayerController newPC = newRoot.GetComponent<PlayerController>();
        newPC.SetDirection(rotation * direction);
    }

    public void KillRoot()
    {
        gameObject.tag = "Dead";
        alive = false;
        canSplit = false;

        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        while (lifetime >= 0)
        {
            lifetime -= Time.deltaTime * 3;
            trailRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(hydrationProp, lifetime / maxLifetime);
            trailRenderer.SetPropertyBlock(mpb);
            yield return null;
        }
        trailRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(hydrationProp, 0.0f);
        trailRenderer.SetPropertyBlock(mpb);
    }
}
