using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Components
    private CharacterController Controller;
    public Transform Cam;
    private Animator animPlayer;
    private AudioSource asPlayer;

    // Audio Clips
    public AudioClip punch;
    public AudioClip hit;
    public AudioClip blocked;

    // Attack Parameters
    public Transform attackPoint;
    public Transform attackPoint2;
    public LayerMask enemyLayer;
    public float attackRange = 0.5f;
    public int attackDamage = 10;

    // Gameplay Parameters
    public bool gameOver = false;
    public float Speed = 100.0f;
    private bool isBlocking = false;
    private bool isHurt = false;
    private bool isAttacking = false;
    private bool isAttacking2 = false;
    private bool isKicking = false;
    private bool isKicking2 = false;
    private float timeBetweenAttacks = 0.75f;
    private float attackTimer = 0f;

    // Health Parameters
    private int currentHealth;
    public int maxHealth = 100;
    public GameObject hitParticlePrefab;
    public GameObject blockParticlePrefab;
    public GameObject gameOverUI;
    public HealthBarUI healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        Controller = GetComponent<CharacterController>();
        asPlayer = GetComponent<AudioSource>();
        animPlayer = GetComponent<Animator>();
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanPerformActions())
        {
            PlayerMovement();
        }

        HandleAttackInput();
        HandleBlockInput();
    }

    bool CanPerformActions()
    {
        return isAttacking || isAttacking2 || isHurt || isBlocking || isKicking || isKicking2 || gameOver;
    }

    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isHurt && !isBlocking && !gameOver)
        {
            if (!isAttacking && !isAttacking2)
            {
                StartCoroutine(PerformAttackCombo());
            }
            else if (isAttacking && !isAttacking2)
            {
                StartCoroutine(PerformSecondAttack());
            }
            else
            {
                StartCoroutine(PerformThirdAttack());
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isHurt && !isBlocking && !gameOver)
        {
            if (!isKicking && !isKicking2)
            {
                StartCoroutine(PerformKickCombo());
            }
            else if (isKicking && !isKicking2)
            {
                StartCoroutine(PerformSecondKick());
            }
            else
            {
                StartCoroutine(PerformThirdKick());
            }
        }

        if (isAttacking || isAttacking2 || isKicking || isKicking2)
        {
            HandleAttackCooldown();
            return;
        }
    }

    void HandleBlockInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBlocking = true;
            animPlayer.SetBool("Block", true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isBlocking = false;
            animPlayer.SetBool("Block", false);
        }
    }

    void HandleAttackCooldown()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            isAttacking = false;
            isKicking = false;
            attackTimer = 0f;
        }
        UpdateAnimatorParameters(0f, 0f);
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;

        Vector3 movement = Cam.transform.right * horizontal + Cam.transform.forward * vertical;
        movement.y = 0f;

        if (movement.magnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
        }

        Controller.Move(movement);

        if (movement.magnitude != 0f)
        {
            UpdateAnimatorParameters(horizontal, vertical);
        }
    }

    void UpdateAnimatorParameters(float horizontal, float vertical)
    {
        float absHorizontal = Mathf.Abs(horizontal);
        float absVertical = Mathf.Abs(vertical);
        float speed = Mathf.Max(absHorizontal, absVertical);

        Debug.Log("Speed (Before Modification): " + speed);

        // Modify the speed if necessary

        Debug.Log("Speed (After Modification): " + speed);

        animPlayer.SetFloat("Speed", speed);
    }

    private IEnumerator PerformAttackCombo()
    {
        isAttacking = true;

        animPlayer.SetTrigger("Punch1");

        yield return new WaitForSeconds(timeBetweenAttacks);

        isAttacking = false;
    }
    private IEnumerator PerformSecondAttack()
    {
        isAttacking2 = true;

        animPlayer.SetTrigger("Punch2");

        yield return new WaitForSeconds(timeBetweenAttacks);

        isAttacking2 = false;
    }
    private IEnumerator PerformThirdAttack()
    {
        isAttacking = true;
        animPlayer.SetTrigger("Punch3");

        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
    //KICK COMBOS
        private IEnumerator PerformKickCombo()
    {
        isKicking = true;

        animPlayer.SetTrigger("Kick1");

        yield return new WaitForSeconds(timeBetweenAttacks);

        isKicking = false;
    }
    private IEnumerator PerformSecondKick()
    {
        isKicking2 = true;

        animPlayer.SetTrigger("Kick2");

        yield return new WaitForSeconds(timeBetweenAttacks);

        isKicking2 = false;
    }
    private IEnumerator PerformThirdKick()
    {
        isKicking = true;
        animPlayer.SetTrigger("Kick3");

        yield return new WaitForSeconds(timeBetweenAttacks);
        isKicking = false;
    }
    private void HitBoxEvent()
    {
        asPlayer.PlayOneShot(punch);
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyFollow enemyFollow = enemy.GetComponent<EnemyFollow>();

            if (enemyFollow != null)
            {
                enemyFollow.TakeDamage(attackDamage);

                asPlayer.PlayOneShot(hit);

            }
        }
    }
    private void HitBoxEvent2()
    {
        asPlayer.PlayOneShot(punch);
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint2.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyFollow enemyFollow = enemy.GetComponent<EnemyFollow>();

            if (enemyFollow != null)
            {
                enemyFollow.TakeDamage(attackDamage);

                asPlayer.PlayOneShot(hit);

            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (isBlocking )
        {
            isHurt = true;
            animPlayer.SetTrigger("BlockStun");
            asPlayer.PlayOneShot(blocked);
            Vector3 spawnPosition = transform.position + Vector3.up + new Vector3(0f, 1f, 1f);
            Instantiate(blockParticlePrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            isHurt = true;
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            animPlayer.SetTrigger("Hurt");
            asPlayer.PlayOneShot(hit);
            Vector3 spawnPosition = transform.position + Vector3.up + new Vector3(0f, 1f, 1f);
            Instantiate(hitParticlePrefab, spawnPosition, Quaternion.identity);

            if (currentHealth <= 0)
            {
                die();
            }

        }
        UpdateAnimatorParameters(0f, 0f);
        return;
    }
    private void HurtEnd()
    {
        isHurt = false;
    }

    private void die() 
    {
        gameObject.layer = 0;
        gameObject.tag = "Untagged";
        gameOver = true;
        animPlayer.SetBool("Dead", true);
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(attackPoint2.position, attackRange);
    }
}