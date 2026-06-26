using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject fireObject;

    private bool fireStarted = false;

    public void StartFire()
    {
        if (!fireStarted)
        {
            fireObject.SetActive(true);
            fireStarted = true;
        }
    }

    public void StopFire()
    {
        fireObject.SetActive(false);
    }
}
