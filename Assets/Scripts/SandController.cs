using UnityEngine;

public class SandController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IABase ia = other.GetComponent<IABase>();
            if (ia != null)
            {
                ia.ChangeTerrainToSand();
            }
            else
            {
                IAPoulpe iaPoulpe = other.GetComponent<IAPoulpe>();
                if (iaPoulpe != null)
                {
                    iaPoulpe.ChangeTerrainToSand();
                }
            }
        }
    }
}
