using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    Vector2 startingVelocity;

    [SerializeField]
    float startingRotation;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(startingVelocity);
        GetComponent<Rigidbody2D>().AddTorque(startingRotation);
    }

    // Update is called once per frame
    void Update() { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
