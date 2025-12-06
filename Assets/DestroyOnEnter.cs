using UnityEngine;
using UnityEngine.Events;

public class DestroyOnEnter : MonoBehaviour

{

    public UnityEvent breakCombo;
    void OnTriggerEnter(Collider other)
    {
        breakCombo.Invoke();
        Destroy(other.gameObject);
    }
}
