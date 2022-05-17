using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandlerPlayer : HealthHandler {

    public int startArmor;
    [SerializeField]
    protected int armor;
    public int Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
            _lastArmorHitTime = ArmorHitRefresh;
            if (armor <= 0)
            {
                armor = 0;
            }
            UpdateUI();
        }
    }

    protected float ArmorHitRefresh = 5f;

    float _lastArmorHitTime = 0f;

    public delegate void OnHealthChangedDelegate(float v);
    public static OnHealthChangedDelegate OnHealthChanged;

    public delegate void OnArmorChangedDelegate(float v);
    public static OnArmorChangedDelegate OnArmorChanged;

    protected override void Awake()
    {
        base.Awake();
        Armor = startArmor;
        UpdateUI();
    }

    protected virtual void Update()
    {
        if (_lastArmorHitTime > 0f)
            _lastArmorHitTime -= Time.deltaTime;
        else if (_lastArmorHitTime < 0)
        {
            Armor = startArmor;
            _lastArmorHitTime = 0f;
        }
    }

    public override void Damage(int damage)
    {
        int dif = Armor - damage;
        Armor -= damage;
        if (dif < 0)
            Health += dif;
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        OnArmorChanged?.Invoke((float)Armor / (float)startArmor);
        OnHealthChanged?.Invoke((float)Health / (float)startHealth);
    }
}
