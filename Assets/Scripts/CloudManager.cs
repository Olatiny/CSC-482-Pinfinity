using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] big_clouds;

    [SerializeField]
    private GameObject[] small_clouds;
    private float _whenToSpawn;
    private List<GameObject> activeClouds = new List<GameObject>();
    private GameManager gameManager;

    void NextCloudSpawn()
    {
        _whenToSpawn = gameObject.transform.position.y + Random.Range(0.3f, 15.0f);
    }

    void SetupCloud(GameObject newCloud)
    {
        newCloud.transform.parent = Camera.main.transform;
        activeClouds.Add(newCloud);
        int randomCloud = Random.Range(0, 100);
        newCloud.transform.position += new Vector3(0, (randomCloud - 50) / 30, 0);
        newCloud.AddComponent<Despawn>();
        newCloud.AddComponent<Parallax>();
        if (randomCloud % 2 == 0)
        {
            newCloud.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (randomCloud > 80)
        {
            newCloud.transform.localScale *= 3;
            newCloud.AddComponent<Cloud>().depth = (float)Depth.CloseClouds;
        }
        else if (randomCloud < 50)
        {
            newCloud.transform.localScale *= 2;
            newCloud.AddComponent<Cloud>().depth = (float)Depth.FarClouds1;
        }
        else
        {
            newCloud.AddComponent<Cloud>().depth = (float)Depth.FarClouds2;
        }
    }

    void SpawnSmallCloud()
    {
        GameObject newCloud = Instantiate(
            small_clouds[Random.Range(0, small_clouds.Length)],
            new Vector3(Random.Range(-2.5f, 2.5f), transform.position.y, (float)Depth.FarClouds1),
            Quaternion.identity
        );
        SetupCloud(newCloud);
    }

    void SpawnBigCloud()
    {
        GameObject newCloud = Instantiate(
            big_clouds[Random.Range(0, big_clouds.Length)],
            new Vector3(Random.Range(-2.5f, 2.5f), transform.position.y, (float)Depth.CloseClouds),
            Quaternion.identity
        );
        SpawnSmallCloud();
        SpawnSmallCloud();
        SpawnSmallCloud();
        SetupCloud(newCloud);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (gameManager.getBallHeight() > GameManager.SPACE_BOUNDARY) { }
        else if (gameManager.getBallHeight() > GameManager.SKY_BOUNDARY)
        {
            SpawnBigCloud();
            NextCloudSpawn();
        }
        if (_whenToSpawn < gameObject.transform.position.y)
        {
            SpawnSmallCloud();
            SpawnSmallCloud();
            SpawnSmallCloud();
            NextCloudSpawn();
        }
    }
}
