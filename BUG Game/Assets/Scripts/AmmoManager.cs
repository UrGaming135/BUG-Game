using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    private PlayerController player;
    private RangedWeapon weapon;
    private int remainingAmmo;
    private int ammoInMag;

    private TextMeshProUGUI ammoValue;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.GetComponent<PlayerController>();
        ammoValue = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            weapon = player.currentWeapon;
        }
        if (weapon != null)
        {
            SetAmmoValueText();

        }
    }

    void SetAmmoValueText()
    {
        remainingAmmo = weapon.remainingAmmo;
        ammoInMag = weapon.ammoInMag;

        var ammoValueText = $"{ammoInMag} / {remainingAmmo}";
        ammoValue.text = ammoValueText;
    }
}
