using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
	public GameObject blockBase;

	public BlockData[] blockData;

	[Required]
	public Transform spawnArea;

	private void Start()
	{
		StartCoroutine(InstantiateBlock(2.0f));
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
			Vector3 pos = new Vector3(
				Random.Range(
					spawnArea.position.x - spawnArea.localScale.x,
					spawnArea.position.x + spawnArea.localScale.x
				),
				Random.Range(
					spawnArea.position.y - spawnArea.localScale.y,
					spawnArea.position.y + spawnArea.localScale.y
				),
				Random.Range(
					spawnArea.position.z - spawnArea.localScale.z,
					spawnArea.position.z + spawnArea.localScale.z
				)
			);

			GameObject GO = GameObject.Instantiate(blockBase, pos, Quaternion.identity, null);
			blockRef = GO.GetComponent<Block>();
			blockRef.DestroyBlock(20.0f);
		}
		return blockRef;
	}

}
