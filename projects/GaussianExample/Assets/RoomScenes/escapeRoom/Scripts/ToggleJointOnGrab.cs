using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleJointOnGrab : MonoBehaviour
{
    public ConfigurableJoint joint;

    public void JointOn()
    {
        JointDrive drive = new JointDrive
        {
            positionSpring = 500000f,
            positionDamper = 6000f,
            maximumForce = 500f
        };

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        drive = new JointDrive
        {
            positionSpring = 100000f,
            positionDamper = 6000f,
            maximumForce = 200f
        };

        joint.slerpDrive = drive;
    }

    public void JointOff()
    {
        JointDrive drive = new JointDrive
        {
            positionSpring = 0f,
            positionDamper = 0f,
            maximumForce = 0f
        };

        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;
        joint.slerpDrive = drive;
    }
}
