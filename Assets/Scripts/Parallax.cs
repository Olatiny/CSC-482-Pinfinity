using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float depth;
    private Vector3 originalSpawn;
    private float originalOffset;
    float originalCameraHieght;

    // Start is called before the first frame update
    void Start()
    {
        originalSpawn = transform.position;
        originalCameraHieght = Camera.main.transform.position.y;
        originalOffset = transform.position.y - Camera.main.transform.position.y;
        depth = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Transform cameraTransform = Camera.main.transform;
        float cameraY = cameraTransform.position.y;
        transform.position = new Vector3(
            transform.position.x,
            (
                cameraY
                + originalOffset
                - ((Camera.main.transform.position.y - originalCameraHieght) / depth)
            ),
            originalSpawn.z
        );
    }
}
