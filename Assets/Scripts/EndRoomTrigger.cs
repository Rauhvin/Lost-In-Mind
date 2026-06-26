using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameTimer timer = FindObjectOfType<GameTimer>();

            if (timer != null)
            {
                timer.StopTimer();

                if (timer.GetTimeRemaining() > 0)
                {
                    SceneManager.LoadScene("Ending - Truth");
                }
                else
                {
                    SceneManager.LoadScene("End1-bad");
                }
            }
            else
            {
                Debug.LogError("Nie znaleziono skryptu GameTimer na scenie");
            }
        }
    }
}