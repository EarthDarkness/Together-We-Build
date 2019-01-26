using NaughtyAttributes;

using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create PlayerData")]
public class PlayerData : ScriptableObject
{
	[BoxGroup("Atributes")]
	public int ID = -1;

	[BoxGroup("Atributes")]
	public float speed = 6.0f;

	[BoxGroup("Atributes")]
	public float jumpSpeed = 8.0f;

	[BoxGroup("Atributes")]
	public float gravity = 20.0f;
}
