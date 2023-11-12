using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainObjective : MonoBehaviour
{
    private int tokens;
    public int winAmount;

    public static MainObjective instance;

    public RectTransform winPanel;
    public TMPro.TMP_Text tokenText;

    bool winConditionRate;

    bool hasWon;

    public int Tokens { 
        get => tokens;
        set 
        {
            tokenText.text = "Tridents Produced:" + value.ToString();
            tokens = value; 
        }
    }
    public float sampleTime = 20.0f;
    public float time = 0.0f;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (hasWon)
            return;
        if (winConditionRate)
        {
            time += Time.deltaTime;
            if (time >= sampleTime)
            {
                time -= sampleTime;
                if (Tokens >= winAmount)
                    Win();
                Tokens = 0;
            }
        }
        else
        {
            if (Tokens >= winAmount)
                Win();
        }
    }

    public void Win()
    {
        winPanel.gameObject.SetActive(true);
        hasWon = true;
        // Disable look var, and enable mouse cursor to click continue button
    }
}
