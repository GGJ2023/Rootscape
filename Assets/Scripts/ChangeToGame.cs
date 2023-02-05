using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeToGame : MonoBehaviour
{
    public Button b;

    public SpriteRenderer title;

    Color tmp;

    bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        tmp = title.GetComponent<SpriteRenderer>().color;

        b
            .onClick
            .AddListener(() =>
            {
                clicked = true;
            });
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Camera.main.gameObject.transform.position.y >= -5.5f &&
            clicked == true
        )
        {
            Camera.main.gameObject.transform.Translate(0f, -0.005f, 0f);
            tmp.a = title.GetComponent<SpriteRenderer>().color.a - 0.003f;
            title.GetComponent<SpriteRenderer>().color = tmp;
        }
        else if (Camera.main.gameObject.transform.position.y <= -5.5f)
        {
            SceneManager.LoadScene(1);
        }
    }
}
