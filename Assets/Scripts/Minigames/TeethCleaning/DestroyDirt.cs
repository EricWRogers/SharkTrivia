using UnityEngine;

public class DestroyDirt : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("dirty shark");
        
        if (other.CompareTag("Dirt"))
            Destroy(other.gameObject);
            
    }
}
