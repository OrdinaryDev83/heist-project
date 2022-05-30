using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class GameCyclesManager : MonoBehaviour
{
    public static GameCyclesManager I = null;

    private void Awake()
    {
        I = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    string GetPanelText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<b>Statistics</b>");
        sb.Append("\n");
        sb.Append(string.Concat("Bags - ", "<b>$", Utils.FormatNumber(EscapeVan.I.MoneyInVan), "</b>", "x", EscapeVan.I.BagsInVan));
        sb.Append("\n");
        sb.Append(string.Concat("Quick Cash - ", "<b>$", Utils.FormatNumber(PlayersManager.QuickMoney), "</b>"));
        sb.Append("\n");
        sb.Append(string.Concat("Kills - ", "<b>$", Utils.FormatNumber(PlayersManager.Kills), "</b>"));
        sb.Append("\n");
        sb.Append(string.Concat("Hostages Killed - ", "<b>$", Utils.FormatNumber(PlayersManager.HostageKilled), "</b> -$", Utils.Money_HostagePenalty((int)WaveManager.I.difficulty, PlayersManager.HostageKilled)));
        sb.Append("\n\n");
        sb.Append("<b>Progression</b>");
        sb.Append("\n");
        sb.Append(string.Concat("XP - ", "<b>$", Utils.FormatNumber(Utils.ProcessXP(PlayersManager.Kills, (int)WaveManager.I.difficulty, EscapeVan.I.BagsInVan, PlayersManager.HostageKilled)), "</b>"));
        sb.Append("\n");
        sb.Append(string.Concat("Money - ", "<b>$", Utils.FormatNumber(Utils.ProcessMoney((int)WaveManager.I.difficulty, EscapeVan.I.BagsInVan, PlayersManager.HostageKilled)), "</b>"));
        return sb.ToString();
    }

    public void OnSuccess()
    {
        PanelsUI.I.ShowWinPanel(GetPanelText());
    }

    public void OnFail()
    {
        PanelsUI.I.ShowLosePanel(GetPanelText());
    }
}
