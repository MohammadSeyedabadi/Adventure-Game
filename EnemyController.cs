using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AudioClip playerHit;
    public AudioSource footstepSource;
    public AudioSource FixAudio;
    // The enemies start off broken and need to be fixed by the player — this variable stores that state and sets the enemy to initialize in the broken state.
    bool broken = true;
    Animator animator;
    // changeTime is a float variable, which is a good fit for countdowns.
    public float changeTime = 3.0f;
    float timer = 3;
    int direction = 1;
    public bool vertical = true;
    public float speed = 1;
    Rigidbody2D rigidbody2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        footstepSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        timer = changeTime;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        // You’ve used if statements to perform checks before — here the check is whether the broken bool is not (!) true.
        // If broken is not true (the enemy is fixed), Unity exits the function. If the enemy is still broken, the rest of the code in the function will execute.
        if (!broken)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            // The Animator.SetFloat function sends float values to the Animator component to affect transitions between animations.
            // When the enemy moves vertically, 0 is sent to the horizontal parameter and the direction variable controls whether the enemy moves up or down.
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
            player.PlaySound(playerHit);
        }
    }

    // This function is public so you can call it from the Projectile script.
    // The first instruction sets the broken bool variable to false.
    // The second instruction sets the simulated property of the Enemy GameObject’s Rigidbody component to false, which removes the Enemy GameObject from the physics system’s simulations for collisions. This change means that projectiles will no longer collide with enemies or be able to damage the enemy character.
    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        footstepSource.Stop();
        FixAudio.PlayOneShot(FixAudio.clip);
        animator.SetTrigger("Fixed");
    }
}
