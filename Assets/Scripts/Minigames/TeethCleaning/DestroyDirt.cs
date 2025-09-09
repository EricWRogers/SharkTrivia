using UnityEngine;
//using Systems.Collections;

public class DestroyDirt : MonoBehaviour
{
    public float fadeDuration = 2f;
    //public Renderer rend;

    void start(){
    }


    void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("dirty shark");
        
        if (other.CompareTag("Dirt"))
            Destroy(other.gameObject);
            
    }


}
