using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UniversalNetworkInput;

public class MenuController : MonoBehaviour
{
	[BoxGroup("EventSystem")]
	public EventSystem eventSystem; 

	[BoxGroup("Camera")]
	public Transform cam;

	[BoxGroup("Menu")]
	public Transform menu;

	[BoxGroup("Credits")]
	public Transform credits;

	[BoxGroup("Choose Character")]
	public Transform choose;

	[BoxGroup("Choose Character")]
	public UnityEngine.UI.Image fadeBackground;

	[BoxGroup("MenuToCredits")]
	public float menuTime;

	[BoxGroup("MenuToCredits")]
	public AnimationCurve menuAnimation;

	[BoxGroup("CreditsToMenu")]
	public float creaditsTime;

	[BoxGroup("CreditsToMenu")]
	public AnimationCurve creditsAnimation;

	[BoxGroup("Menu Music")]
	public string menuMusic;

	[Button]
	public void MenoToCredits()
	{
		AudioManager.Instance.PlaySound("SubmitSound");
		StartCoroutine(Move(menu, credits, menuAnimation, menuTime));
		eventSystem.enabled = false;
	}
	
	[Button]
	public void CreditsToMenu()
	{
		AudioManager.Instance.PlaySound("CancelSound");
		StartCoroutine(Move(credits, menu, creditsAnimation, creaditsTime));
		eventSystem.enabled = true;
	}

	public void MenuToChoose()
	{
		AudioManager.Instance.PlaySound("SubmitSound");
		StartCoroutine(ChooseFade(menu, choose));
	}

	public void ChooseToMenu()
	{
		AudioManager.Instance.PlaySound("CancelSound");
		StartCoroutine(ChooseFade(choose, menu));
	}

	public void Start()
	{
		AudioManager.Instance.PlaySound(menuMusic);
		AudioManager.Instance.StopSound("GameplayMusic");
	}

	public void Update()
	{
		if(!eventSystem.enabled)
		{
			if(UNInput.GetButtonDown(ButtonCode.B))
			{
				CreditsToMenu();
				eventSystem.enabled = true;
			}
		}
	}

	private IEnumerator Move(Transform origin, Transform destiny, AnimationCurve animationCurve, float time = 1f)
	{
		float timer = 0;
		while (timer < time)
		{
			cam.transform.position = Vector3.Lerp(origin.transform.position, destiny.transform.position, timer);
			cam.transform.rotation = Quaternion.Lerp(origin.transform.rotation, destiny.transform.rotation, timer);
			timer += Time.deltaTime;
			yield return null;
		}

		cam.transform.position = destiny.transform.position;
		cam.transform.rotation = destiny.transform.rotation;
	}

	private IEnumerator ChooseFade(Transform origin, Transform destiny)
	{
		float timer = 0f;

		Color begin = new Color(1f,1f,1f,0f);
		Color end = Color.white;

		while(timer < 1f)
		{
			timer += Time.deltaTime;
			fadeBackground.color = Color.Lerp(begin, end, timer); 
			yield return null;
		}
				
		origin.parent.gameObject.SetActive(false);
		destiny.parent.gameObject.SetActive(true);

		cam.transform.position = destiny.transform.position;
		cam.transform.rotation = destiny.transform.rotation;

		timer = 0f;
		while (timer < 1f)
		{
			timer += Time.deltaTime;
			fadeBackground.color = Color.Lerp(end, begin, timer);
			yield return null;
		}

	}

	public static void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
	public static void Quit()
	{
		AudioManager.Instance.PlaySound("SubmitSound");
		Application.Quit();
	}

	public void AudioPlay(string name)
	{
		AudioManager.Instance.PlaySound(name);
	}

	public void AudioStop(string name)
	{
		AudioManager.Instance.StopSound(name);
	}

}
