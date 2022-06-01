using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTData : MonoBehaviour
{
    public GameObject selectedBox;

    public delegate void FaceDelegate();

    public FaceDelegate FaceLeft;
    public FaceDelegate FaceRight;

    public delegate void ThrowDelegate(int dir);
    public ThrowDelegate startThrow;

    public bool ThrowInProgress = false;
    public int directionToGo = 0;

    public bool boxGrabbed = false;

    public bool allowedGrab = false;
}