using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelsUI : MonoBehaviour
{
    public static PanelsUI I = null;

    private void Awake()
    {
        I = this;
    }

    public GameObject winPanel;
    public GameObject losePanel;
    
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    
    public void ShowWinPanel(string text)
    {
        winText.text = text;
        winPanel.SetActive(true);
    }
    
    public void ShowLosePanel(string text)
    {
        loseText.text = text;
        losePanel.SetActive(true);
    }
}
