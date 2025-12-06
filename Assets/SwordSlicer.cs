using UnityEngine;
using EzySlice;
using UnityEngine.Events;
using Unity.VisualScripting;

public class SwordSlicer : MonoBehaviour
{
    [Header("Sword color (must match cube tag)")]
    public string swordColorTag = "Red"; // Set to "Red" or "Blue" (matches cube tag)

    [Header("Direction gating")]
    public float minSpeed = 0.5f;     // don�t slice on tiny taps
    public float directionDot = 0.5f; // 1 = perfect alignment, 0 = perpendicular

    [Header("Slice visuals/physics")]
    public Material crossSectionMaterial;     // material for the cut surface
    public float pieceExplosionForce = 2.5f;  // small kick so halves separate

    [Header("Events")]
    public UnityEvent breakSuccess;

    private Vector3 lastPos;
    private Vector3 velocity;

    private AudioSource audioPlayer;

    void Update()
    {
        audioPlayer = GetComponent<AudioSource>();
        velocity = (transform.position - lastPos) / Mathf.Max(Time.deltaTime, 1e-5f);
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Color must match: cube's tag must equal swordColorTag
        if (!other.CompareTag(swordColorTag))
        {
            // Wrong color: no slice
            return;
        }

        var cube = other.GetComponent<MovingCube>();
        if (!cube) return;

        // Speed gate
        if (velocity.magnitude < minSpeed) return;

        // Direction gate
        Vector3 swingDir = velocity.normalized;
        if (Vector3.Dot(swingDir, cube.requiredDirection) < directionDot) return;


        breakSuccess.Invoke();
        audioPlayer.Play();

        // --- Define the slice plane ---
        Vector3 planePos = other.ClosestPoint(transform.position);

        // Motion-swept plane (uses sword orientation + motion)
        // Adjust bladeEdgeDir axis to match your saber model's edge direction
        Vector3 bladeEdgeDir = transform.up;
        Vector3 planeNormal = Vector3.Cross(swingDir, bladeEdgeDir).normalized;



        // --- Slice with EzySlice ---
        var pieces = other.gameObject.SliceInstantiate(planePos, planeNormal, crossSectionMaterial);
        if (pieces == null || pieces.Length == 0) return;

        // Add colliders & rigidbodies so the halves behave
        foreach (var p in pieces)
        {
            var mf = p.GetComponent<MeshFilter>();

            // Collider: MeshCollider (convex) is most accurate; BoxCollider is cheaper on mobile
            var mc = p.AddComponent<MeshCollider>();
            mc.sharedMesh = mf.sharedMesh;
            mc.convex = true;

            var rb = p.AddComponent<Rigidbody>();
            rb.mass = 1f;

            // Auto-cleanup
            p.AddComponent<DestroyAfterSeconds>();

            // Nudge them apart along the plane normal
            rb.AddForce(planeNormal * pieceExplosionForce, ForceMode.Impulse);
        }

        // Remove original
        Destroy(other.gameObject);
    }
}
