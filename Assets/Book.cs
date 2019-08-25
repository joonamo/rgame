using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Book : MonoBehaviour
{
    private Page[] pages;
    private int currPageIdx;
    // Start is called before the first frame update
    void Start()
    {
        pages = GetComponentsInChildren<Page>();
        currPageIdx = 0;

        Debug.Log(pages.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating())
        {
            return;
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            if (currPageIdx == pages.Length)
            {
                SceneManager.LoadScene("jhlevel");
            }
            else
            {
                pages[currPageIdx].Turn();
                currPageIdx++;
            }
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            if (currPageIdx > 0)
            {
                pages[currPageIdx - 1].TurnBack();
                currPageIdx--;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("jhlevel");
        }
    }

    bool isRotating()
    {
        bool rotating = false;
        foreach (var page in pages)
        {
            rotating = rotating || page.IsRotationOn();
        }
        return rotating;
    }
}
