using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFollow : MonoBehaviour
{

    private Animator animEnemy;

    public GameObject hitParticlePrefab;
    public Transform target;
    public float speed = 6.5f;
    public Rigidbody rb;
    private BoxCollider boxCollider;
    public float fightRange = 3f;

    public Transform attackPoint;
    public Transform attackPoint2;
    public Transform attackPoint3;
    public LayerMask playerLayer;
    public float attackRange = .5f;
    public float attackRange2 = .5f;
    public float attackRange3 = .5f;
    public int attackDamage = 10;
    private bool isAttacking = false;
    private bool isHurt = false;
    private GameObject player;
    public int maxHealth = 100;
    int currentHealth;

    private string[] attackTriggers = { "attack", "attack2", "attack3" };
    public void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        currentHealth = maxHealth;
        target = GameObject.Find("Fighter").transform;
        animEnemy = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
    }

    public void Update()
        {
        CheckPlayerTag();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(player.transform.position, rb.position);

                if (distance <= fightRange && !isAttacking)
                {
                    isAttacking = true;

                    string randomAttackTrigger = attackTriggers[Random.Range(0, attackTriggers.Length)];

                    animEnemy.SetTrigger(randomAttackTrigger);
                    StartCoroutine(AttackCooldown());
                }
            }
    }
    private void CheckPlayerTag()
    {
        if (player != null)
        {
            if (player.tag != "Player")
            {
                animEnemy.SetFloat("Speed", 0f);
            }
        }
        else
        {
            // Handle the case when the player object is null (e.g., player went out of scope)
            animEnemy.SetFloat("Speed", 0f);
        }
    }
    void EnemyHitBox()
    {

        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider player in hitPlayer)
        {
            PlayerController playerCon = player.GetComponent<PlayerController>();

            if (playerCon != null)
            {
                playerCon.TakeDamage(attackDamage);
            }
        }
    }

    void EnemyHitBox2()
    {

        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint2.position, attackRange2, playerLayer);

        foreach (Collider player in hitPlayer)
        {
            PlayerController playerCon = player.GetComponent<PlayerController>();

            if (playerCon != null)
            {
                playerCon.TakeDamage(attackDamage);
            }
        }
    }

    void EnemyHitBox3()
    {

        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint3.position, attackRange3, playerLayer);

        foreach (Collider player in hitPlayer)
        {
            PlayerController playerCon = player.GetComponent<PlayerController>();

            if (playerCon != null)
            {
                playerCon.TakeDamage(attackDamage);
            }
        }
    }
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    void FollowPlayer()
    {
        if (animEnemy != null)
        {
            animEnemy.SetFloat("Speed", 1f);
        }
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        rb.MovePosition(pos);
        transform.LookAt(target);

    }


    void OnTriggerStay(Collider player)
    {

        if (player.tag == "Player" && !isHurt && !isAttacking)
        {
            FollowPlayer();
        }

    }

    private void OnTriggerExit(Collider player)
    {

        if (player.tag == "Player")
        {
            if (animEnemy != null)
            {
                animEnemy.SetFloat("Speed", 0f);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        isHurt = true;

        currentHealth -= damage;

        animEnemy.SetTrigger("Hurt");


        Vector3 spawnPosition = transform.position + Vector3.up + new Vector3(0f, 1f, 01f);
        Instantiate(hitParticlePrefab, spawnPosition, Quaternion.identity);

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }


    }
    private IEnumerator Die()
    {
        animEnemy.SetTrigger("Dead");
        isHurt = true;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(2.75f);
        Destroy(gameObject);
    }


    void HurtEnd()
    {
        isHurt = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);
        Gizmos.DrawWireSphere(attackPoint3.position, attackRange3);
    }

}