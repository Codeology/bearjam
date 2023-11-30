using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossEnemy : MonoBehaviour
{
    Animator anim;
    public PlayerController player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 direction = player.transform.position - transform.position;

        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
            anim.SetBool("Vertical", true);
        } else {
            anim.SetBool("Vertical", false);
        }
    }

    #region Attack
    public float damage;

    private void Attack()
    {
        //RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
        //foreach (RaycastHit2D hit in hits) {
        //    if (hit.transform.CompareTag("Player")) {
        //        Debug.Log("hits player");
        //        player.TakeDamage(damage);
        //    }
        //}
        player.TakeDamage(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Attack();
        }
    }
    #endregion
}