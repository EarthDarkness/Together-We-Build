using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalNetworkInput;
using TMPro;

public class InputPuzzle : MonoBehaviour
{

    public TextMeshProUGUI text;

    public enum PuzzleType
    {
        Press,
        Alternate,
        Combination
    }

    private ButtonCode[] randButton = { ButtonCode.A, ButtonCode.B, ButtonCode.X, ButtonCode.Y };

    private ButtonCode[] combButtons = { ButtonCode.A, ButtonCode.LeftBumper, ButtonCode.RightBumper };

    private ButtonCode[] wrongButtons;

    ButtonCode lastPressed;

    public PuzzleType puzzleType;

    int buttonSelected;

    public float resetTime = 0.5f;

    float resetTimer = 0.0f;

    int combCount = 0;

    float percentage = 0;

    float percentageIncrease = 10.0f;

    float percentageDecrease = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        buttonSelected = UnityEngine.Random.Range(0, randButton.Length);
        text.text = "<sprite name=\"" + randButton[buttonSelected].ToString() + "\">";
        int enumSize = Enum.GetNames(typeof(ButtonCode)).Length;
        wrongButtons = new ButtonCode[enumSize - combButtons.Length];
        for (int i = 0; i < enumSize; i++)
        {
            for (int j = 0; j < combButtons.Length; j++)
            {
                bool wrong = false;
                if (i == (int)combButtons[j])
                {
                    continue;
                }
                wrongButtons[i] = (ButtonCode)i;
            }
        }
        foreach (ButtonCode button in wrongButtons)
        {
            Debug.Log(button);
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

                break;
            case PuzzleType.Combination:
                if (resetTimer >= resetTime)
                {
                    Debug.Log("Time");
                    ResetCombination();
                }

                if (UNInput.GetButtonDown(combButtons[combCount]))
                {
                    Debug.Log("Right");
                    combCount++;
                    if (combCount == combButtons.Length)
                    {
                        //Finished Combination.
                        text.text = "Finished";
                        combCount = 0;
                    }
                }

                else
                {
                    ResetCombination();
                    Debug.Log("Bad");
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

}