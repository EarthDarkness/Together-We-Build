using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	[BoxGroup("BlockData")]
	public BlockData blockData;

	[HideInInspector]
	public Rigidbody rigidBody;

	private MeshRenderer meshRenderer;
	private Outline outline;

	private void Start()
	{
		outline = GetComponent<Outline>();
		meshRenderer = GetComponent<MeshRenderer>();
		rigidBody = GetComponent<Rigidbody>();
		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		meshRenderer.material.color =  blockData.color;
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

}
