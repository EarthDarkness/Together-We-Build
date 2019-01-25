using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniversalNetworkInput;
using NaughtyAttributes;

public class InputPuzzle : MonoBehaviour
{
    public enum PuzzleType
    {
        Press,
        Alternate,
        Combination
    }

    public PuzzleType puzzleType;

    [ShowIf("ShowPress"), ReorderableList]
    public ButtonCode[] randButton = { ButtonCode.A, ButtonCode.B, ButtonCode.X, ButtonCode.Y };

    [ShowIf("ShowCombination"), ReorderableList]
    public ButtonCode[] combButtons = { ButtonCode.A, ButtonCode.LeftBumper, ButtonCode.RightBumper };

    [ShowIf("ShowAlternate"), ReorderableList,OnValueChanged("AlternateMaxTwo")]
    public ButtonCode[] altButton = { ButtonCode.A, ButtonCode.B };

    private List<ButtonCode> wrongButtons = new List<ButtonCode>();

    int buttonSelected;
    [HideIf("ShowCombination"), ReadOnly]
    float percentage = 0;
    [HideIf("ShowCombination")]
    public float percentageIncrease = 10.0f;
    [HideIf("ShowCombination")]
    public float percentageDecrease = 20.0f;

    ButtonCode nextPress;

    [ShowIf("ShowCombination")]
    public float resetTime = 0.5f;
    float resetTimer = 0.0f;
    int combCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttonSelected = UnityEngine.Random.Range(0, randButton.Length);

        int enumSize = Enum.GetNames(typeof(ButtonCode)).Length;
        string[] values = Enum.GetNames(typeof(ButtonCode));
        nextPress = altButton[0];

        for (int i = 0; i < enumSize; i++)
        {
            ButtonCode code = (ButtonCode)Enum.Parse(typeof(ButtonCode), values[i]);
            bool wrong = true;
            for (int j = 0; j < combButtons.Length; j++)
            {
                if (code == combButtons[j])
                {
                    wrong = false;
                    continue;
                }
            }
            if (wrong)
            {
                wrongButtons.Add(code);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (puzzleType)
        {
            case PuzzleType.Press:
                if (UNInput.GetButtonDown(randButton[buttonSelected]))
                {
                    percentage += percentageIncrease;
                }

                percentage -= Mathf.Clamp(percentageDecrease * Time.deltaTime, 0, 100f);
                Debug.Log(percentage);
                break;
            case PuzzleType.Alternate:
                if (UNInput.GetButtonDown(nextPress))
                {
                    nextPress = nextPress != altButton[0] ? altButton[0] : altButton[1];
                    percentage += percentageIncrease;
                }
                percentage -= Mathf.Clamp(percentageDecrease * Time.deltaTime, 0, 100f);
                Debug.Log(percentage);
                break;
            case PuzzleType.Combination:
                if (resetTimer >= resetTime)
                {
                    ResetCombination();
                }

                if (UNInput.GetButtonDown(combButtons[combCount]))
                {
                    combCount++;
                    if (combCount == combButtons.Length)
                    {
                        //Finished Combination.
                        combCount = 0;
                    }
                }
                for (int i = 0; i < wrongButtons.Count; i++)
                {
                    if (UNInput.GetButtonDown(wrongButtons[i]))
                    {
                        ResetCombination();
                        resetTimer = 0.0f;
                    }
                }

                resetTimer += Time.deltaTime;
                break;
        }

    }

    private void ResetCombination()
    {
        resetTimer = 0.0f;
        combCount = 0;
    }

    private void AlternateMaxTwo()
    {
        ButtonCode button1 = altButton[0], button2 = altButton[1];
        altButton = new ButtonCode[2];
        altButton.SetValue(button1, 0);
        altButton.SetValue(button2, 1);
    }

    private bool ShowPress()
    {
        return puzzleType == PuzzleType.Press ? true : false;
    }

    private bool ShowAlternate()
    {
        return puzzleType == PuzzleType.Alternate ? true : false;
    }

    private bool ShowCombination()
    {
        return puzzleType == PuzzleType.Combination ? true : false;
    }

}