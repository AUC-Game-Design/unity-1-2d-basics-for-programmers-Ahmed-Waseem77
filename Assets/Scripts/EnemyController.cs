using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public float maxMoveSpeed = 2.0f;
  public float acceleration = 0.2f;
  public bool vertical;
  bool broken = true;

  // patrolling variables
  public float changeTime = 3.0f;
  float timer;
  int direction = 1;

  Animator animator;

  public ParticleSystem smokeEffect;

  public int damage = -1;

  Rigidbody2D rigidbody2d;

  AudioSource audioSource;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    rigidbody2d = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
    timer = changeTime;
  }

  void Update()
  {
    timer -= Time.deltaTime;
    if (timer < 0)
    {
      direction = -direction;
      timer = changeTime;
    }
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if(!broken)
    {
      return;
    }

    Vector2 position = rigidbody2d.position;

    if (vertical)
    {
      animator.SetFloat("Move X", 0);
      animator.SetFloat("Move Y", direction);
      position.y = position.y + maxMoveSpeed * direction * acceleration * Time.deltaTime;
    }
    else
    {
      animator.SetFloat("Move Y", 0);
      animator.SetFloat("Move X", direction);
      position.x = position.x + maxMoveSpeed * direction * acceleration * Time.deltaTime;
    }

    rigidbody2d.MovePosition(position);
    acceleration = (((acceleration * 10.0f) + 1.0f)%10)/10.0f;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    PlayerController player = other.gameObject.GetComponent<PlayerController>();
    if (player != null)
    {
      player.ChangeHealth(damage);
    }
  }



  public void Fix()
  {
    animator.SetTrigger("Fixed");
    broken = false;
    rigidbody2d.simulated = false;
    audioSource.Stop();
    smokeEffect.Stop();
  }
}
