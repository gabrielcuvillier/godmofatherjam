using UnityEngine;

public class SandController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBase"))
        {
            IABase ia = other.GetComponent<IABase>();
            if (ia != null)
            {
                ia.ChangeTerrainToSand();
            }
        }
    }
}
