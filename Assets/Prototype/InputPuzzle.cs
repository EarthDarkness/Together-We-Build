using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniversalNetworkInput;

public class InputPuzzle : MonoBehaviour
{
    public enum PuzzleType
    {
        Press = 0,
        Alternate = 1,
        Combination = 2
    }

    private int controllerID;

    public PuzzleType puzzleType;

    [ShowIf("ShowPress"), ReorderableList]
    public ButtonCode[] randButton = { ButtonCode.A, ButtonCode.B, ButtonCode.X, ButtonCode.Y };

    [ShowIf("ShowCombination"), ReorderableList, ReadOnly]
    public ButtonCode[] combButtons = { ButtonCode.A, ButtonCode.LeftBumper, ButtonCode.RightBumper };

    [ShowIf("ShowAlternate"), ReorderableList, OnValueChanged("AlternateMaxTwo"), ReadOnly]
    public ButtonCode[] altButton = { ButtonCode.A, ButtonCode.B };

    [Required]
    public FloatVariable progressValue;

    [Required]
    public ButtonCodeVariable keycodeVariable;

    private ButtonCode[] availableButtons = {
        ButtonCode.A,
        ButtonCode.B,
        ButtonCode.X,
        ButtonCode.Y,
        ButtonCode.LeftBumper,
        ButtonCode.RightBumper,
        ButtonCode.DPadDown,
        ButtonCode.DPadUp,
        ButtonCode.DPadLeft,
        ButtonCode.DPadRight
    };

    private ButtonCode[][] altButtonCombinations = {
       new ButtonCode[] { ButtonCode.A, ButtonCode.B },
       new ButtonCode[] { ButtonCode.LeftBumper, ButtonCode.RightBumper },
       new ButtonCode[] { ButtonCode.X, ButtonCode.Y },
       new ButtonCode[] { ButtonCode.X, ButtonCode.B },
       new ButtonCode[] { ButtonCode.DPadLeft, ButtonCode.DPadRight },
       new ButtonCode[] { ButtonCode.DPadUp, ButtonCode.DPadDown }
    };

    private ButtonCode[] allButtons =
    {
        ButtonCode.A,
        ButtonCode.B,
        ButtonCode.X,
        ButtonCode.Y,
        ButtonCode.DPadDown,
        ButtonCode.DPadLeft,
        ButtonCode.DPadRight,
        ButtonCode.DPadUp,
        ButtonCode.LeftBumper,
        ButtonCode.LeftStick,
        ButtonCode.RightBumper,
        ButtonCode.RightStick
    };

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

    [ReadOnly]
    public bool isComplete = false;

    public void CreatePuzzle(PuzzleType type, int controllerID, int combinationSize = 3, bool randCombination = true, bool randAlternative = true )
    {
        isComplete = false;

        puzzleType = type;
        ResetCombination();

        percentage = 0;

        this.controllerID = controllerID;

        buttonSelected = UnityEngine.Random.Range(0, randButton.Length);

        if (randCombination)
        {
            RandomizeCombination(combinationSize);
        }

        if (randAlternative)
        {
            RandomizeAlternate();
        }

        nextPress = altButton[0];

        switch (puzzleType)
        {
            case PuzzleType.Press:
                keycodeVariable.keyCodes = new ButtonCode[] { randButton[buttonSelected] };
                break;
            case PuzzleType.Alternate:
                keycodeVariable.keyCodes = altButton;
                break;
            case PuzzleType.Combination:
                keycodeVariable.keyCodes = combButtons;
                break;
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
                    if (UNInput.GetButtonDown(controllerID, randButton[buttonSelected]))
                    {
                        percentage += percentageIncrease;
                        if (percentage >= 100.0f)
                        {
                            Debug.Log("Completed");
                            isComplete = true;
                        }
                    }

                    percentage -= percentageDecrease * Time.deltaTime;
                    percentage = Mathf.Abs(percentage);
                    progressValue.value = percentage / 100f;
                    break;
                case PuzzleType.Alternate:
                    if (UNInput.GetButtonDown(controllerID, nextPress))
                    {
                        nextPress = nextPress != altButton[0] ? altButton[0] : altButton[1];
                        percentage += percentageIncrease;
                        if (percentage >= 100.0f)
                        {
                            Debug.Log("Completed");
                            isComplete = true;
                        }
                    }
                    percentage -= percentageDecrease * Time.deltaTime;
                    percentage = Mathf.Abs(percentage);
                    progressValue.value = percentage / 100f;
                    break;
                case PuzzleType.Combination:
                    if (resetTimer >= resetTime)
                    {
                        ResetCombination();
                    }
                    foreach (ButtonCode button in allButtons)
                    {
                        if (!UNInput.GetButtonDown(controllerID, button))
                        {
                            continue;
                        }

                        Debug.Log(button);
                        if(button != combButtons[combCount])
                        {
                            ResetCombination();
                            resetTimer = 0.0f;
                        }
                        else
                        {
                            combCount++;
                        }
						transform.GetChild(2).GetChild(1).GetComponentInChildren<ButtonCodeWriter>().SetText(combCount);
                        if(combCount >= combButtons.Length)
                        {
                            Debug.Log("Completed");
                            isComplete = true;
                            break;
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