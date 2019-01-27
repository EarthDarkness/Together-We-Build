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

	public float maxLife = 10.0f;

	public float timer = 10.0f;

	public bool stoppd = false;

	private void Start()
	{
		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		meshRenderer.material.color = blockData.color;
		outline.OutlineColor = Color.white;
	}

	private void Update()
	{
		Color col = blockData.color;
		col.a = timer/maxLife;
		meshRenderer.material.color = col;

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
		maxLife = lifetime;
		timer = lifetime;
	}
}
