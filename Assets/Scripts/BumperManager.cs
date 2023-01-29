using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] validBumpers;

    [SerializeField]
    private GameObject spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BumperRow"))
        {
            GameObject bump = validBumpers[Random.Range(0, validBumpers.Length)];
            //GameObject bump = validBumpers[2];

            Vector2 pos1 = collision.gameObject.transform.GetChild(0).position;
            Vector2 pos2 = spawnPoint.transform.position;

            if (pos1.y > pos2.y)
            {
                Instantiate(bump, pos1, spawnPoint.transform.rotation);
            }
            else
            {
                Instantiate(bump, pos2, spawnPoint.transform.rotation);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("Trigger detected in bumper row");

        if (collision.gameObject.CompareTag("Despawn"))
        {
            Destroy(collision.transform.parent.gameObject);
            //Debug.Break();
        }
    }
}
