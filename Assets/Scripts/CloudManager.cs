using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    public GameObject[] prefabClouds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Clouds"))
        {
            GameObject cloud = prefabClouds[Random.Range(0, prefabClouds.Length)];

            Vector2 pos1 = collision.gameObject.transform.GetChild(0).position;
            Vector2 pos2 = spawnPoint.transform.position;

            if (pos1.y > pos2.y)
            {
                Instantiate(cloud, pos1, spawnPoint.transform.rotation);
            }
            else
            {
                Instantiate(cloud, pos2, spawnPoint.transform.rotation);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Cloud detected in bumper row");

        if (collision.gameObject.CompareTag("Despawn"))
        {
            Destroy(collision.transform.parent.gameObject);
        }
    }

    class Cloud
    {
        public GameObject gameObjectC;
        public float depth;
    }
}
