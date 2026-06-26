using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    public FireManager fireManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fireManager.StartFire();
            //Destroy(gameObject);
        }
    }
}
