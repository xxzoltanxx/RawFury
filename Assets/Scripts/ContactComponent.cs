using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactComponent : MonoBehaviour
{
    private BTData data;

    private void Start() {
        data = transform.parent.parent.gameObject.GetComponent<BTData>();
    }


        private void OnTriggerEnter2D(Collider2D other) {
        if (!data.boxGrabbed && !data.ThrowInProgress && data.allowedGrab)
        {
         if (other.gameObject.layer == 6)
         {
             data.selectedBox = other.gameObject;
             data.allowedGrab = false;
             Debug.Log(data.ThrowInProgress);
             data.boxGrabbed = true;
             HingeJoint2D joint = other.gameObject.AddComponent<HingeJoint2D>();
             joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
             joint.autoConfigureConnectedAnchor = false;
             joint.connectedAnchor = new Vector2(0.5f, 0);
         }
        }
        }

    private void OnTriggerStay2D(Collider2D other) {
        if (!data.boxGrabbed && !data.ThrowInProgress && data.allowedGrab)
        {
         if (other.gameObject.layer == 6)
         {
             data.selectedBox = other.gameObject;
             data.allowedGrab = false;
             Debug.Log(data.ThrowInProgress);
             data.boxGrabbed = true;
             HingeJoint2D joint = other.gameObject.AddComponent<HingeJoint2D>();
             joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
             joint.autoConfigureConnectedAnchor = false;
             joint.connectedAnchor = new Vector2(0.5f, 0);
         }
        }
    }
}
