using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{

    public GameObject prefabToSpawn;
    public float spawnInterval = 1f;
    public float minScale = 0.5f;
    public float maxScale = 2f;
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
                                                cameraBottomLeft.y - 2, 0f);
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnTimer = 0f;

            // Randomize the size of the spawned object
            float randomScale = Random.Range(minScale, maxScale);
            spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, 1f);

            // Randomize the color of the spawned object
            Renderer rend = spawnedObject.GetComponent<Renderer>();
            Color originalColor = rend.material.color;
            float colorVariance = Random.Range(-0.2f, 0.2f);
            rend.material.color = new Color(originalColor.r + colorVariance, originalColor.g + colorVariance, originalColor.b + colorVariance, 1f);
        }
    }

}
