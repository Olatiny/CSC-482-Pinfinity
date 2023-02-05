using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHeight : MonoBehaviour
{
    [SerializeField]
    private float startHeight;

    [SerializeField]
    private GameObject firstLevel;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0, startHeight, 0);
        firstLevel.transform.position += new Vector3(0, startHeight, 0);
    }
}
