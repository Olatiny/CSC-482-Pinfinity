// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BalloonSpawner : MonoBehaviour
// {
//     private GameManager gameManager;

//     [SerializeField]
//     private GameObject[] balloons;

//     [SerializeField]
//     private GameObject[] particleEffects;
//     private int[] balloonIDs = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 2 };

//     void Start()
//     {
//         gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//         StartCoroutine(SpawnBalloons());
//     }

//     IEnumerator SpawnBalloons()
//     {
//         while (gameManager.getBallHeight() < GameManager.SKY_BOUNDARY)
//         {
//             int balloonID = balloonIDs[(int)Random.Range(0, 10)];
//             GameObject newBallon = GameObject.Instantiate(balloons[balloonID]);
//             GameObject particleEffect = particleEffects[balloonID];
//             int balloonSize = Random.Range(1, 3);
//             newBallon.transform.localScale = new Vector3(balloonSize, balloonSize, balloonSize);
//             newBallon.GetComponent<Balloon>().balloon_speed = 1.0f + Random.Range(0.0f, 1.0f);
//             newBallon.GetComponent<CircleCollider2D>().enabled = false;
//             newBallon.transform.position =
//                 GameObject.Find("DespawnPoint").transform.position
//                 + new Vector3(Random.Range(-5.0f, 5.0f), 2.5f, 0);
//             StartCoroutine(popBalloon(newBallon, particleEffect));
//             yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
//             Debug.Log("hi");
//         }
//     }

//     IEnumerator popBalloon(GameObject balloon, GameObject particleEffect)
//     {
//         yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));
//         GameObject.Destroy(Instantiate(particleEffect, gameObject.transform), 1.0f);
//         balloon.GetComponent<Balloon>().popped = true;
//     }
// }
