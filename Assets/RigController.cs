using UnityEngine;

public class RigController : MonoBehaviour
{
    public Transform headIKTarget;
    public Transform kyleRig;
    public Transform headBone;

    public Transform vrCamera;

    public Transform leftIKTarget;
    public Transform rightIKTarget;

    public Transform rightController;
    public Transform leftController;


    void LateUpdate()
    {
        kyleRig.rotation = Quaternion.Euler(0f, vrCamera.eulerAngles.y, 0f);

        kyleRig.position += headIKTarget.position - headBone.position;

        leftIKTarget.SetPositionAndRotation(leftController.position, leftController.rotation);
        rightIKTarget.SetPositionAndRotation(rightController.position, rightController.rotation);
    }

}
