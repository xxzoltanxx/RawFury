using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ThrowState{
    Baseline = 0,
    RaiseArm = 1,
    RaiseArmHalfway = 2,
    LowerArm = 3,
    Disengage = 4,
}

public enum ThrowStateStatus {
    NotStarted = -1,
    Finished = 0,
    Running = 1
}

public class RobotController : MonoBehaviour
{
    [SerializeField] public BTData controllerData;
    [SerializeField] public float robotSpeed = 100.0f;
    [SerializeField] public float armAngularPower = 200.0f; 
    [SerializeField] public AudioClip ThrowSound;

    private GameObject arm;
    private GameObject armHolder;
    private Queue<ThrowState> ThrowingProcedure = new Queue<ThrowState>();
    private ThrowState armState = ThrowState.Baseline;
    private ThrowStateStatus armStatus = ThrowStateStatus.NotStarted;
    private int angleModifier = 1;

    // Start is called before the first frame update
    void Start()
    {
        armHolder = transform.GetChild(0).gameObject;
        arm = transform.GetChild(0).GetChild(0).gameObject;
        controllerData.startThrow = StartThrow;
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
        GetComponent<AudioSource>().PlayOneShot(ThrowSound);
    }

    private void UpdateArm(float dt)
    {
        if (armStatus == ThrowStateStatus.Running)
        {
            switch (armState)
            {
                case ThrowState.RaiseArm:
                    armHolder.transform.Rotate(0,0, armAngularPower * dt * angleModifier);
                    if (armHolder.transform.localEulerAngles.z > 80 &&  armHolder.transform.localEulerAngles.z < (280))
                    {
                        armStatus = ThrowStateStatus.Finished;
                    }
                    break;
                case ThrowState.Disengage:
                    Destroy(controllerData.selectedBox.GetComponent<HingeJoint2D>());
                    ThrowBox();
                    armStatus = ThrowStateStatus.Finished;
                    break;
                case ThrowState.LowerArm:
                    armHolder.transform.Rotate(0,0, - armAngularPower * dt * angleModifier);
                    if (armHolder.transform.localEulerAngles.z < 2 || armHolder.transform.localEulerAngles.z > 358)
                    {
                        armStatus = ThrowStateStatus.Finished;
                        
                    }
                    break;
                case ThrowState.RaiseArmHalfway:
                    armHolder.transform.Rotate(0,0, armAngularPower * dt * angleModifier);
                    if (armHolder.transform.localEulerAngles.z > 40)
                    {
                        armStatus = ThrowStateStatus.Finished;
                    }
                    break;
                default:
                    break;
            }
        }
        if (armStatus == ThrowStateStatus.Finished)
        {
            if (ThrowingProcedure.Count != 0)
            {
                armStatus = ThrowStateStatus.Running;
                armState = ThrowingProcedure.Dequeue();
            }
            else
            {
                ThrowingProcedure = new Queue<ThrowState>();
                controllerData.ThrowInProgress = false;
                armState = ThrowState.Baseline;
                armStatus = ThrowStateStatus.NotStarted;
            }
        }
    }

    public void StartThrow(int Throwing)
    {

        if (Throwing == -1)
        {
            FaceLeft();
            ThrowingProcedure.Enqueue(ThrowState.RaiseArm);
            ThrowingProcedure.Enqueue(ThrowState.Disengage);
            ThrowingProcedure.Enqueue(ThrowState.LowerArm);
        }

        else if (Throwing == 1)
        {
            FaceRight();
            ThrowingProcedure.Enqueue(ThrowState.RaiseArm);
            ThrowingProcedure.Enqueue(ThrowState.Disengage);
            ThrowingProcedure.Enqueue(ThrowState.LowerArm);
        }
        armStatus = ThrowStateStatus.Running;
        armState = ThrowingProcedure.Dequeue();
    }
}
