using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAI : MonoBehaviour
{
    private BTNode elementaryNode;
    private NodeStatus status = NodeStatus.NotStarted;
    // Start is called before the first frame update
    void Start()
    {
        Sequence elementarySeq = new Sequence();
        Debug.Log(elementaryNode);
        
        FindBox boxFindNode = new FindBox(transform.gameObject);
        GoToBox goToBox = new GoToBox(transform.gameObject);
        Waiter wait = new Waiter(0.4f);

        elementarySeq.addNode(boxFindNode);
        elementarySeq.addNode(goToBox);

        Selector redBlueToss = new Selector();
        
        ThrowRed ThrowRed = new ThrowRed(transform.gameObject);
        ThrowBlue ThrowBlue = new ThrowBlue(transform.gameObject);

        redBlueToss.addNode(ThrowRed);
        redBlueToss.addNode(ThrowBlue);

        elementarySeq.addNode(redBlueToss);
        elementarySeq.addNode(wait);

        elementaryNode = new Repeater(elementarySeq);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(status);
        if (status == NodeStatus.Success || status == NodeStatus.Failure)
        {
            return;
        }
        status = elementaryNode.Tick(Time.deltaTime);
    }
}
