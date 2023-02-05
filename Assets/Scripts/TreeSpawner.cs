using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public bool GameSpawner = true;
    public TreeSpawner LevelSpawner;
    public GameObject[] trees;
    public GameObject instance;
    public int next = 0;
    public bool loadNext = true;
    // Start is called before the first frame update
    void Start()
    {
        if(GameSpawner)
        {
            int choice = Random.Range(0, trees.Length); 
            instance = Instantiate(trees[choice], transform.position, transform.rotation);
            tree newTree = instance.GetComponent<tree>();
            newTree.curveAmount = Random.Range(0.0f, 0.0001f); 
            newTree.splitThresholdOne = Random.Range(1.0f, 2.0f); 
            newTree.splitThresholdTwo = Random.Range(2.0f, 4.0f); 
            newTree.splitThresholdThree = Random.Range(3.0f, 5.0f); 
            newTree.splitThresholdFour = Random.Range(4.0f, 6.0f); 
        }
        else
        {
            foreach (GameObject tree in trees)
            {
                if(tree)
                {
                    GameObject newOne = Instantiate(tree, transform.position, transform.rotation);
                    tree newTree = newOne.GetComponent<tree>();
                    newTree.height = 0; 
                }
            }
        }
    }

    void Update()
    {
        if(GameSpawner)
        {
            if(GameObject.FindGameObjectsWithTag("Root").Length <= 0 && loadNext)
            {
                LevelSpawner.trees[LevelSpawner.next % LevelSpawner.trees.Length] = instance;
                ++LevelSpawner.next;
                loadNext = false;
            }
        }
    }
}


