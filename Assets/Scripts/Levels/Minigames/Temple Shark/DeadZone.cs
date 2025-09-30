using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (other.CompareTag("Player"))
        {
            // Get the name of the active scene
            string currentScene = SceneManager.GetActiveScene().name;

            // Load the scene by its name (restart game)
            SceneManager.LoadScene(currentScene);
        }
    }
}
