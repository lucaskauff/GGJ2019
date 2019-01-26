using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHingeScriptNew : MonoBehaviour
{
    [SerializeField]
    private float force;
    [SerializeField]
    private float inertia;
        private int inertiaStep;
    [SerializeField]
    private float speedLimit;
        private float speed;
        private float speedStep = 10;

    [SerializeField]
    private int startAngle;
    [SerializeField]
    private int angleStep;

    private bool overLimit;
    private float directionSign;

    private int brakeAmount;

    private bool insideBrake;
    private float brakingDirection;
        private int brakingSign;
        private float angleToCheck;

    private HingeJoint2D hingeJoint;

    private JointAngleLimits2D jointAngleLimits = new JointAngleLimits2D();
    private JointMotor2D jointMotor = new JointMotor2D();

    // Start is called before the first frame update
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();

        jointAngleLimits.max = startAngle;
        jointAngleLimits.min = -startAngle;
        hingeJoint.limits = jointAngleLimits;

        jointMotor = hingeJoint.motor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = hingeJoint.connectedBody.transform.position - transform.position;
        float currentAngle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
     
        if (Mathf.Abs(currentAngle) > (startAngle - angleStep) + (angleStep * inertiaStep) && !overLimit)
        {
            inertiaStep++;
            speedLimit += speedStep * inertiaStep;
            force += inertia * inertiaStep;

            jointAngleLimits.max = startAngle + (angleStep * inertiaStep);
            jointAngleLimits.min = -startAngle + (angleStep * -inertiaStep);
            hingeJoint.limits = jointAngleLimits;

            directionSign = Mathf.Sign(direction.x); 

            overLimit = true;
        }

        if(overLimit && Mathf.Abs(currentAngle) > (startAngle - angleStep) + (angleStep * inertiaStep) && Mathf.Sign(direction.x) == -directionSign)
        {
            overLimit = false;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if(horizontalInput != 0)
        {
            brakeAmount = 0;

            if (horizontalInput > 0) speed = Mathf.Lerp(speed, speedLimit + speedStep, force * Time.deltaTime);
            else speed = Mathf.Lerp(speed, -speedLimit - speedStep, force * Time.deltaTime);

            jointMotor.motorSpeed = speed;
        }
        else
        {
            angleToCheck = Mathf.Sign(-direction.x) * (angleStep * inertiaStep);
            
            if(angleToCheck > 0)
            {
                if (currentAngle > angleToCheck)
                {
                    Debug.Log("Changed Sign");
                    angleToCheck = -angleToCheck;
                    brakingSign = -1;
                }
            }
            else
            {
                if (currentAngle < angleToCheck)
                {
                    Debug.Log("Changed Sign");
                    angleToCheck = -angleToCheck;
                    brakingSign = 1;
                }
            }

            speed = Mathf.Lerp(speed, (speedLimit + speedStep) * brakingSign, force * Time.deltaTime);

            jointMotor.motorSpeed = speed;
        }

        hingeJoint.motor = jointMotor;
    }
}
