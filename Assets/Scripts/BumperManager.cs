using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperManager : MonoBehaviour
{
    public int current_stage = 0;

    [SerializeField]
    private GameObject[] stage_1;

    [SerializeField]
    private GameObject[] stage_2;

    [SerializeField]
    private GameObject[] stage_3;

    [SerializeField]
    private GameObject[] stage_4;

    private int stages_num = 4;

    [SerializeField]
    private GameObject spawnPoint;
    private GameObject[][] stages;

    private void Awake()
    {
        stages = new GameObject[stages_num][];
        stages[0] = stage_1;
        stages[1] = stage_2;
        stages[2] = stage_3;
        stages[3] = stage_4;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BumperRow"))
        {
            GameObject bump = stages[current_stage][Random.Range(0, stages[current_stage].Length)];

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

            Debug.Log(bump.name + "was instantiated");
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
