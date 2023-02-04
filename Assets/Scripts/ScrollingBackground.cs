using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 2f;
    
    private float spriteHeight;
    private float screenHeight;
    private Transform bg1;
    private Transform bg2;
    private Vector2 startPosition1;
    private Vector2 startPosition2;
    private Camera mainCam;
    private Transform cameraTransform;
    private SpriteRenderer spriteRenderer;

    private List<Transform> bgList = new List<Transform>();

    private void Start()
    {
        mainCam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteHeight = spriteRenderer.bounds.size.y;
        screenHeight = Camera.main.orthographicSize * 2;
        bg1 = transform.GetChild(0);
        bg2 = transform.GetChild(1);
        startPosition1 = bg1.position;
        startPosition2 = new Vector2(bg2.position.x, bg2.position.y + spriteHeight);
        bg2.position = new Vector2(bg2.position.x, bg1.position.y - spriteHeight);
        cameraTransform = Camera.main.transform;

        bgList.Add(bg1);
        bgList.Add(bg2);
    }

    private void Update()
    {
        cameraTransform.Translate(new Vector3(0, -scrollSpeed * Time.deltaTime, 0));
        //mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, mainCam.orthographicSize + 1, Time.deltaTime * scrollSpeed);

        if (bg1.position.y >= cameraTransform.position.y + spriteHeight)
        {
            bg1.position = new Vector2(bg2.position.x, bg2.position.y - spriteHeight);
        }

        if (bg2.position.y >= cameraTransform.position.y + spriteHeight)
        {
            bg2.position = new Vector2(bg1.position.x, bg1.position.y - spriteHeight);
        }
    }
}
