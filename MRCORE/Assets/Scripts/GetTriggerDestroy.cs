using UnityEngine;

public class GateTriggerDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            Destroy(other.gameObject); // Poof! Fish gone.
        }
    }
}
