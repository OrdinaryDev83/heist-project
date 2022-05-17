using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MPUIKIT;

public class BarsUIHandler : MonoBehaviour
{
    public MPImage armorBar;
    public MPImage healthBar;

    private void Awake()
    {
        HealthHandlerPlayer.OnHealthChanged += SetHealth;
        HealthHandlerPlayer.OnArmorChanged += SetArmor;
    }

    public float ArmorBarTarget
    {
        set
        {
            _armorBarTarget = value;
        }
    }

    public float HealthBarTarget
    {
        set
        {
            _healthBarTarget = value;
        }
    }

    float _armorBarTarget, _healthBarTarget;

    void SetHealth(float v)
    {
        _healthBarTarget = v;
    }

    void SetArmor(float v)
    {
        _armorBarTarget = v;
    }

    private void Update()
    {
        armorBar.fillAmount = Mathf.SmoothStep(armorBar.fillAmount, _armorBarTarget, Time.deltaTime * 30f);
        healthBar.fillAmount = Mathf.SmoothStep(healthBar.fillAmount, _healthBarTarget, Time.deltaTime * 30f);
    }
}
