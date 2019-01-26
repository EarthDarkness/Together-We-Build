using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniversalNetworkInput;

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

    [ShowIf("ShowCombination"), ReorderableList, ReadOnly]
    public ButtonCode[] combButtons = { ButtonCode.A, ButtonCode.LeftBumper, ButtonCode.RightBumper };

    [ShowIf("ShowAlternate"), ReorderableList, OnValueChanged("AlternateMaxTwo"), ReadOnly]
    public ButtonCode[] altButton = { ButtonCode.A, ButtonCode.B };

    private ButtonCode[] availableButtons = { ButtonCode.A, ButtonCode.B, ButtonCode.LeftBumper, ButtonCode.RightBumper, ButtonCode.DPadDown, ButtonCode.DPadUp, ButtonCode.DPadLeft, ButtonCode.DPadRight };

    private ButtonCode[][] altButtonCombinations = { 
       new ButtonCode[] { ButtonCode.A, ButtonCode.B },
       new ButtonCode[] { ButtonCode.LeftBumper, ButtonCode.RightBumper },
       new ButtonCode[] { ButtonCode.X, ButtonCode.Y },
       new ButtonCode[] { ButtonCode.X, ButtonCode.B },
       new ButtonCode[] { ButtonCode.DPadLeft, ButtonCode.DPadRight },
       new ButtonCode[] { ButtonCode.DPadUp, ButtonCode.DPadDown }
    };

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

    public bool isComplete = false; 

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
        if (!isComplete)
        {
            switch (puzzleType)
            {
                case PuzzleType.Press:
                    if (UNInput.GetButtonDown(randButton[buttonSelected]))
                    {
                        percentage += percentageIncrease;
                        if (percentage >= 100.0f)
                        {
                            isComplete = true;
                        }
                    }

                    percentage -= percentageDecrease * Time.deltaTime;
                    percentage = Mathf.Abs(percentage);
                    break;
                case PuzzleType.Alternate:
                    if (UNInput.GetButtonDown(nextPress))
                    {
                        nextPress = nextPress != altButton[0] ? altButton[0] : altButton[1];
                        percentage += percentageIncrease;
                        if (percentage >= 100.0f)
                        {
                            isComplete = true;
                        }
                    }
                    percentage -= percentageDecrease * Time.deltaTime;
                    percentage = Mathf.Abs(percentage);
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
                            isComplete = true;
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

    }

    private void ResetCombination()
    {
        resetTimer = 0.0f;
        combCount = 0;
    }

    public void RandomizeAlternate()
    {
        int selectedAlt = UnityEngine.Random.Range(0, altButtonCombinations.Length);
        altButton = altButtonCombinations[selectedAlt];
    }

    public void RandomizeCombination(int combinationSize)
    {
        combButtons = new ButtonCode[combinationSize];
        for (int i = 0; i < combinationSize; i++)
        {
            int combRand = UnityEngine.Random.Range(0, availableButtons.Length);
            combButtons[i] = availableButtons[combRand];
        }
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