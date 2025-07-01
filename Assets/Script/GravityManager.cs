using UnityEngine;
using System.Collections;

public class GravityManager : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    public float fallForce = 20f;
    public GameObject hologramPrefab;
    public float hologramHeight = 2f;

    public Transform headTransform;
    public float topAngle = 180f;
    public float downAngle = 0f;
    public float leftAngle = 90f;
    public float rightAngle = -90f;

    private GameObject hologramInstance;
    public Camera mainCamera;
    private bool previewActive = false;
    private Vector3 targetGravity = Vector3.down;
    private Rigidbody rb;
    private bool isRotating = false;

    private bool left;
    private bool right;
    private bool top;
    private bool down;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (hologramPrefab)
        {
            hologramInstance = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
            hologramInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetPreview(Vector3.up);
            top = true; down = left = right = false;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetPreview(Vector3.down);
            down = true; top = left = right = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetPreview(Vector3.left);
            left = true; right = top = down = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetPreview(Vector3.right);
            right = true; left = top = down = false;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && previewActive)
        {
            ApplyGravity();
            // RotateAroundHead();
            previewActive = false;
            if (hologramInstance)
                hologramInstance.SetActive(false);
        }

        if (previewActive && hologramInstance)
        {
            hologramInstance.transform.position = transform.position + Vector3.up * hologramHeight;
            hologramInstance.transform.rotation = Quaternion.FromToRotation(Vector3.down, targetGravity);
        }
    }

    void ApplyGravity()
    {
        Vector3 currentGravity = Physics.gravity.normalized;
        Vector3 newGravity = Vector3.zero;
        Vector3 rotationAxis = Vector3.zero;
        float rotationAngle = 0f;

        if (top)
        {
            newGravity = Vector3.up;
            rotationAxis = Vector3.forward;
            rotationAngle = topAngle;
        }
        else if (down)
        {
            newGravity = Vector3.down;
            rotationAxis = Vector3.back;
            rotationAngle = downAngle;
        }
        else if (left)
        {
            newGravity = Vector3.left;
            rotationAxis = Vector3.left;
            rotationAngle = leftAngle;
        }
        else if (right)
        {
            newGravity = Vector3.right;
            rotationAxis = Vector3.right;
            rotationAngle = rightAngle;
        }

        if (currentGravity != newGravity)
        {
            rb.freezeRotation = false;
            Physics.gravity = newGravity * gravityStrength;
            transform.RotateAround(headTransform.position, rotationAxis, rotationAngle);
            mainCamera.transform.RotateAround(headTransform.position, rotationAxis, rotationAngle);

            rb.freezeRotation = true;
            Debug.Log("Gravity applied: " + Physics.gravity);
        }
        else
        {
            Debug.Log("Gravity direction unchanged. No rotation applied.");
        }
    }


    void SetPreview(Vector3 direction)
    {
        targetGravity = direction.normalized;
        previewActive = true;

        if (hologramInstance)
        {
            hologramInstance.SetActive(true);
            hologramInstance.transform.position = transform.position + Vector3.up * hologramHeight;
            hologramInstance.transform.rotation = Quaternion.FromToRotation(Vector3.down, targetGravity);
        }
    }
}
