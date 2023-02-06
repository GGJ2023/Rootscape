using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset = new Vector3(0, -10, -10);
    public float maxZoom = 40f;
    public float minZoom = 10f;
    public float ZoomLimiter = 50f;

    private Vector3 velocity;
    private bool treeZoom = false;
    private bool finale = false;
    private bool canRestart = false;
    public float smoothTime = 0.5f;
    private GameObject[] objectsWithTag;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        treeZoom = false;
    }
    IEnumerator waiter()
    {
        finale = true;
        //Wait for 4 seconds
        yield return new WaitForSeconds(20);
        treeZoom = true;
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(3);
        canRestart = true;
    }

        private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && finale)
        {
            treeZoom = true;
            StartCoroutine(delay());
        }

        if (Input.GetKey(KeyCode.Space) && canRestart)
        {
            SceneManager.LoadScene(1);
            PlayerController.ResetZ();
        }

        objectsWithTag = GameObject.FindGameObjectsWithTag("Root");
        if (objectsWithTag.Length <= 0)
        {

            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
            for (int i = 0; i < spawners.Length; i++)
            {
                Destroy(spawners[i]);
            }
            if (finale == false)
            {
                StartCoroutine(waiter());
            }
            if (treeZoom == true)
            {
                maxZoom = 10f;
                minZoom = 10f;
                ZoomLimiter = 10f;
                offset = new Vector3(0, 0, -10);
                objectsWithTag = GameObject.FindGameObjectsWithTag("Tree");
            }
            else
            {
                maxZoom = 10000f;
                ZoomLimiter = 10000f;
                offset = new Vector3(0, 0, -10);
                objectsWithTag = GameObject.FindGameObjectsWithTag("Dead");
                Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, new Color(0.83f, 0.66f, 0.40f), Time.deltaTime * 0.5f);
            }
        }


        if (targets.Count != objectsWithTag.Length)
        {
            targets.Clear();
            for (int i = 0; i < objectsWithTag.Length; i++)
            {
                targets.Add(objectsWithTag[i].transform);
            }
        }

        if (targets.Count == 0)
            return;

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    void Zoom()
    {
        //Debug.Log(GetGreatestDistance());
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / ZoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, new Vector3(0, 0, -10));
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

}