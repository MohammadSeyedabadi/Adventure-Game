using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called on the frame after the Projectile GameObject is instantiated, so when the PlayerController script’s Launch function calls Instantiate, this reference is empty. Awake is called when a GameObject is initialized, before the Start function, which will ensure that the reference is in place.
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // You can consider position to be a vector from the center of the game world to the location of the Projectile GameObject. Magnitude is the length of that vector, so the magnitude of the position is the distance to the center of the game world.
        // If the distance is greater than 100, the Projectile GameObject is destroyed.
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    // The function takes a Vector2 variable(direction) and a float variable(force) as its parameters.
    // You’ll use these parameters to move the projectile’s Rigidbody component: the higher the force value, the faster the projectile moves.
    public void Launch(Vector2 direction, float force)
    {
        // The function calls AddForce on the projectile’s Rigidbody component; the calculation for this here is the direction variable multiplied by the force variable.
        // When the force is applied to the Rigidbody component, the physics engine will apply that force and direction to move the Projectile GameObject every frame.
        rigidbody2d.AddForce(direction * force);
    }

    // The projectile will affect the enemy on impact, so you need to detect when it collides with another GameObject.
    // The parameter other stores the Collision2D data associated with the collision.
    void OnTriggerEnter2D(Collider2D other)
    {
        // The first instruction prints a message to the console when the projectile collides with a GameObject, so you can identify the specific GameObject it has collided with.
        // The second instruction destroys the Projectile GameObject on collision. Eventually these projectiles will fix the enemy robots; you’ll add that functionality later in this tutorial.
        // Debug.Log("Projectile collision with " + other.gameObject);
        // This instruction stores a reference to the Enemy Controller (Script) component on the GameObject that collides with the projectile in a variable called enemy, if it has one.
        EnemyController enemy = other.GetComponent<EnemyController>();
        // If enemy is not null (that is, if the OnTriggerEnter2D function has an EnemyController script reference), the Fix function in the Enemy Controller script is called.
        if (enemy != null)
        {
            enemy.Fix();
        }
        Destroy(gameObject);
    }

    // Since the previous function only handles collisions with triggers, you need to create a new one to handle collisions with standard colliders, like those used for objects, obstacles, and similar elements. This way, the projectile will be properly destroyed on impact.
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
