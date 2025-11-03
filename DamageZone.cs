using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public AudioClip playerHit;
    void OnTriggerStay2D(Collider2D other)
    {

        PlayerController controller = other.GetComponent<PlayerController>();
        // Player health is irrelevant to the damage zone functionality, so the only condition for the health decrease is whether the colliding GameObject has a PlayerController script on it.
        if (controller != null && !controller.Invincible)
        {
            controller.ChangeHealth(-1);
            Debug.Log(controller.Invincible + "EXECUTE NEXT LINE");
            controller.PlaySound(playerHit);
            // playerHit.Play();
        }
    }
}
