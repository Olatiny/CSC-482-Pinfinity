using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperManager : MonoBehaviour
{
    [SerializeField] private GameObject[] validBumpers;
    [SerializeField] private GameObject spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BumperRow"))
        {
            GameObject bump = validBumpers[0];
            Instantiate(bump, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger detected in bumper row");

        if (collision.gameObject.CompareTag("Despawn"))
        {
            Destroy(collision.transform.parent.gameObject);
            //Debug.Break();
        }
    }
}
