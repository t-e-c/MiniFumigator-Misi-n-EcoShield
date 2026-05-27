using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;

    [Header("Distancia")]
    public float distance = 5f;

    [Header("Altura")]
    public float height = 1.5f;

    [Header("Sensibilidad")]
    public float mouseSensitivity = 1f;

    [Header("Límites Verticales")]
    public float minY = -30f;
    public float maxY = 60f;

    private float rotationX;
    private float rotationY;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No hay target asignado en CameraController");
            return;
        }

        Vector3 angles = transform.eulerAngles;

        rotationX = angles.y;
        rotationY = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // INPUT DEL MOUSE
        Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

        rotationX +=
            mouseDelta.x * mouseSensitivity;

        rotationY -=
            mouseDelta.y * mouseSensitivity;

        // LIMITAR ROTACIÓN VERTICAL
        rotationY = Mathf.Clamp(
            rotationY,
            minY,
            maxY
        );

        // ROTACIÓN
        Quaternion rotation =
            Quaternion.Euler(
                rotationY,
                rotationX,
                0f
            );

        // POSICIÓN OBJETIVO
        Vector3 targetPosition =
            target.position +
            Vector3.up * height;

        // POSICIÓN FINAL DE LA CÁMARA
        Vector3 cameraPosition =
            targetPosition -
            rotation * Vector3.forward * distance;

        // APLICAR
        transform.position = cameraPosition;
        transform.rotation = rotation;
    }
}