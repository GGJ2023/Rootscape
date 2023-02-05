using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] trees;
    // Start is called before the first frame update
    void Start()
    {
        int choice = Random.Range(0, trees.Length); 
        GameObject newOne = Instantiate(trees[choice], transform.position, transform.rotation);
        tree newTree = newOne.GetComponent<tree>();
        newTree.curveAmount = Random.Range(0.0f, 0.0001f); 
        newTree.splitThresholdOne = Random.Range(1.0f, 2.0f); 
        newTree.splitThresholdTwo = Random.Range(2.0f, 4.0f); 
        newTree.splitThresholdThree = Random.Range(3.0f, 5.0f); 
        newTree.splitThresholdFour = Random.Range(4.0f, 6.0f); 
    }
}
