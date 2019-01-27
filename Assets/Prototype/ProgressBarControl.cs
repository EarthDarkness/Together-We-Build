using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarControl : MonoBehaviour {

    [Required]
    public FloatVariable progressValue;
    [Required]
    public Image progressImage;

	// Update is called once per frame
	void Update () {
        progressImage.fillAmount = progressValue.value;
	}
}
