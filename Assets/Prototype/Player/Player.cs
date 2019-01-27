using NaughtyAttributes;
using UnityEngine;
using UniversalNetworkInput;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	[BoxGroup("PlayerData"), Required]
	public PlayerData playerData;

	[BoxGroup("Model")]
	public PlayerModels models;

	[BoxGroup("Skin")]
	public SkinColors skinColors;

	[BoxGroup("PROTOTYPE")]
	public GameObject body, spawn;

	[BoxGroup("Block System")]
	public Block interactBlock, catchBlock;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private Vector2 movement = Vector2.zero;

	private Vector3 startPos = Vector3.zero;
	private Quaternion startRot = Quaternion.identity;
	private bool canSetModelIndex = true;
	private bool canSetSkinIndex = true;
	private int skinToneID = 0;
	private void Start()
	{
		controller = GetComponent<CharacterController>();
		startPos = transform.position;
		startRot = transform.rotation;
		//ActivePlayer(playerData.ID);

		if (IsActive())
		{
			GameManager.Instance.players.Add(this);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		Movement();
		DropBlock();
		CatchBlock();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (catchBlock)
		{
			return;
		}
		if (other.CompareTag("Block"))
		{
			if (interactBlock)
			{
				interactBlock.DisableBlock();
			}
			interactBlock = other.GetComponent<Block>();
			interactBlock.EnableBlock();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Block"))
		{
			interactBlock = other.GetComponent<Block>();
			interactBlock.DisableBlock();
			interactBlock = null;
		}
	}

	public void IncrementModel()
	{
		SetModelIndex(true);
	}

	public void DecrementModel()
	{
		SetModelIndex(false);
	}

	public void IncrementSkin()
	{
		SetSkinIndex(true);
	}

	public void DecrementSkin()
	{
		SetSkinIndex(false);
	}

	private void SetSkinIndex(bool increment)
	{
		if (canSetSkinIndex)
		{
			if (increment)
			{
				++skinToneID;
			}
			else
			{
				--skinToneID;
			}
			skinToneID = (skinToneID + skinColors.skins.Length) % skinColors.skins.Length;

			playerData.skinColor = skinColors.skins[skinToneID];

			canSetSkinIndex = false;
			Invoke("EnableSkinIndex", .5f);
		}
	}

	private void SetModelIndex(bool increment)
	{
		if (canSetModelIndex)
		{
			if (increment)
			{
				++playerData.modelID;
			}
			else
			{
				--playerData.modelID;
			}
			playerData.modelID = (playerData.modelID + 6) % 6;
			canSetModelIndex = false;
			Invoke("EnableModelIndex", .5f);
		}
	}

	private void EnableModelIndex()
	{
		canSetModelIndex = true;
	}

	private void EnableSkinIndex()
	{
		canSetSkinIndex = true;
	}

	public void ActivePlayer(int id, bool enablePlayerScript = true)
	{
		playerData.ID = id;
		playerData.modelID = 0;
		//body.GetComponent<MeshRenderer>().material.color = playerData.playerColor;
		//body.SetActive(true);
		spawn.SetActive(false);
		enabled = enablePlayerScript;
	}

	public void DesactivePlayer()
	{
		PlayerChecker.playersActivated.Remove(playerData.ID);
		playerData.ID = -1;
		playerData.modelID = -1;
		skinToneID = Random.Range(0, skinColors.skins.Length);
		playerData.skinColor = skinColors.skins[skinToneID];
		spawn.GetComponent<MeshRenderer>().material.color = playerData.playerColor;
		//transform.position = startPos;
		//transform.rotation = startRot;

		spawn.SetActive(true);
		body.SetActive(false);

		enabled = false;
	}

	public void Movement()
	{
		if (controller.isGrounded)
		{
			movement.x = UNInput.GetAxis(playerData.ID, AxisCode.LeftStickHorizontal);
			movement.y = UNInput.GetAxis(playerData.ID, AxisCode.LeftStickVertical);
			moveDirection = new Vector3(
				Mathf.Abs(movement.x) > .4f ? movement.x : 0f,
				0.0f,
				Mathf.Abs(movement.y) > .4f ? movement.y : 0f);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection = moveDirection * playerData.speed;
		}

		// Apply gravity
		moveDirection.y = moveDirection.y - (playerData.gravity * Time.deltaTime);

		// Move the controller
		controller.Move(moveDirection * Time.deltaTime);

		//transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

		if (catchBlock)
		{
			catchBlock.transform.position = new Vector3(transform.position.x, 4f, transform.position.z);
		}
	}

	private void CatchBlock()
	{
		if (catchBlock)
		{
			return;
		}
		if (UNInput.GetButtonDown(playerData.ID, ButtonCode.A))
		{
			if (interactBlock)
			{
				interactBlock.rigidBody.isKinematic = true;
				catchBlock = interactBlock;
				interactBlock = null;
			}
		}
	}

	private void DropBlock()
	{
		if (UNInput.GetButtonDown(playerData.ID, ButtonCode.B))
		{
			if (catchBlock)
			{
				catchBlock.rigidBody.isKinematic = false;
				catchBlock.rigidBody.AddForce(Vector3.up * 500f);
				catchBlock = null;
			}
		}
	}

	public bool IsActive()
	{
		return playerData.ID != -1 ? true : false;
	}
}
