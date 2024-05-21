using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locks : MonoBehaviour
{
    [SerializeField]
    string lockID;

    [SerializeField]
    string lockText;

    // Start is called before the first frame update
    void Update()
    {
        isActive();
    }

    public void isActive()
    {
        gameObject.SetActive(
            !GameObject.Find("GameManager").GetComponent<UnlockManager>().isUnlocked(lockID)
        );
    }

    public void updateUI()
    {
        GameObject.Find("SkinDescription").GetComponent<TMPro.TextMeshProUGUI>().text = lockText;
    }
}
