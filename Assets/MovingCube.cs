using UnityEngine;

public class MovingCube : MonoBehaviour
{

    public Vector3 requiredDirection;

    void Update()
    {
        transform.position += transform.forward * -2.5f * Time.deltaTime;
    }


}
