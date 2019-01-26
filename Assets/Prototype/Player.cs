using NaughtyAttributes;
using UnityEngine;
using UniversalNetworkInput;

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

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	private void Start()
	{
		controller = GetComponent<CharacterController>();		
	}

	private void Update()
	{
		if (controller.isGrounded)
		{			
			moveDirection = new Vector3(UNInput.GetAxis(AxisCode.LeftStickHorizontal), 0.0f, UNInput.GetAxis(AxisCode.LeftStickVertical);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection = moveDirection * speed;

			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}

		// Apply gravity
		moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

		// Move the controller
		controller.Move(moveDirection * Time.deltaTime);
	}
}
