using UnityEngine;

public class Dirt : MonoBehaviour
{

    public SpriteRenderer dirt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Toothbrush"))
        {
            ScoreManager.instance.AddPoint();
            FadeOut();
            Destroy(gameObject);

        }
    }

    public void FadeOut(){
        Color c = dirt.color;
        c.a = 1f;
        while(c.a > 0f){
            c.a -= 0.2f;
            dirt.color = c;
        }
    }
}
