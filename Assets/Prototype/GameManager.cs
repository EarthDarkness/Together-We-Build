using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
	[Required]
	public BlockManager blockManager;
	public List<Block> puzzle = new List<Block>();

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (puzzle.Count == 0)
			{
				puzzle.Add(blockManager.CreateBlock(transform.position + Vector3.up * 8f,
					blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
				puzzle.Add(blockManager.CreateBlock(transform.position + Vector3.up * 8f + Vector3.left * 3f,
					blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
				puzzle.Add(blockManager.CreateBlock(transform.position + Vector3.up * 8f + Vector3.right * 3f,
					blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Player p = other.GetComponentInParent<Player>();
			if (!p.catchBlock)
			{
				return;
			}
			Block currentBlock;
			for (int i = 0; i < puzzle.Count; i++)
			{
				if (puzzle[i].blockData.ID == p.catchBlock.blockData.ID)
				{
					Destroy(puzzle[i].gameObject, .1f);
					Destroy(p.catchBlock.gameObject, .1f);
					puzzle.RemoveAt(i);
					p.catchBlock = null;
					break;
				}
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//Debug.Log(other.gameObject.name + " Saiu na casa!222");
			//Player p = other.GetComponentInParent<Player>();
		}
	}
}
