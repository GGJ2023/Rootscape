using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // drag your prefab to this field in the inspector
    public float spawnInterval = 1f; // time between spawns in seconds
    private float spawnTimer = 0f; // timer to track when to spawn
    private Camera mainCamera; // reference to the main camera
    private Vector3 cameraBottomLeft; // bottom-left corner of the camera's view
    private Vector3 cameraTopRight; // top-right corner of the camera's view

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
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnTimer = 0f;

            // Track the spawned object and delete it if it goes past the top of the camera's view
            StartCoroutine(TrackAndDelete(spawnedObject));
        }
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
