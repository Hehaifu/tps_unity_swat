using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] Image[] gunIcons;
    [SerializeField] GameObject[] gunHighlights;
    [SerializeField] TextMeshProUGUI[] ammoTexts;

    [SerializeField] Color gunIconSelectColor;
    [SerializeField] Color gunIconDeSelectColor;

    [SerializeField] Color textSelectColor;
    [SerializeField] Color textDeSelectColor;
    // Start is called before the first frame update
    void Start()
    {
        selectWeapon(0);
    }

    public void deselectAllWeapons()
    {
        foreach (Image img in gunIcons)
        {
            img.color = gunIconDeSelectColor;
            if (img.sprite == null)
            {
                img.gameObject.SetActive(false);
            }
        }
        foreach(var highlight in gunHighlights)
        {
            highlight.SetActive(false);
        }
        foreach (var text in ammoTexts)
        {
            text.color = textDeSelectColor;
        }
    }

    public void emptyAllWeapon()
    {
        //清空UI中已有的图标，方便接下来填入
        foreach (Image img in gunIcons)
        {
            img.color = gunIconDeSelectColor;
            img.sprite = null;
            img.gameObject.SetActive(false);
        }
        foreach (var highlight in gunHighlights)
        {
            highlight.SetActive(false);
        }
        foreach (var text in ammoTexts)
        {
            text.color = textDeSelectColor;
            text.text = "";
        }
    }
    public void selectWeapon(int idx)
    {
        deselectAllWeapons();
        if (gunIcons[idx].sprite == null)
        {
            print("no weapon exist in position" + idx);
            return;
        }
        gunIcons[idx].color = gunIconSelectColor;
        ammoTexts[idx].color = gunIconSelectColor;
        gunHighlights[idx].SetActive(true);
    }

    public void setAmmoText(int idx, int currentAmmo,int maxAmmo)
    {
        ammoTexts[idx].text = currentAmmo + "/" + maxAmmo;

    }

    public void setWeapon(int idx, Sprite icon)
    {
        gunIcons[idx].sprite = icon;
        gunIcons[idx].gameObject.SetActive(true);
    }

    public void removeWeapon(int idx)
    {
        gunIcons[idx].sprite = null;
        gunIcons[idx].gameObject.SetActive(false);
        ammoTexts[idx].text = "";
        foreach(var highlight in gunHighlights)
        {
            highlight.SetActive(false);
        }
    }

    public void refreshStatus(List<GameObject> rifleInGame, Dictionary<string, int> gunAmmoDictionary)
    {
        emptyAllWeapon();
        //根据枪的排布更新UI
        for(int i = 0; i < rifleInGame.Count; i++)
        {
            var rifle = rifleInGame[i];
            WeaponScript weaponScript = rifle.GetComponent<WeaponScript>();
            gunIcons[i].gameObject.SetActive(true);
            gunIcons[i].sprite = weaponScript.icon;
            string gunName = rifle.GetComponent<WeaponScript>().GetName();
            int CarryAmmo = gunAmmoDictionary[gunName];
            //ammoTexts[i].text = weaponScript.availableAmmo + "/" + weaponScript.ammoCapacity;
            ammoTexts[i].text = weaponScript.availableAmmo + "/" + CarryAmmo;
        }
        deselectAllWeapons();
    } 
}
