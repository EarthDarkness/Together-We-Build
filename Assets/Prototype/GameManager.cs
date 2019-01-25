using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
	[Required]
	public BlockManager blockManager;
	public List<Block> puzzle = new List<Block>();

    public int puzzleNumber = 3;

    private int houseFloors = 0;

    private void Start()
    {
        for (int i = 0; i < puzzleNumber; i++)
        {
            puzzle.Add(blockManager.CreateBlock(transform.position + Vector3.up * 8f + Vector3.left * 3f + (Vector3.right * 3.0f) * i,
                    blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
        }
    }

    private void Update()
	{

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

            if (puzzle.Count <= 0)
            {
                
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
