using UnityEngine;
using UniversalNetworkInput;

public class PlayerChecker : MonoBehaviour
{
	public Player[] player;

	private void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (UNInput.GetButtonDown(i, ButtonCode.A))
			{
				Debug.Log(i);
				break;
			}

			//if (!UNInput.GetButtonDown(i, ButtonCode.A))
			//{
			//	return;
			//}

			//for (int p = 0; p < player.Length; p++)
			//{
			//	if (player[p].IsActive())
			//	{
			//		return;
			//	}
			//	player[p].ActivePlayer(i);
			//	break;
			//}
		}
	}
}
