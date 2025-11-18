using UnityEngine;

public class Dirt : MonoBehaviour
{

    //public SpriteRenderer dirt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Toothbrush"))      //when can change the tag to whateven tool we need... possably a "pick"
        {
            //Debug.Log("touching");
            if(Input.GetMouseButtonDown(0)){
                //Debug.Log("clicked");
                ScoreManager.instance.AddPoint();

                RemoveTween();
                
            }
            // Destroy(gameObject);

        }
    }

    void RemoveTween(){
        // while(Vector3.position.y > -10){
        //     transfrom.Translate(0,-2,0);
        //     transfrom.Rotate(0,0,0.1);
        // }

        Destroy(gameObject);
    }
}
