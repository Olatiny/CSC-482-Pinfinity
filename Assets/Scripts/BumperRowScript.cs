using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperRowScript : MonoBehaviour
{
    [SerializeField] private GameObject[] validBumpers;
    [SerializeField] private GameObject spawnPoint;
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Despawn")
        {
            GameObject bump = validBumpers[0];

            Instantiate(bump, spawnPoint.transform.position, spawnPoint.transform.rotation);

            Debug.Break();

            Destroy(gameObject);
        }
    }
}
