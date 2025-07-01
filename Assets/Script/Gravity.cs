using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Rigidbody rb;
    public Transform headTransform;

    private Quaternion originalRotation;

    void Start()
    {
        rb.useGravity = false;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Flip the player 180° around the head's right axis
            transform.RotateAround(headTransform.position, headTransform.right, 180f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Reset the player's rotation to the original
            transform.rotation = originalRotation;
        }
    }

    void FixedUpdate()
    {
        ApplyLocalGravity();
    }

    void ApplyLocalGravity()
    {
        Vector3 localGravity = -transform.forward * 9.81f;
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }
}
