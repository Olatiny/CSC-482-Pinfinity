using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    //[SerializeField] private GameObject screenSwipe;

    private void Update()
    {
        if (Input.anyKey)
        {
            beginTransition();
        }
    }

    private void beginTransition()
    {
        GetComponent<Animator>().Play("IntroFadeOut");
    }

    private void loadMainScene()
    {
        SceneManager.LoadScene("MainGame");
    }
}
