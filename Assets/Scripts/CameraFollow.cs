using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 2f; 
    [SerializeField] private float yOffset = -1f;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = new Vector3(
                Mathf.Clamp(target.position.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(target.position.y + yOffset, minBounds.y, maxBounds.y),
                -10f 
            );

      
            transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, -10f), new Vector3(minBounds.x, maxBounds.y, -10f));
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, -10f), new Vector3(maxBounds.x, maxBounds.y, -10f));
        Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, -10f), new Vector3(maxBounds.x, minBounds.y, -10f));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, -10f), new Vector3(minBounds.x, minBounds.y, -10f));
    }
}
