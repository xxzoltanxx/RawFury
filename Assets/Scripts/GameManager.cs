using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject redBox;
    [SerializeField] public GameObject blueBox;
    [SerializeField] public GameObject robot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene("FirstScene", LoadSceneMode.Single);
            return;
        }
        else if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene("SecondScene", LoadSceneMode.Single);
            return;
        }
        
        if (Input.GetKeyDown("space"))
        {
            //I should have probably polled this
            GameObject[] objsRed = GameObject.FindGameObjectsWithTag("RedBox");
            GameObject[] objsBlue = GameObject.FindGameObjectsWithTag("BlueBox");
            GameObject robotInstance = GameObject.FindGameObjectWithTag("Respawn");

            Destroy(robotInstance);

            Instantiate(robot, new Vector3(0,-3,0), Quaternion.identity);

            foreach (GameObject obj in objsRed)
            {
                Destroy(obj);
            }

            foreach (GameObject obj in objsBlue)
            {
                Destroy(obj);
            }

            int boxes = Random.Range(10, 25);

            for (int i = 0; i < boxes; ++i)
            {
                int red = Random.Range(0, 2);

                float positionX = Random.Range(-9.0f, 9.0f);
                float positionY = Random.Range(-2.0f, 4.0f);
                if (red == 1)
                {
                    Instantiate(redBox, new Vector3(positionX, positionY, 0), Quaternion.Euler(0,0, Random.Range(0.0f, 180.0f)));
                }
                else
                {
                    Instantiate(blueBox, new Vector3(positionX, positionY, 0), Quaternion.Euler(0,0, Random.Range(0.0f, 180.0f)));
                }
            }

        }
        
    }
}
