using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineofSight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            GetComponentInParent<BasicEnemy>().player_transform = collision.transform;
            Debug.Log("sees player");
        }
    }
}
