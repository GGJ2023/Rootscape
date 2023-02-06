using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeToGame : MonoBehaviour
{
    public SpriteRenderer title;

    Color tmp;

    private bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        tmp = title.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Space)) clicked = true;
        if (
            Camera.main.gameObject.transform.position.y >= -20.5f &&
            clicked == true
        )
        {
            Camera.main.gameObject.transform.Translate(0f, -0.02f, 0f);
            tmp.a = title.GetComponent<SpriteRenderer>().color.a - 0.003f;
            title.GetComponent<SpriteRenderer>().color = tmp;
        }
        else if (Camera.main.gameObject.transform.position.y <= -20.5f)
        {
            SceneManager.LoadScene(1);
        }
    }
}
