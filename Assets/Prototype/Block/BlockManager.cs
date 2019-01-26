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

	private IEnumerator InstantiateBlock(float interval)
	{
		float timer = 0f;
		float time = interval;
		int blockCount = 0;

		while (timer < interval)
		{
			timer += Time.deltaTime;
			if (timer > interval - (interval * .1f))
			{
				CreateBlock();
				++blockCount;
				timer = 0f;
			}

			yield return null;
		}
	}

	private void CreateBlock()
	{
		blockBase.GetComponent<Block>().blockData = blockData[Random.Range(0,blockData.Length)];
		GameObject.Instantiate(blockBase, new Vector3(Random.Range(0f, 20f)-10f, 15f, Random.Range(0f, 20f)-15f), Quaternion.identity, null);
	}

}
