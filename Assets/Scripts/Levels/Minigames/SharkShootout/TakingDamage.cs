using UnityEngine;
using System.Collections;

public class TakingDamage : MonoBehaviour

{
    public EndGame endgame;
    public float flashDuration = 0.1f; 
    public int flashCount = 3; 
    private SpriteRenderer spriteRenderer;
    
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found!");
        }
        
    }

    
    public void TakeDamage(int hitCount)
     {
        
         {
             StartCoroutine(DamageFlashRoutine());
         }
     }

    IEnumerator DamageFlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashDuration / 2);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashDuration / 2);
        }
        spriteRenderer.enabled = true;
    }
}


