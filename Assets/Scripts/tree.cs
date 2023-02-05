using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PlantType
{
    ALTERNATE,
    OPPOSITE,
    BUSH,
    PINE,
};

public class tree : MonoBehaviour
{
    float min(float a, float b)
    {
        return a > b ? b : a;
    }

    float clamp(float f, float a, float b)
    {
        if(a > f) return a;
        if(b < f) return b;
        return f;
    }

    bool randomChance(float threshold)
    {
        if (Random.Range(0.0f, 1.0f) < threshold)
            return true;
        return false;
    }

    [SerializeField] public PlantType type = PlantType.ALTERNATE;

    [SerializeField, Range(0.0f, 10.0f)] public float height = 0.0f;
    [SerializeField, Range(0.0f, 10.0f)] public float multiplier = 1.0f;
    [SerializeField] private GameObject branchPrefab = null;

    [SerializeField, Range(10.0f, 90.0f)] private float minSplitAngle = 30.0f;
    [SerializeField, Range(10.0f, 90.0f)] private float maxSplitAngle = 40.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float maxHeight = 10.0f;
    [SerializeField] public bool curve = false;
    [SerializeField, Range(0.0f, 10.0f)] public float splitThresholdOne = 1.0f;
    [SerializeField, Range(0.0f, 10.0f)] public float splitThresholdTwo = 2.0f;
    [SerializeField, Range(0.0f, 10.0f)] public float splitThresholdThree = 5.0f;
    [SerializeField, Range(0.0f, 10.0f)] public float splitThresholdFour = 6.0f;
    [SerializeField, Range(0.0f, 0.0001f)] public float curveAmount = 0.0001f;
    public float leafSizeMult = 0.1f;

    private Vector2 direction = Vector2.up;
    private bool canSplit = true;

    private Vector3 initial;
    private bool splitUpcomingOne = true;
    private bool splitUpcomingTwo = true;
    private bool splitUpcomingThree = true;
    private bool splitUpcomingFour = true;
    private int deep = 0;
    private bool switched = false;

    private float last = 0;

    private void Awake()
    {
#if UNITY_EDITOR
        if (branchPrefab == null)
        {
            Debug.LogError("Branch prefab not set");
        }
#endif
        initial = transform.position;
    }

    void Update()
    {
        Vector2 curveDirection = new Vector2(curveAmount, 0.0f);
        if(height > 2 && !switched)
            curveDirection = -curveDirection;

        if(curve)
        {
            direction += curveDirection;
        }

        if(height > maxHeight)
        {
            return;
        }

        if(!(last < Globals.distanceTravelled))
            return;
        last = Globals.distanceTravelled;

        height += 0.0005f;

        if(multiplier > 0.05f)
            multiplier = clamp(multiplier - 0.00005f, 0.0f, 2.0f);

        if(multiplier < 0.05f)
            return;

        Vector3 move = height * direction * multiplier;

        transform.position = initial + move;
        transform.localScale = new Vector3(height * leafSizeMult, height * leafSizeMult, transform.localScale.z);

        if(splitUpcomingOne && height > splitThresholdOne)
        {
            splitUpcomingOne = false;
            Split(false);
        }

        if(splitUpcomingTwo && height > splitThresholdTwo)
        {
            splitUpcomingTwo = false;
            Split(true);
        }

        if(splitUpcomingThree && height > splitThresholdThree)
        {
            splitUpcomingThree = false;
            Split(true);
        }

        if(splitUpcomingFour && height > splitThresholdFour)
        {
            splitUpcomingFour = false;
            Split(true);
        }
    }

    public void SetCanSplit(bool val)
    {
        canSplit = val;
    }

    public void SetDirection(Vector2 newDir)
    {
        direction = newDir;
    }

    public void Split(bool right)
    {
        // Don't split if you can't
        if (!canSplit)
        {
            return;
        }
        
        // Get a random rotation angle between the bounds
        float rotationAmount = Random.Range(minSplitAngle, maxSplitAngle);
        if(right)
            rotationAmount = Random.Range(-minSplitAngle, -maxSplitAngle);
        Quaternion rotation = Quaternion.Euler(Vector3.forward * rotationAmount);

        // Instatiate the new root
        GameObject newBranch = Instantiate(branchPrefab, transform.position, transform.rotation);
        tree newTree = newBranch.GetComponent<tree>();
        newTree.direction = Vector3.Normalize(rotation * direction);
        newTree.transform.rotation = rotation;
        if(newTree.direction.y < -0.5)
        {
            newTree.direction.y = -newTree.direction.y;
        }
        newTree.height = 0.0f;

        if(type == PlantType.ALTERNATE)
        {
            newTree.splitThresholdOne = Random.Range(1.0f, 2.5f);
            newTree.splitThresholdTwo = Random.Range(1.0f, 2.5f);
        }
        else if(type == PlantType.OPPOSITE)
        {
            float range = Random.Range(1.0f, 2.5f);
            newTree.splitThresholdOne = range;
            newTree.splitThresholdTwo = range;
        }
        else
        {
            newTree.splitThresholdOne = Random.Range(1.0f, 1.0f);
            newTree.splitThresholdTwo = Random.Range(1.0f, 1.0f);
        }
        newTree.deep = deep + 1;
        if(deep > 4)
        {
            splitUpcomingOne = false;
            splitUpcomingTwo = false;
        }
        else
        {
            newTree.splitUpcomingOne = true;
            newTree.splitUpcomingTwo = true;
        }
    }
}
