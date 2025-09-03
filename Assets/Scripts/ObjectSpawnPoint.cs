using UnityEngine;

public class ObjectSpawnPoint : MonoBehaviour
{
    public bool IsPointAvailable { get; set; } = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
