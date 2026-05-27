using UnityEngine;

public class FlyingPatrol : MonoBehaviour
{
    [Header("Puntos")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movimiento")]
    public float speed = 2f;

    private Transform targetPoint;

    void Start()
    {
        targetPoint = pointB;
    }

    void Update()
    {
        // MOVER
        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.deltaTime
            );

        // ROTAR HACIA DIRECCIÓN
        Vector3 direction =
            (targetPoint.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    5f * Time.deltaTime
                );
        }

        // CAMBIAR OBJETIVO
        if (Vector3.Distance(
            transform.position,
            targetPoint.position) < 0.1f)
        {
            if (targetPoint == pointA)
            {
                targetPoint = pointB;
            }
            else
            {
                targetPoint = pointA;
            }
        }
    }
}