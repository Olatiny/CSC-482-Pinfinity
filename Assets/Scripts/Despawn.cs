using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private GameObject despawn;

    void Start()
    {
        despawn = GameObject.Find("DespawnPoint");
    }

    void FixedUpdate()
    {
        if (gameObject.transform.position.y < despawn.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
