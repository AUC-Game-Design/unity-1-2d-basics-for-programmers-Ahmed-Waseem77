using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  public bool debugMovementFlag = false;
  public InputAction MoveAction;

  public bool debugHealthFlag = false;
  public int maxHealth = 5;
  int currentHealth;

  public GameObject projectilePrefab;

  public InputAction talkAction;

  Animator animator;
  Vector2 moveDirection = new Vector2(1,0);

  AudioSource audioSource;

  Rigidbody2D rigidbody2d;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    MoveAction.Enable();
    talkAction.Enable();
    rigidbody2d = GetComponent<Rigidbody2D>();
    currentHealth = maxHealth;
    animator = GetComponent<Animator>();
  }

  public float moveSpeed = 0.01f; // Speed of the player movement
  public float baseMoveSpeed = 0.01f; // Speed of the player movement
  private bool sprinted = false;

  public float sprintDuration = 2.0f;
  public float sprintLerpDuration = 2.0f;
  public float sprintMultiplier = 1.15f;

  Vector2 move;

  // Update is called once per frame
  void Update()
  {
    move = MoveAction.ReadValue<Vector2>().normalized;

    // stop walking audio
    if (move == null) {
      audioSource.Stop();
    }
    if (debugMovementFlag) Debug.Log(move);

    if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y,0.0f))
    {
      moveDirection.Set(move.x, move.y);
      moveDirection.Normalize();
    }
    animator.SetFloat("Look X", moveDirection.x);
    animator.SetFloat("Look Y", moveDirection.y);
    animator.SetFloat("Speed", move.magnitude);

    if (isInvincible)
    {
      damageCooldown -= Time.deltaTime;
      if (damageCooldown < 0)
      {
        isInvincible = false;
      }
    }

    if(Input.GetKeyDown(KeyCode.C))
    {
      Launch();
    }

    if(Input.GetKeyDown(KeyCode.Z)) {
      if(sprinted) {
        moveSpeed = baseMoveSpeed;
        sprinted = false;
      } else {
        moveSpeed *= sprintMultiplier;
        sprinted = true;
      }
    }

    if (Input.GetKeyDown(KeyCode.X))
    {
      FindFriend();
    }
  }

  // Fixed Update is better for physics calculations as it its done at regular intervals
  void FixedUpdate()
  {
    Vector2 position = (Vector2)rigidbody2d.position + move * moveSpeed * Time.deltaTime;
    rigidbody2d.MovePosition(position);
  }

  public float timeInvincible = 2.0f;
  bool isInvincible;
  float damageCooldown;

  public void ChangeHealth (int amount)
  {
    if (amount < 0)
    {
      animator.SetTrigger("Hit");
      if (isInvincible)
      {
        return;
      }
      isInvincible = true;
      damageCooldown = timeInvincible;
    }
    currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    if (debugHealthFlag) Debug.Log("Changed Health: " + currentHealth + "/" + maxHealth);
  }

  public int health { get { return currentHealth; }}

  void Launch()
  {
    GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    Projectile projectile = projectileObject.GetComponent<Projectile>();
    projectile.Launch(moveDirection, 300);
    animator.SetTrigger("Launch");
  }

  void FindFriend()
  {
    RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
    if (hit.collider != null)
    {
      NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
      if (character != null)
      {
          UIHandler.instance.DisplayDialogue();
      }
    }
  }

  public void PlaySound(AudioClip clip)
   {
     audioSource.PlayOneShot(clip);
   }
}
