using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Referencia al jugador
    [SerializeField] private float smoothSpeed = 5f; // Velocidad de seguimiento
    [SerializeField] private Vector3 offset; // Desplazamiento de la cámara respecto al jugador

    [SerializeField] private Vector2 minLimits; // Límite inferior izquierdo
    [SerializeField] private Vector2 maxLimits; // Límite superior derecho

    private void LateUpdate()
    {
        if (player == null) return; // Evita errores si el jugador no está asignado

        // Posición deseada con offset
        Vector3 targetPosition = player.position + offset;

        // Aplicar límites a la posición de la cámara
        float clampedX = Mathf.Clamp(targetPosition.x, minLimits.x, maxLimits.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minLimits.y, maxLimits.y);

        // Interpolación suave
        Vector3 finalPosition = new Vector3(clampedX, clampedY, targetPosition.z);
        transform.position = Vector3.Lerp(transform.position, finalPosition, smoothSpeed * Time.deltaTime);
    }
}
