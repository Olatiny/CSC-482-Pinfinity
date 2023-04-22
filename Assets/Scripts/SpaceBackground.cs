using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBackground : MonoBehaviour
{
    [SerializeField]
    private GameObject SpaceShip;

    [SerializeField]
    private GameObject[] Planets;

    [SerializeField]
    private GameObject[] Stars;

    [SerializeField]
    private float minX;

    [SerializeField]
    private float maxX;

    public void SpawnNewElements(Vector3 spawnPoint, float levelSize)
    {
        float size = Random.Range(0.5f, 2f);

        GameObject p = Planets[Random.Range(0, Planets.Length)];

        // planet.GetComponent<SpriteRenderer>().color *= (size / 2.0f);
        GameObject planet = Instantiate(
            p,
            new Vector3(
                Random.Range(minX, maxX),
                Camera.main.transform.position.y + 7.5f + Random.Range(0.0f, 4.0f),
                10
            ),
            Quaternion.Euler(0, 0, Random.Range(0, 360)),
            gameObject.transform
        );
        planet.transform.localScale = Vector3.one * size;
        planet.GetComponent<Parallax>().depth = 10 / size;
        planet.GetComponent<SpriteRenderer>().color = Color.white * (size / 2.0f);
        for (int i = 0; i < (int)levelSize; i++)
        {
            GameObject s = Stars[Random.Range(0, Stars.Length)];

            GameObject star = Instantiate(
                s,
                new Vector3(
                    Random.Range(minX, maxX),
                    Camera.main.transform.position.y + 7.5f + Random.Range(0.0f, 4.0f),
                    10
                ),
                s.transform.localRotation
            );
            size = Random.Range(0.5f, 2f);
            star.transform.localScale = Vector3.one * size;
            star.GetComponent<Parallax>().depth = 15 / size;
            star.GetComponent<SpriteRenderer>().color = Color.white * (size / 2.0f);
        }
        if (Random.Range(0, 3) > 2)
        {
            size = Random.Range(0.5f, 2f);
            // planet.GetComponent<SpriteRenderer>().color *= (size / 2.0f);
            GameObject ship = Instantiate(
                SpaceShip,
                new Vector3(
                    Random.Range(minX, maxX),
                    Camera.main.transform.position.y + 7.5f + Random.Range(0.0f, 4.0f),
                    10
                ),
                Quaternion.Euler(0, 0, Random.Range(0, 360)),
                gameObject.transform
            );
            ship.transform.localScale = Vector3.one * size;
            ship.transform.GetChild(0).transform.localScale = Vector3.one * size;
            ship.GetComponent<SpaceshipBackground>().depth = 6 / size;
        }
    }
}
