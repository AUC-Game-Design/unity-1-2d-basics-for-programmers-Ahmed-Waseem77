using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
  public int healthQuantity = -1;

  public AudioClip collectedClip;

  void OnTriggerEnter2D(Collider2D other)
  {
    PlayerController controller = other.GetComponent<PlayerController>();

    if (controller != null)
    {
      if (controller != null && controller.health < controller.maxHealth)
      {
        controller.ChangeHealth(1);
        controller.PlaySound(collectedClip);
        Destroy(gameObject);
      }
    }
  }
}
