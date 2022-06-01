using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum YeetState{
    Baseline = 0,
    RaiseArm = 1,
    RaiseArmHalfway = 2,
    LowerArm = 3,
    Disengage = 4,
}

public enum YeetStateStatus {
    NotStarted = -1,
    Finished = 0,
    Running = 1
}

public class RobotController : MonoBehaviour
{
    [SerializeField] public BTData controllerData;
    [SerializeField] public float robotSpeed = 100.0f;
    [SerializeField] public float armAngularPower = 200.0f; 
    [SerializeField] public AudioClip yeetSound;

    private GameObject arm;
    private GameObject armHolder;
    private Queue<YeetState> yeetingProcedure = new Queue<YeetState>();
    private YeetState armState = YeetState.Baseline;
    private YeetStateStatus armStatus = YeetStateStatus.NotStarted;
    private int angleModifier = 1;

    // Start is called before the first frame update
    void Start()
    {
        armHolder = transform.GetChild(0).gameObject;
        arm = transform.GetChild(0).GetChild(0).gameObject;
        controllerData.startYeet = StartYeet;
        controllerData.FaceLeft = FaceLeft;
        controllerData.FaceRight = FaceRight;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(robotSpeed * Time.deltaTime * controllerData.directionToGo, 0, 0);

    }

    private void FixedUpdate() {
        UpdateArm(Time.deltaTime);
    }

    private void FaceLeft()
    {
        angleModifier = -1;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
    }

    private void FaceRight()
    {
        angleModifier = 1;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
    }

    private void ThrowBox()
    {
        controllerData.selectedBox.GetComponent<Rigidbody2D>().AddForce(new Vector2(10 * angleModifier, 5) , ForceMode2D.Impulse);
        controllerData.selectedBox.GetComponent<Rigidbody2D>().AddTorque(10 * angleModifier);
        controllerData.selectedBox = null;
        controllerData.boxGrabbed = false;
        GetComponent<AudioSource>().PlayOneShot(yeetSound);
    }

    private void UpdateArm(float dt)
    {
        if (armStatus == YeetStateStatus.Running)
        {
            switch (armState)
            {
                case YeetState.RaiseArm:
                    armHolder.transform.Rotate(0,0, armAngularPower * dt * angleModifier);
                    if (armHolder.transform.localEulerAngles.z > 80 &&  armHolder.transform.localEulerAngles.z < (280))
                    {
                        armStatus = YeetStateStatus.Finished;
                    }
                    break;
                case YeetState.Disengage:
                    Destroy(controllerData.selectedBox.GetComponent<HingeJoint2D>());
                    ThrowBox();
                    armStatus = YeetStateStatus.Finished;
                    break;
                case YeetState.LowerArm:
                    armHolder.transform.Rotate(0,0, - armAngularPower * dt * angleModifier);
                    if (armHolder.transform.localEulerAngles.z < 2 || armHolder.transform.localEulerAngles.z > 358)
                    {
                        armStatus = YeetStateStatus.Finished;
                        
                    }
                    break;
                case YeetState.RaiseArmHalfway:
                    armHolder.transform.Rotate(0,0, armAngularPower * dt * angleModifier);
                    if (armHolder.transform.localEulerAngles.z > 40)
                    {
                        armStatus = YeetStateStatus.Finished;
                    }
                    break;
                default:
                    break;
            }
        }
        if (armStatus == YeetStateStatus.Finished)
        {
            if (yeetingProcedure.Count != 0)
            {
                armStatus = YeetStateStatus.Running;
                armState = yeetingProcedure.Dequeue();
            }
            else
            {
                yeetingProcedure = new Queue<YeetState>();
                controllerData.yeetInProgress = false;
                armState = YeetState.Baseline;
                armStatus = YeetStateStatus.NotStarted;
            }
        }
    }

    public void StartYeet(int yeeting)
    {

        if (yeeting == -1)
        {
            FaceLeft();
            yeetingProcedure.Enqueue(YeetState.RaiseArm);
            yeetingProcedure.Enqueue(YeetState.Disengage);
            yeetingProcedure.Enqueue(YeetState.LowerArm);
        }

        else if (yeeting == 1)
        {
            FaceRight();
            yeetingProcedure.Enqueue(YeetState.RaiseArm);
            yeetingProcedure.Enqueue(YeetState.Disengage);
            yeetingProcedure.Enqueue(YeetState.LowerArm);
        }
        armStatus = YeetStateStatus.Running;
        armState = yeetingProcedure.Dequeue();
    }
}
