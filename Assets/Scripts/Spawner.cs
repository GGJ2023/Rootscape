using UnityEngine;

public class Spawner : MonoBehaviour
{
    public System.Collections.Generic.List<GameObject> prefabsToSpawn; 
    public float spawnInterval = 1f;
    private float spawnTimer = 0f; 
    private Camera mainCamera; 
    private Vector3 cameraBottomLeft; 
    private Vector3 cameraTopRight; 

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            // Calculate the bottom-left and top-right corners of the camera's view
            cameraBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
            cameraTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

            // Calculate a random position within the bounds of the camera's view and below the bottom of the camera's view
            Vector3 spawnPosition = new Vector3(Random.Range(cameraBottomLeft.x, cameraTopRight.x),
                                                cameraBottomLeft.y-2, 0f);
            GameObject spawnedObject = Instantiate(pickPrefab(), spawnPosition, Quaternion.identity);
            spawnTimer = 0f;

            // Track the spawned object and delete it if it goes past the top of the camera's view
            StartCoroutine(TrackAndDelete(spawnedObject));
        }
    }

    private GameObject pickPrefab()
    {
        int index = Random.Range(0, prefabsToSpawn.Count);
        return prefabsToSpawn[index];
    }

    private System.Collections.IEnumerator TrackAndDelete(GameObject spawnedObject)
    {
        while (spawnedObject.transform.position.y < cameraTopRight.y + 2)
        {
            yield return null;
        }

        Destroy(spawnedObject);
    }
}
