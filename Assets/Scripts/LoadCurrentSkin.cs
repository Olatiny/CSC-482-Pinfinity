using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCurrentSkin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        updateSkin();
    }

    public void updateSkin()
    {
        GetComponent<Image>().sprite = GameObject
            .Find("SkinManager")
            .GetComponent<SkinManager>()
            .getSkin();
        Debug.Log("UpdatedSkin");
    }
}
