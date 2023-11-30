using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FieldofView fieldOfView;
    private Vector2 last_direction;
    Animator anim;

    #region Movement
    public float movement_speed;
    private float x_direction;
    private float y_direction;
    Rigidbody2D player_rigidbody;

    private void Move()
    {
        anim.SetBool("Moving", true);

        if (x_direction != 0) {
            player_rigidbody.velocity = new Vector2(x_direction * movement_speed, 0);
            if (x_direction == 1) {
                curr_direction = Vector2.right;
            } else {
                curr_direction = Vector2.left;
            }
        }
        else if (y_direction != 0) {
            player_rigidbody.velocity = new Vector2(0, y_direction * movement_speed);
            if (y_direction == 1)
            {
                curr_direction = Vector2.up;
            }
            else {
                curr_direction = Vector2.down;
            }
        }
        else
        {
            player_rigidbody.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
        last_direction = curr_direction;

        anim.SetFloat("DirX", curr_direction.x);
        anim.SetFloat("DirY", curr_direction.y);
    }
    #endregion

    #region Health
    public float max_health;
    private float curr_health;
    public Slider health_bar;

    public void TakeDamage(float value) {
        curr_health = curr_health - value;
        health_bar.value = curr_health / max_health;
        Debug.Log("Health:" + curr_health.ToString());

        if (curr_health <= 0) {
            Die();
        }
    }

    private void Die() {
        Destroy(this.gameObject);

        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }
    #endregion

    #region Attack
    public float damage;
    public float attack_speed;
    float attack_timer;
    public float hitbox_timing;
    public float animation_timing;
    bool is_attacking;
    Vector2 curr_direction;

    private void Attack() {
        attack_timer = attack_speed;
        StartCoroutine(Attack_Routine());
    }

    IEnumerator Attack_Routine() {
        is_attacking = true;
        player_rigidbody.velocity = Vector2.zero;
        Debug.Log("Attacking");
        yield return new WaitForSeconds(hitbox_timing);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(player_rigidbody.position + curr_direction, Vector2.one, 0f, Vector2.zero);


        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag("Enemy")) {
                hit.transform.GetComponent<BasicEnemy>().TakeDamage(damage);
                Debug.Log("Hit enemy");
            }
        }

        yield return new WaitForSeconds(hitbox_timing);
        is_attacking = false;

        yield return null;
    }
    #endregion

    private void Awake()
    {
        player_rigidbody = GetComponent<Rigidbody2D>();
        curr_health = max_health;

        health_bar.value = curr_health / max_health;

        attack_timer = 0;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (is_attacking) {
            return;
        }

        x_direction = Input.GetAxisRaw("Horizontal");
        y_direction = Input.GetAxisRaw("Vertical");
        fieldOfView.SetDirection((new Vector3(last_direction.x,last_direction.y, 0)).normalized);
        fieldOfView.SetOrigin(transform.position);

        Move();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        else {
            attack_timer = attack_timer - Time.deltaTime;
        }
    }
}
