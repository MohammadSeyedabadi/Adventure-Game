// Delete the Start and Update functions. This script will only be executed when the player character collides with a health collectible, not at the start of the game or every frame. Your script should now look like this:
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    // At the top of the class, add a public AudioClip type class member:
    public AudioClip collectedClip;
    // OnTriggerEnter2D is a specific Unity function that is called on in the first frame when the physics system detects a GameObject with a Rigidbody component hitting the GameObject collider that is a trigger.
    // The parameter (other) will contain the reference to the collider component that just entered the trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        // This instruction creates a variable that contains the reference to the Player Controller (Script) component on GameObject that collides with the collectible.
        // The PlayerController script is only attached to the PlayerCharacter GameObject, so the variable will only be filled with this reference if the player character collides with the collectible.
        // If a GameObject that doesn’t have this script attached collides with the collectible, this variable will remain empty and return no value (null).
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && player.health < player.maxHealth)
        {
            player.ChangeHealth(1);
            Destroy(gameObject);
            player.PlaySound(collectedClip);
        }
    }
}
