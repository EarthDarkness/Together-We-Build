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

	private void Start()
	{
		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		meshRenderer.material.color = blockData.color;
		outline.OutlineColor = Color.white;
	}

	public void EnableBlock()
	{
		outline.enabled = true;
	}

	public void DisableBlock()
	{
		outline.enabled = false;
	}

	public void DestroyBlock()
	{
		Destroy(gameObject,10f);
	}
}
