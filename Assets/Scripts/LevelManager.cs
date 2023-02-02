using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int current_stage = 0;

    [SerializeField]
    private GameObject[] stage_1;

    [SerializeField]
    private GameObject[] stage_2;

    [SerializeField]
    private GameObject[] stage_3;

    [SerializeField]
    private GameObject[] planes;
    private int stages_num = 3;

    [SerializeField]
    private GameObject spawnPoint;
    private GameObject[][] stages;

    private void Awake()
    {
        stages = new GameObject[stages_num][];
        stages[0] = stage_1;
        stages[1] = stage_2;
        stages[2] = stage_3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BumperRow"))
        {
            GameObject bump = stages[current_stage][Random.Range(0, stages[current_stage].Length)];

            Vector2 pos1 = new Vector3(
                spawnPoint.transform.position.x,
                collision.gameObject.transform.GetChild(0).position.y,
                10
            );
            Vector2 pos2 = spawnPoint.transform.position;
            if (current_stage == 1)
            {
                int chance = Random.Range(0, 3);
                if (chance == 1)
                {
                    Instantiate(
                        planes[0],
                        new Vector3(
                            planes[0].transform.position.x,
                            spawnPoint.transform.position.y,
                            10
                        ),
                        spawnPoint.transform.rotation
                    );
                    Debug.Log("spawned");
                }
                if (chance == 2)
                {
                    Instantiate(
                        planes[1],
                        new Vector3(
                            planes[1].transform.position.x,
                            spawnPoint.transform.position.y,
                            10
                        ),
                        spawnPoint.transform.rotation
                    );
                }
            }
            if (pos1.y > pos2.y)
            {
                Instantiate(bump, pos1, spawnPoint.transform.rotation);
            }
            else
            {
                Instantiate(bump, pos2, spawnPoint.transform.rotation);
            }

            // Debug.Log(bump.name + "was instantiated");
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Despawn"))
    //    {
    //        Destroy(collision.transform.parent.gameObject);
    //    }
    //}
}