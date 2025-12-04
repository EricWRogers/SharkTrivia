using UnityEngine;

public class Dirt : MonoBehaviour
{
    public static int dirtRemaining = 0;

    //public SpriteRenderer dirt;
    void Start(){
        dirtRemaining++;
        var col = GetComponent<Collider2D>();
        if (col is PolygonCollider2D poly)
            poly.SetPath(0, poly.GetPath(0)); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        //if (other.CompareTag("Toothbrush"))      //when can change the tag to whateven tool we need... possably a "pick"
        {
            //Debug.Log("touching");
            //if(Input.GetMouseButtonDown(0)){
                //Debug.Log("clicked");
                //ScoreManager.instance.AddPoint();

                //RemoveTween();
                
           // }
            // Destroy(gameObject);

        }
    }
    void OnDestroy(){
        dirtRemaining--;
        ProgressBar pb = FindFirstObjectByType<ProgressBar>();
        if (pb != null)
            pb.Win();

    }
    void OnMouseDown()
    {
        
        if (ToolManager.ActiveToolName == "Pick")
        {
            Destroy(gameObject);
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
