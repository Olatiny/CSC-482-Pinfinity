using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] skins;

    public void setCurrentSkin(Sprite update)
    {
        PlayerPrefs.SetString("Skin", update.name);
        updateUI();
    }

    public Sprite getSkin()
    {
        string skinName = PlayerPrefs.GetString("Skin");
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].GetComponent<SpriteRenderer>().sprite.name == skinName)
            {
                return skins[i].GetComponent<SpriteRenderer>().sprite;
            }
        }
        return null;
    }

    public GameObject getBall()
    {
        string skinName = PlayerPrefs.GetString("Skin");
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].GetComponent<SpriteRenderer>().sprite.name == skinName)
            {
                return skins[i];
            }
        }
        return null;
    }

    public void updateUI()
    {
        GameObject.Find("SkinDescription").GetComponent<TMPro.TextMeshProUGUI>().text = getBall()
            .GetComponent<BallDescription>()
            .desc;
    }
}
