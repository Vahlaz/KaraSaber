using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [Tooltip("Time in seconds before this object is destroyed")]
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
