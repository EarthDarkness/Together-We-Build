using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCtrl : MonoBehaviour
{

	public PlayerData data;

	private GameObject child;
	private GameObject childBase;
	private GameObject childMale;
	private GameObject childFemale;
	private GameObject childNeutral;
	private Transform childHand;

	private GameObject adult;
	private GameObject adultBase;
	private GameObject adultMale;
	private GameObject adultFemale;
	private GameObject adultNeutral;
	private Transform adultHand;

	Animator[] graph;

	float grabDelay;
	float throwDelay;

	private void SetVisualModel(int id)
	{
		child.SetActive(false);
		adult.SetActive(false);

		GameObject[] gmList = new GameObject[6]{
			adultMale,
			adultFemale,
			adultNeutral,
			childMale,
			childFemale,
			childNeutral
		};

		foreach(GameObject gm in gmList){
			gm.SetActive(false);
		}

		if (id < 0)
			return;


		if(id < 3){
			adult.SetActive(true);
			gmList[id].SetActive(true);

		}else if(id < 6){
			child.SetActive(true);
			gmList[id].SetActive(true);

		}

	}

	private void SetColorCode(Color cloth, Color skin)
	{
		int id = data.modelID;
		if(id < 0 || id >= 6)
			return;
		
		Material[] mats;
		if(id < 3){
			mats = adultBase.GetComponent<SkinnedMeshRenderer>().materials;
			mats[0].SetColor("_Color", cloth);
			mats[1].SetColor("_Color", skin);
		}else if(id < 6){
			mats = childBase.GetComponent<SkinnedMeshRenderer>().materials;
			mats[0].SetColor("_Color", cloth);
			mats[1].SetColor("_Color", skin);
		}

	}

	public Transform HandTransform(){
		if(data.modelID < 0)
			return null;

		if(data.modelID <  3)
			return adultHand;
		if(data.modelID <  6)
			return childHand;

		return null;
	}

	public float Grab(){
		foreach(Animator anim in graph){
			anim.SetTrigger("Grab");
		}
		return grabDelay;
	}
	public float Throw(){
		foreach(Animator anim in graph){
			anim.SetTrigger("Throw");
		}
		return throwDelay;
	}
	public void Build(bool state){
		foreach(Animator anim in graph){
			anim.SetBool("Throw", state);
		}
	}

	private void Start()
	{
		adult = transform.GetChild(0).GetChild(0).gameObject;
		adultBase = adult.transform.GetChild(1).gameObject;
		adultMale = adult.transform.GetChild(3).gameObject;
		adultFemale = adult.transform.GetChild(4).gameObject;
		adultNeutral = adult.transform.GetChild(0).gameObject;
		adultHand = adult.transform.GetChild(2).GetChild(0);

		child = transform.GetChild(1).GetChild(0).gameObject;
		childBase = child.transform.GetChild(0).gameObject;
		childMale = child.transform.GetChild(2).gameObject;
		childFemale = child.transform.GetChild(4).gameObject;
		childNeutral = child.transform.GetChild(3).gameObject;
		childHand = child.transform.GetChild(1).GetChild(0);
	
		graph = GetComponentsInChildren<Animator>();

		
		//AnimatorClipInfo[] clips = graph[0].GetCurrentAnimatorClipInfo(0);
		AnimationClip[] clips = graph[0].runtimeAnimatorController.animationClips;
		
		foreach(AnimationClip clip in clips){
			if(clip.name == "Grab"){
				grabDelay = clip.length;
			}else if(clip.name == "Throw"){
				throwDelay = clip.length;
			}
		}
	}
	
	void Update()
	{
		if(data == null)
			return;

		SetVisualModel(data.modelID);
		SetColorCode(data.playerColor, data.skinColor);
	}
}
