using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Leaving them all in one file because I think it's more comprehensive this way

public enum NodeStatus
{
    NotStarted = 0,
    Running = 1,
    Success = 2,
    Failure = 3
}

public abstract class BTNode
{
    public abstract NodeStatus Tick(float dt);

    public NodeStatus Status {get; set;}

    public virtual void Reset()
    {
        Status = NodeStatus.NotStarted;
    }
}

public class Waiter : BTNode
{
    private float timer = 0;
    private float limit;

    public Waiter(float toWait)
    {
        limit = toWait;
    }
    public override NodeStatus Tick(float dt)
    {
        timer += dt;
        if (timer >= limit)
        {
            timer = 0;
            Status = NodeStatus.Success;
        }
        else
        {
            Status = NodeStatus.Running;
        }
        return Status;
    }
}

public abstract class BTComposite : BTNode
{
    public void addNode(BTNode node)
    {
        Debug.Log(node);
        mNodes.Add(node);
    }
    public override void Reset()
    {
        Status = NodeStatus.NotStarted;
        mCurrentIndex = 0;
        foreach (BTNode node in mNodes)
        {
            node.Reset();
        }
    }
    protected List<BTNode> mNodes = new List<BTNode>();
    protected int mCurrentIndex = 0;
}

public class Selector : BTComposite
{
    public override NodeStatus Tick(float dt)
    {
        NodeStatus nodeStatus = mNodes[mCurrentIndex].Tick(dt);
        if (nodeStatus == NodeStatus.Running || nodeStatus == NodeStatus.Success)
        {
            Status = nodeStatus;
        }
        else if (nodeStatus == NodeStatus.Failure)
        {
            ++mCurrentIndex;
            if (mCurrentIndex == mNodes.Count)
            {
                Status = NodeStatus.Failure;
            }
            else
            {
                Status = NodeStatus.Running;
            }
        }
        return Status;
    }
}

public class Repeater : BTNode
{
    private BTNode repeatedNode;

    public Repeater(BTNode repeated)
    {
        repeatedNode = repeated;
    }
    public override NodeStatus Tick(float dt)
    {
        NodeStatus nodeStatus = repeatedNode.Tick(dt);
        if (nodeStatus == NodeStatus.Failure || nodeStatus == NodeStatus.Success)
        {
            repeatedNode.Reset();
        }
        return NodeStatus.Running;
    }
}

public class Sequence : BTComposite
{
   public override NodeStatus Tick(float dt)
    {
        NodeStatus nodeStatus = mNodes[mCurrentIndex].Tick(dt);
        if (nodeStatus == NodeStatus.Failure || nodeStatus == NodeStatus.Running)
        {
            Status = nodeStatus;
        }
        else if (nodeStatus == NodeStatus.Success)
        {
            Debug.Log("Now Running:");
            Debug.Log(mNodes[mCurrentIndex]);
            ++mCurrentIndex;
            if (mCurrentIndex == mNodes.Count)
            {
                Status = NodeStatus.Success;
            }
            else
            {
                Status = NodeStatus.Running;
            }
        }
        return Status;
    }
}

public class FindBox : BTNode
{
    private GameObject boundObject;

    private BTData boundData;

    private const float raycastDistance = 80.0f;

    private int layerMask;

    public FindBox(GameObject obj)
    {

        boundObject = obj;
        layerMask = LayerMask.GetMask("Boxes");
        boundData = boundObject.GetComponent<BTData>();
    }

    public override NodeStatus Tick(float dt)
    {
        Debug.Log("FindBox Enter");



        RaycastHit2D raycastHitRight = Physics2D.Raycast(new Vector2(boundObject.transform.position.x, boundObject.transform.position.y - 0.1f), new Vector2(1,0), raycastDistance, layerMask);
        RaycastHit2D raycastHitLeft = Physics2D.Raycast(new Vector2(boundObject.transform.position.x, boundObject.transform.position.y - 0.1f), new Vector2(-1, 0), raycastDistance, layerMask);

        if (raycastHitRight.collider == null && raycastHitLeft.collider != null)
        {
            boundData.selectedBox = raycastHitLeft.collider.gameObject;
            boundData.FaceLeft();
            return NodeStatus.Success;
        }
        else if (raycastHitLeft.collider == null && raycastHitRight.collider != null)
        {
            boundData.selectedBox = raycastHitRight.collider.gameObject;
            boundData.FaceRight();
            return NodeStatus.Success;
        }
        else if (raycastHitRight.distance < raycastHitLeft.distance && raycastHitRight.collider != null)
        {
            boundData.selectedBox = raycastHitRight.collider.gameObject;
            boundData.FaceRight();
            return NodeStatus.Success;
        }
        else if (raycastHitLeft.distance < raycastHitRight.distance && raycastHitLeft.collider != null)
        {
            boundData.selectedBox = raycastHitLeft.collider.gameObject;
            boundData.FaceLeft();
            return NodeStatus.Success;
        }
        else
            return NodeStatus.Running;
    }
}

public class ThrowRed : BTNode
{
    private GameObject boundObject;
    private BTData boundData;

    public ThrowRed(GameObject obj)
    {
        boundObject = obj;
        boundData = boundObject.GetComponent<BTData>();
    }

    public override NodeStatus Tick(float dt)
    {
        Debug.Log("Start ThrowRed");
        if (boundData.ThrowInProgress == true)
        {
            Status = NodeStatus.Running;
        }
        else if (Status == NodeStatus.Running && boundData.ThrowInProgress == false)
        {
            Status = NodeStatus.Success;
        }
        else if (boundData.selectedBox.tag == "RedBox")
        {
            boundData.startThrow(-1);
            boundData.ThrowInProgress = true;
            Status = NodeStatus.Running;
        }
        else
        {
            Status = NodeStatus.Failure;
        }
        return Status;
    }
}

public class ThrowBlue : BTNode
{
    private GameObject boundObject;
    private BTData boundData;

    public ThrowBlue(GameObject obj)
    {
        boundObject = obj;
        boundData = boundObject.GetComponent<BTData>();
    }

    public override NodeStatus Tick(float dt)
    {
        if (boundData.ThrowInProgress == true)
        {
            Status = NodeStatus.Running;
        }
        else if (Status == NodeStatus.Running && boundData.ThrowInProgress == false)
        {
            Status = NodeStatus.Success;
        }
        else if (boundData.selectedBox.tag == "BlueBox")
        {
            boundData.startThrow(1);
            boundData.ThrowInProgress = true;
            Status = NodeStatus.Running;
        }
        else
        {
            Status = NodeStatus.Failure;
        }
        return Status;
    }
}

public class GoToBox : BTNode
{
    private GameObject boundObject;
    private BTData boundBTData;
    private const float raycastDistance = 80.0f;
    private int layerMask;


    public GoToBox(GameObject obj)
    {
        boundObject = obj;
        boundBTData = boundObject.GetComponent<BTData>();
        layerMask = LayerMask.GetMask("Boxes");
    }

    public override NodeStatus Tick(float dt)
    {
        Debug.Log("Enter GoToBox");
        float diff = boundBTData.selectedBox.transform.position.x - boundObject.transform.position.x;
        //Right
        if (diff > 0)
        {
            RaycastHit2D raycastHitRight = Physics2D.Raycast(new Vector2(boundObject.transform.position.x, boundObject.transform.position.y - 0.1f), new Vector2(1,0), raycastDistance, layerMask);
            if (boundBTData.boxGrabbed == true)
            {
                Debug.Log("Reached");
                boundBTData.directionToGo = 0;
                Status = NodeStatus.Success;
                boundBTData.allowedGrab = false;
            }
            else if (raycastHitRight.collider == null || raycastHitRight.collider.gameObject.transform.position != boundBTData.selectedBox.transform.position)
            {
                boundBTData.directionToGo = 0;
                Status = NodeStatus.Failure;
                boundBTData.allowedGrab = false;
            }
            else
            {
                boundBTData.directionToGo = 1;
                Status = NodeStatus.Running;
                boundBTData.allowedGrab = true;
            }
        }
        //Left
        else if (diff <= 0)
        {
             RaycastHit2D raycastHitLeft = Physics2D.Raycast(new Vector2(boundObject.transform.position.x, boundObject.transform.position.y - 0.1f), new Vector2(-1, 0), raycastDistance, layerMask);
            if (boundBTData.boxGrabbed == true)
            {
                Debug.Log("Reached");
                boundBTData.directionToGo = 0;
                Status = NodeStatus.Success;
                boundBTData.allowedGrab = false;
            }
            else if (raycastHitLeft.collider == null || raycastHitLeft.collider.gameObject.transform.position != boundBTData.selectedBox.transform.position)
            {
                boundBTData.directionToGo = 0;
                Status = NodeStatus.Failure;
                boundBTData.allowedGrab = false;
            }
            else
            {
                boundBTData.directionToGo = -1;
                Status = NodeStatus.Running;
                boundBTData.allowedGrab = true;
            }
        }
        return Status;
    }
}

