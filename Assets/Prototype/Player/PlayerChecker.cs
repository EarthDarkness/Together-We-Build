using UnityEngine;
using UniversalNetworkInput;

using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerChecker : MonoBehaviour
{
	public Player[] players;

	public bool blockCheck = false;

	public static List<int> playersActivated = new List<int>();

	private void Start()
	{
		foreach (Player player in players)
		{
			player.DesactivePlayer();
		}
	}

	private void Update()
	{
		if(UNInput.GetButtonDown(ButtonCode.Start))
		{
			if(playersActivated.Count > 0)
			{
				SceneManager.LoadScene(1);
			}
		}

		for (int id = 0; id < 4; id++)
		{
			if (playersActivated.Contains(id))
			{
				continue;
			}

			blockCheck = false;

			if (UNInput.GetButtonDown(id, ButtonCode.A))
			{
				for (int i = 0; i < 4; i++)
				{
					if (players[i].IsActive())
					{
						continue;
					}
					players[i].ActivePlayer(id, false);
					playersActivated.Add(id);
					blockCheck = true;
					break;
				}
			}
			if (blockCheck)
				break;
		}
	}
}
