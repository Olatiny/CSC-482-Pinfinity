using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            GetComponent<Animator>().Play("Fade Out");
        }
    }

    private void loadCutscene()
    {
        SceneManager.LoadScene("IntroCutscene");
    }
}