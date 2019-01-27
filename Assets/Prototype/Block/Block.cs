using NaughtyAttributes;

using UnityEngine;

using System.Collections.Generic;

public class Block : MonoBehaviour
{
	[BoxGroup("BlockData")]
	public BlockData blockData;

	[BoxGroup("References")]
	public Rigidbody rigidBody;

	[BoxGroup("References")]
	public MeshRenderer meshRenderer;

	[BoxGroup("References")]
	public Outline outline;

	[HideInInspector]
	public Player player;

	public float timer = float.MaxValue;

	public bool stoppd = false;

	private void Start()
	{
		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		meshRenderer.material.color = blockData.color;
		outline.OutlineColor = Color.white;
	}

	private void Update()
	{
		if(!stoppd)
			timer -= Time.deltaTime;
		if(timer < 0.0f)
			Destroy(transform.gameObject);
	}

	public void EnableBlock()
	{
		outline.enabled = true;
	}

	public void DisableBlock()
	{
		outline.enabled = false;
	}

	public void DestroyBlock(float lifetime)
	{
		timer = lifetime;
	}
}
