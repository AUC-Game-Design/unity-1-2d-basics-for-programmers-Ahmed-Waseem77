using UnityEngine;
using UnityEngine.InputSystem;


public class Projectile : MonoBehaviour
{
  Rigidbody2D rigidbody2d;


  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Awake()
  {
    rigidbody2d = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
  if(transform.position.magnitude > 100.0f)
    {
      Destroy(gameObject);
    }
  }

  public void Launch(Vector2 direction, float force)
  {
    rigidbody2d.AddForce(direction * force);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    EnemyController enemy = other.GetComponent<EnemyController>();
    Debug.Log("Projectile collision with " + other.gameObject);
    if (enemy != null)
    {
      enemy.Fix();
    }
    Destroy(gameObject);
  }

  void OnCollisionEnter2D(Collision2D other)
  {
    Destroy(gameObject);
  }
}
