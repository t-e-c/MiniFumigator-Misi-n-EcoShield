using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;

        // 🔥 Dirección relativa a la cámara (CORRECTA)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Evita que el personaje se incline hacia arriba/abajo
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * input.y + right * input.x;

        if (moveDir.magnitude >= 0.1f)
        {
            // Rotación suave hacia donde se mueve
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Movimiento
            transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
        }
    }
}