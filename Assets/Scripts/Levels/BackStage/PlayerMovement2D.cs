using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1.75f;     // nice slow stroll
    public float accel = 10f;
    public float decel = 12f;

    [Header("Facing")]
    public Transform sprite;
    public bool flipXWhenFacingLeft = true;

    Rigidbody2D rb;
    float targetVelX;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal"); // A/D, Left/Right
        targetVelX = input * walkSpeed;

        // Flip sprite
        if (sprite != null && Mathf.Abs(input) > 0.01f)
        {
            var sr = sprite.GetComponent<SpriteRenderer>();
            if (sr != null && flipXWhenFacingLeft)
                sr.flipX = input < 0;
            else
                sprite.localScale = new Vector3(Mathf.Sign(input), 1, 1);
        }
    }

    void FixedUpdate()
    {
        float velX = Mathf.MoveTowards(rb.linearVelocity.x, targetVelX, (Mathf.Abs(targetVelX) > 0.01f ? accel : decel) * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);
    }
}
