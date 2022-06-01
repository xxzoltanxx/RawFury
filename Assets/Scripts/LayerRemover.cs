using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerRemover : MonoBehaviour
{
    [SerializeField] public bool isRed = true;
    void OnTriggerStay2D(Collider2D other){
        if (isRed && other.gameObject.tag == "RedBox" && other.gameObject.layer == 6)
        {
            other.gameObject.layer = 2;
        }
        else if (!isRed && other.gameObject.tag == "BlueBox" && other.gameObject.layer == 6)
        {
            other.gameObject.layer = 2;
        }
    }
}
