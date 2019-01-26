using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "BlockData", menuName = "Create BlockData")]
public class BlockData : ScriptableObject
{
	[ShowAssetPreview]
	public Texture2D icon;
	public Color color = Color.white;
	public int ID = 0;	
}
