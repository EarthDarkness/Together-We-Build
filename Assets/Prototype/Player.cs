using NaughtyAttributes;
using UnityEngine;
using UniversalNetworkInput;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	[BoxGroup("Atributes")]
	public int ID = 0;

	[BoxGroup("Atributes")]
	public float speed = 6.0f;

	[BoxGroup("Atributes")]
	public float jumpSpeed = 8.0f;

	[BoxGroup("Atributes")]
	public float gravity = 20.0f;

	[BoxGroup("References")]
	public GameObject body, spawn;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private Vector2 movement = Vector2.zero;

	private Vector3 startPos = Vector3.zero;
	private Quaternion startRot = Quaternion.identity;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
		startPos = transform.position;
		startRot = transform.rotation;
		//DesactivePlayer();
	}

	private void Update()
	{
		if (controller.isGrounded)
		{
			movement.x = UNInput.GetAxis(ID, AxisCode.LeftStickHorizontal);
			movement.y = UNInput.GetAxis(ID, AxisCode.LeftStickVertical);
			moveDirection = new Vector3(
				Mathf.Abs(movement.x) > .4f ? movement.x : 0f,
				0.0f,
				Mathf.Abs(movement.y) > .4f ? movement.y : 0f);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection = moveDirection * speed;
		}

		// Apply gravity
		moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

		// Move the controller
		controller.Move(moveDirection * Time.deltaTime);

		//transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
	}

	public void ActivePlayer(int id)
	{
		ID = id;

		body.SetActive(true);
		spawn.SetActive(false);

		enabled = true;
	}

	public void DesactivePlayer()
	{
		ID = -1;

		transform.position = startPos;
		transform.rotation = startRot;


		spawn.SetActive(true);
		body.SetActive(false);

		enabled = false;
	}

	public bool IsActive()
	{
		return ID != -1 ? true : false;
	}
}
