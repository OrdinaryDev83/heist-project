using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MPUIKIT;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI I = null;
    private void Awake() {
        I = this;
        FillerState = false;
        SetInteractionTextState = false;
    }

    public TextMeshProUGUI ammoCount;
    public TextMeshProUGUI healthCount;

    public TextMeshProUGUI interactionText;

    public InventoryHandler inv;

    public TextMeshProUGUI interactionPercentText;
    public MPImage percentFill;

    public TextMeshProUGUI objectiveText;

    public TextMeshProUGUI failText;

    public MPImage assaultStateImage;
    public TextMeshProUGUI assaultStateText;

    public TextMeshProUGUI moneyText;

    private void Start()
    {
        PlayersManager.OnPlayerSpawned += OnPlayerSpawned;
    }

    void OnPlayerSpawned(Transform player)
    {
        inv = player.GetComponent<InventoryHandler>();
    }

    public void SetInteractionFiller(float percent01) {
        interactionPercentText.text = (Mathf.CeilToInt(percent01 * 100f)).ToString();
        percentFill.fillAmount = percent01;
    }

    public bool FillerState {
        set {
            if(percentFill.gameObject.activeSelf != value)
                percentFill.gameObject.SetActive(value);
        }
    }

    private void Update() {
        if (inv)
        {
            FirearmBase f = inv.GetCurrent();
            int a = f.maxMagAmmo;
            int b = f.MagAmmo;
            int c = f.Ammo;
            ammoCount.text = string.Concat(b, "/", a, "/", c);
        }
        objectiveText.text = string.Concat("Objective : ", ObjectiveManager.I.GetCurrentObj().label);
        moneyText.text = Utils.FormatNumber(PlayersManager.QuickMoney + EscapeVan.I.MoneyInVan);
    }

    public void SetInteractionText(Vector3 pos, string text) {
        SetInteractionTextState = true;
        interactionText.text = text;
        interactionText.GetComponent<RectTransform>().position = pos;
    }

    public string SetFailText {
        set {
            failText.text = value;
        }
    }

    public bool SetInteractionTextState {
        set {
            interactionText.gameObject.SetActive(value);
        }
    }

    public void SetAssaultState(WaveManager.AssaultState state, float timeLeft)
    {
        switch (state)
        {
            case WaveManager.AssaultState.Pause:
                assaultStateImage.color = new Color(1f, 1f, 1f, 0.6980392f);
                assaultStateText.text = Mathf.RoundToInt(timeLeft).ToString();
                assaultStateText.color = Color.white;
                break;
            case WaveManager.AssaultState.Running:
                assaultStateImage.color = new Color(0.9622642f, 0.9569985f, 0.2133321f, 0.6980392f);
                assaultStateText.text = Mathf.RoundToInt(timeLeft).ToString();
                assaultStateText.color = Color.yellow;
                break;
            default:
                break;
        }
    }
}
