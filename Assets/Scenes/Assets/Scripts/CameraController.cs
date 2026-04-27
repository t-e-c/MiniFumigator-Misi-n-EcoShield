using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 10f;
    public float mouseSensitivity = 100f;
    public float smoothSpeed = 10f;

    public float minY = -30f;
    public float maxY = 60f;

    float rotationX = 0f;
    float rotationY = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float mouseX = Mouse.current.delta.ReadValue().x * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.ReadValue().y * mouseSensitivity * Time.deltaTime;

        rotationX += mouseX;
        rotationY -= mouseY;

        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}