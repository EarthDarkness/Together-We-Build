using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonGuide : MonoBehaviour {

	public void Clear(){
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(false);
	}
	public void Grab(){
		transform.GetChild(0).gameObject.SetActive(true);
		transform.GetChild(1).gameObject.SetActive(false);
	}
	public void Throw(){
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);
	}
}
