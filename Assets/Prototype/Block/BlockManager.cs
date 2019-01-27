using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
	public GameObject blockBase;

	public BlockData[] blockData;

	private void Start()
	{
		StartCoroutine(InstantiateBlock(.5f));
	}

	private IEnumerator InstantiateBlock(float interval, PlayerData playerData = null)
	{
		float timer = 0f;
		float time = interval;
		int blockCount = 0;

		while (timer < interval)
		{
			timer += Time.deltaTime;
			if (timer > interval - (interval * .1f))
			{
				CreateBlock(Vector3.zero);
				++blockCount;
				timer = 0f;
			}

			yield return null;
		}
	}

	public Block CreateBlock(Vector3 position, BlockData block = null)
	{
		Block blockRef;
		if (block)
		{
			blockBase.GetComponent<Block>().blockData = block;
			GameObject GO = GameObject.Instantiate(blockBase, position, Quaternion.identity, null);
			blockRef = GO.GetComponent<Block>();
			//blockRef.outline.OutlineColor = blockRef.meshRenderer.material.color;
			//blockRef.meshRenderer.material.color = Color.white;
			blockRef.rigidBody.isKinematic = true;
			blockRef.name = "Block - GameManager";
		}
		else
		{
			blockBase.GetComponent<Block>().blockData = blockData[Random.Range(0, blockData.Length)];
			GameObject GO = GameObject.Instantiate(blockBase, new Vector3(Random.Range(0f, 12f) - 6f, 15f, Random.Range(0f, 12f) - 10f), Quaternion.identity, null);
			blockRef = GO.GetComponent<Block>();
			blockRef.DestroyBlock(10.0f);
		}
		return blockRef;
	}

}
