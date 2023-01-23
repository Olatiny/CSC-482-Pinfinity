using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private int force;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Vector2 dir = (collision.gameObject.transform.position - transform.position).normalized;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x * force, dir.y * force), ForceMode2D.Force);
        }
    }
}
