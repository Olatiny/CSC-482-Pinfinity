using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperManager : MonoBehaviour
{
    [SerializeField] private GameObject[] validBumpers;
    [SerializeField] private GameObject spawnPoint;
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger detected in bumper row");

        if (collision.gameObject.tag == "BumperRow")
        {
            GameObject bump = validBumpers[0];

            Destroy(collision.gameObject);

            Instantiate(bump, spawnPoint.transform.position, spawnPoint.transform.rotation);

            //Debug.Break();
        }
    }
}
