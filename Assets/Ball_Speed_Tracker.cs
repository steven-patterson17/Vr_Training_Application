using UnityEngine;

public class Ball_Speed_Tracker : MonoBehaviour
{
    private Rigidbody rb;

    public float Speed { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError($"{gameObject.name} has no Rigidbody!");
        }
    }

    void Update()
    {
        if (rb != null)
        {
            Speed = rb.linearVelocity.magnitude;
        }
    }
}