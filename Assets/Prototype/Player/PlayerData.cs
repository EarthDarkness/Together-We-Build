using NaughtyAttributes;

using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create PlayerData")]
public class PlayerData : ScriptableObject
{
	[BoxGroup("Controller")]
	public int ID = -1;

	[BoxGroup("Model")]
	public int modelID = -1;
	
	[BoxGroup("Model")]
	public Color playerColor = Color.black;
	
	[BoxGroup("Model")]
	public Color skinColor = Color.black;

	[BoxGroup("Movement")]
	public float speed = 6.0f;

	[BoxGroup("Movement")]
	public float jumpSpeed = 8.0f;

	[BoxGroup("Movement")]
	public float gravity = 20.0f;

}
