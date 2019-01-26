using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHingeScript : MonoBehaviour
{
    private HingeJoint2D hingeJoint;

    [SerializeField]
    private float multiplier;
    [SerializeField]
    private int stepForSpeed;
    [SerializeField]
    private float inertiaFactor;

    private float anglePercentage;
    private const float angleStep = 15;
    JointAngleLimits2D jointAngleLimits = new JointAngleLimits2D();

    JointMotor2D jointMotor = new JointMotor2D();

    private float angle;
    private float segment;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();

        jointAngleLimits = hingeJoint.limits;

        segment = hingeJoint.connectedAnchor.y;
        angle = hingeJoint.limits.max;

        jointMotor = hingeJoint.motor;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            anglePercentage = angleStep * 1;
            jointAngleLimits.max += anglePercentage;
            jointAngleLimits.min -= anglePercentage;

            hingeJoint.limits = jointAngleLimits;
        }

        Vector2 nearPoint = new Vector2(transform.position.x, transform.position.y - segment);
        float hyp = segment / Mathf.Cos(angle * (Mathf.PI / 180));
        float offsetX = Mathf.Sin(angle * (Mathf.PI / 180)) * hyp;
        Vector2 farPoint = new Vector2(nearPoint.x + offsetX, transform.position.y - segment);
            float maxLength = (Vector2.Distance(farPoint, transform.position) - segment);

        Vector2 bodyPos = hingeJoint.connectedBody.transform.position;
        float distanceToBody = Vector2.Distance(bodyPos, transform.position);
        Vector2 directionToBody = bodyPos - (Vector2)transform.position;
        float angleToBody = Mathf.Atan2(directionToBody.x, -directionToBody.y) * Mathf.Rad2Deg;
            float currentLength = (distanceToBody / Mathf.Cos(angleToBody * (Mathf.PI / 180)) - segment);

        float height = currentLength - maxLength;
        float a = stepForSpeed / Mathf.Sqrt(maxLength);
        float Speed = -a * Mathf.Pow(currentLength, 2) + stepForSpeed;


        float direction = Input.GetAxisRaw("Horizontal");
        Debug.Log(Speed);
        //Debug.Log("Speed is : " + speed);

        //Debug.Log(speedDown);

        if(direction != 0)
        {
            jointMotor.motorSpeed = (Speed * Mathf.Sign(direction)) * multiplier;
        }
        else
        {
            float sign = Mathf.Sign(bodyPos.x - transform.position.x);

            /*if (currentLength * 1000 > 0.2f) jointMotor.motorSpeed = (speedDown * -sign * multiplier);
            else jointMotor.motorSpeed = 0;*/

            jointMotor.motorSpeed = Mathf.MoveTowards(jointMotor.motorSpeed, 0, inertiaFactor * Time.deltaTime);
        }

        hingeJoint.motor = jointMotor;
    }
}
