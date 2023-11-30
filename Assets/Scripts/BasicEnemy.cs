using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    Animator anim;

    #region Movement
    public float movement_speed;
    Rigidbody2D enemy_rigidbody;
    public Transform player_transform;
    public PlayerController player;

    private void Move()
    {
        Vector2 direction = player_transform.position - transform.position;
        enemy_rigidbody.velocity = direction * movement_speed;

        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            anim.SetBool("Vertical", true);
        }
        else
        {
            anim.SetBool("Vertical", false);
        }
    }
    #endregion

    #region Attack
    public float damage;
    public float radius;

    private void Attack() {
        //RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
        //foreach (RaycastHit2D hit in hits) {
        //    if (hit.transform.CompareTag("Player")) {
        //        Debug.Log("hits player");
        //        player.TakeDamage(damage);
        //    }
        //}
        player.TakeDamage(damage);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")) {
            Attack();
        }
    }
    #endregion

    #region Health
    public float max_health;
    private float curr_health;

    public void TakeDamage(float value)
    {
        curr_health = curr_health - value;

        if (curr_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion

    private void Awake()
    {
        enemy_rigidbody = GetComponent<Rigidbody2D>();
        curr_health = max_health;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player_transform == null)
        {
            return;
        }
        else {
            Move();
        }
    }

}
