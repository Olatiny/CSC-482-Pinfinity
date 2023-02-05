using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    [SerializeField]
    float fontSize;

    [SerializeField]
    float xFontSize;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() { }

    public void UpdateCombo(float combo)
    {
        if (combo == 1.0f)
        {
            gameObject.SetActive(false);
            return;
        }
        if (combo > 1.51f)
        {
            gameObject.SetActive(true);
            GetComponent<TextMeshProUGUI>().text = combo.ToString();
            GetComponent<TextMeshProUGUI>().fontSize = fontSize + 10 * combo;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = xFontSize + 8 * combo;
            StartCoroutine(ShakeText());
        }
    }

    IEnumerator ShakeText()
    {
        float timeAtStart = Time.timeSinceLevelLoad;
        float animationTime = 0.5f;
        float timeNow = timeAtStart;
        while (timeNow < timeAtStart + animationTime)
        {
            timeNow += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(timeNow * 7) * 10);
            GetComponent<TextMeshProUGUI>().fontSize -= Time.deltaTime * 20;
            GetComponent<TextMeshProUGUI>().fontSize = Mathf.Min(
                fontSize,
                GetComponent<TextMeshProUGUI>().fontSize
            );
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize -= Time.deltaTime * 16;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = Mathf.Min(
                fontSize,
                GetComponent<TextMeshProUGUI>().fontSize
            );
            yield return null;
        }
    }
}
