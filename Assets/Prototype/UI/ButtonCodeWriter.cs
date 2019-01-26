using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ButtonCodeWriter : MonoBehaviour
{

    [Required]
    public ButtonCodeVariable codeVariable;
    [Required]
    public TextMeshProUGUI textMesh;


    public void SetText()
    {
        textMesh.text = string.Empty;
        foreach (UniversalNetworkInput.ButtonCode code in codeVariable.keyCodes)
        {
            textMesh.text += "<sprite name=\"" + code.ToString() + "\">";
        }
    }

}
