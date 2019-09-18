using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquippedWeapon : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI weaponTitle;
    [SerializeField] UnityEngine.UI.Image weaponIcon;

    public void UpdateWeapon(object newWeapon)
    {
        WeaponStatFile weaponStats = (WeaponStatFile)newWeapon;
        if (weaponStats)
        {
            weaponTitle.text = weaponStats.weaponName;
            weaponIcon.sprite = weaponStats.uiIcon;
        }
    }
}
