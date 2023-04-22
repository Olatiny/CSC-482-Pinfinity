using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        //SoundManager.instance.PlayBackground(SoundManager.instance.arcade);
    }

    private void Update()
    {
        if (Input.anyKeyDown && !IsPointerOverUIObject())
        {
            GetComponent<Animator>().Play("Fade Out");
            loadCutscene();
        }
    }

    private bool IsPointerOverUIObject()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
        {
            Touch touch = Input.GetTouch(touchIndex);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return true;
        }

        return false;
    }

    private void loadCutscene()
    {
        SceneManager.LoadScene("IntroCutscene");
    }
}
