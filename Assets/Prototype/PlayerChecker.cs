using UnityEngine;
using UniversalNetworkInput;

using System.Collections.Generic;

public class PlayerChecker : MonoBehaviour
{
	public Player[] player;

	public bool test = false;

	public static List<int> playersActivated = new List<int>();

	private void Update()
	{

		for (int id = 0; id < 4; id++)
		{
			if (playersActivated.Contains(id))
			{
				continue;
			}

			test = false;

			if (UNInput.GetButtonDown(id, ButtonCode.A))
			{
				for (int i = 0; i < 4; i++)
				{
					if (player[i].IsActive())
					{
						continue;
					}
					player[i].ActivePlayer(id);
					playersActivated.Add(id);
					test = true;
					break;
				}
			}
			if (test)
				break;
		}
	}
}
