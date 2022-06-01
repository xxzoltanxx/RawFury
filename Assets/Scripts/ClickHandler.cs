using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] public GameObject gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void OnMouseDown()
    {
        string coord = gameObject.name;
        string x = "";
        string y = "";
        bool passedDot = false;
        foreach (var character in coord)
        {
            if (character == ',')
            { 
                passedDot = true;
                continue;
            }
            if (passedDot)
            {
                y += character;
            }
            else
            {
                x += character;
            }
        }
        
        gameManager.GetComponent<MazePainter>().PaintPath(int.Parse(x), int.Parse(y));
    }
}
