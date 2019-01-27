using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class ButtonCodeWriter : MonoBehaviour
{

    [Required]
    public ButtonCodeVariable codeVariable;
    [Required]
    public TextMeshProUGUI textMesh;


    public void SetText(int filled)
    {
        textMesh.text = string.Empty;
        for(int i=filled;i<codeVariable.keyCodes.Length;++i)
        {
            textMesh.text += "<sprite name=\"" + codeVariable.keyCodes[i].ToString() + "\">";
        }
    }

}
