using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    [SerializeField] public static float distanceTravelled = 0.0f;
    [SerializeField] public static int numberOfSplits = 0;
    [SerializeField]
    private float serializationHelper;
 
    public void OnAfterDeserialize()
    {
        distanceTravelled = serializationHelper;
    }
 
    public void OnBeforeSerialize()
    {
        serializationHelper = distanceTravelled;
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceTravelled = 0.0f;    
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(distanceTravelled.ToString());
        //Debug.Log(numberOfSplits.ToString());
    }
}