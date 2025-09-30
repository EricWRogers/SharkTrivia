using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    public float amp;
    public float freq;
    Vector3 initPos;
    
    private void Start()
    {
        initPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, transform.position.z);
    }
}
