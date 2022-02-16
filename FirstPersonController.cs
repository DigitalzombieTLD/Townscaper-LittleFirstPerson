using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LittleFirstPerson
{
	public class FirstPersonController : MonoBehaviour
	{
		private Rigidbody rb;

		public bool invertCamera = false;
		public bool cameraCanMove = true;
		public float mouseSensitivity = 2f;
		public float maxLookAngle = 89f;

		public bool onWater;
		public bool isFlying;

		// Crosshair
		public bool lockCursor = true;
		public Sprite crosshairImage;
		public Color crosshairColor = Color.white;

		// Internal Variables
		private float yaw = 0.0f;
		private float pitch = 0.0f;
		private Image crosshairObject;

		public bool enableZoom = true;
		public bool holdToZoom = false;

		public float zoomFOV = 30f;
		public float zoomStepTime = 5f;

		// Internal Variables
		private bool isZoomed = false;


		public bool playerCanMove = true;
		public float walkSpeed = 1.2f;
		public float maxVelocityChange = 3f;

		// Internal Variables
		private bool isWalking = false;


		public bool enableSprint = true;
		public bool unlimitedSprint = false;


		public float sprintDuration = 60f;
		public float sprintCooldown = .1f;

		public float sprintFOVStepTime = 10f;

		// Jetpack

		public Vector3 velocity;

		// Internal Variables		
		private bool isSprinting = false;	
		public float jumpPower = 0.5f;

		// Internal Variables
		private bool isGrounded = false;
		

		public Transform joint;
		public Vector3 originalScale;

		public float bobSpeed = 5f;
		public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

		// Internal Variables
		private Vector3 jointOriginalPos;
		private float timer = 0;
		

		public FirstPersonController(IntPtr intPtr) : base(intPtr) { }

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();

			crosshairObject = GetComponentInChildren<Image>();
			//MyModUI.fpsCam = GetComponentInChildren<Camera>();
			MyModUI.fpsCam = LittleFirstPersonMain.fpsCamera;
			joint = this.transform.GetChild(0);

			// Set internal variables
			originalScale = transform.localScale;
			jointOriginalPos = joint.localPosition;

			MyModUI.crosshairObject = crosshairObject.gameObject;
			crosshairObject.sprite = crosshairImage;
			crosshairObject.color = crosshairColor;

			if (InputMain.pointerEnabled)
			{
				crosshairObject.gameObject.SetActive(true);
			}
			else
			{
				crosshairObject.gameObject.SetActive(false);
			}
		}

		private void Start()
		{

		}

	
		private void Update()
		{
			// Control camera movement
				yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

				if (!invertCamera)
				{
					pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
				}
				else
				{
					// Inverted Y
					pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
				}

				// Clamp pitch between lookAngle
				pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

				transform.localEulerAngles = new Vector3(0, yaw, 0);
				MyModUI.fpsCam.transform.localEulerAngles = new Vector3(pitch, 0, 0);
			

			
				// Changes isZoomed when key is pressed
				// Behavior for toogle zoom
				if (Input.GetKeyDown(InputMain.zoom))
				{					
						isZoomed = !isZoomed;
				}

		

				// Lerps camera.fieldOfView to allow for a smooth transistion
				if (isZoomed && !isSprinting)
				{
					MyModUI.fpsCam.fieldOfView = Mathf.Lerp(MyModUI.fpsCam.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
				}

				if(isSprinting)
				{					
					MyModUI.fpsCam.fieldOfView = Mathf.Lerp(MyModUI.fpsCam.fieldOfView, InputMain.sprintFOV, sprintFOVStepTime * Time.deltaTime);
				}

				if (!isZoomed && !isSprinting)
				{
					MyModUI.fpsCam.fieldOfView = Mathf.Lerp(MyModUI.fpsCam.fieldOfView, InputMain.cameraFOV, zoomStepTime * Time.deltaTime);
				}			
						
			CheckGround();

			if (InputMain.headbobEnabled)
			{
				HeadBob();
			}
		}

		void FixedUpdate()
		{
			if (playerCanMove)
			{
				// Calculate how fast we should be moving
				//Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Forward"));
				if (Input.GetKey(InputMain.forward))
				{
					LittleFirstPersonMain.currentVelocity.z = LittleFirstPersonMain.maxVelocity.z;
				}
				else if (Input.GetKey(InputMain.back))
				{
					LittleFirstPersonMain.currentVelocity.z = LittleFirstPersonMain.minVelocity.z;
				}
				else
				{
					LittleFirstPersonMain.currentVelocity.z = 0;
				}

				if (Input.GetKey(InputMain.left))
				{
					LittleFirstPersonMain.currentVelocity.x = LittleFirstPersonMain.minVelocity.x;
				}
				else if (Input.GetKey(InputMain.right))
				{
					LittleFirstPersonMain.currentVelocity.x = LittleFirstPersonMain.maxVelocity.x;
				}
				else
				{
					LittleFirstPersonMain.currentVelocity.x = 0;
				}

				//Vector3 targetVelocity = LittleFirstPersonMain.currentVelocity;

				// Checks if player is walking and isGrounded
				// Will allow head bob
				if (LittleFirstPersonMain.currentVelocity.x != 0 || LittleFirstPersonMain.currentVelocity.z != 0 && isGrounded)
				{
					isWalking = true;
				}
				else
				{
					isWalking = false;
				}


				if(isGrounded && isWalking && InputMain.footstepAudio)
				{
					if (!isSprinting)
					{
						if(!onWater)
						{
							AudioMain.PlayRepeat("WALK01_TILE");
						}
						else
						{
							AudioMain.PlayRepeat("WALK01_WATER");
						}
						
					}
					else if(isSprinting)
					{
						if (!onWater)
						{
							AudioMain.PlayRepeat("RUN01_TILE");
						}
						else
						{
							AudioMain.PlayRepeat("RUN01_WATER");
						}
					}
					else
					{
						AudioMain.StopPlay();
					}
				}
				else
				{
					if(isFlying && InputMain.jetpackAudio)
					{
						AudioMain.PlayRepeat("JETPACK");
					}
					else
					{
						AudioMain.StopPlay();
					}					
				}

				// All movement calculations shile sprint is active
				if (Input.GetKey(InputMain.sprint))
				{
					LittleFirstPersonMain.currentVelocity = transform.TransformDirection(LittleFirstPersonMain.currentVelocity) * InputMain.sprintSpeed;

					// Apply a force that attempts to reach our target velocity
					Vector3 velocity = rb.velocity;
					Vector3 velocityChange = (LittleFirstPersonMain.currentVelocity - velocity);
					velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
					velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
					velocityChange.y = 0;

					// Player is only moving when valocity change != 0
					// Makes sure fov change only happens during movement
					if (velocityChange.x != 0 || velocityChange.z != 0)
					{
						isSprinting = true;										
					}

					rb.AddForce(velocityChange, ForceMode.VelocityChange);
				}
				// All movement calculations while walking
				else
				{
					isSprinting = false;

					LittleFirstPersonMain.currentVelocity = transform.TransformDirection(LittleFirstPersonMain.currentVelocity) * walkSpeed;

					// Apply a force that attempts to reach our target velocity
					Vector3 velocity = rb.velocity;
					Vector3 velocityChange = (LittleFirstPersonMain.currentVelocity - velocity);
					velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
					velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
					velocityChange.y = 0;

					rb.AddForce(velocityChange, ForceMode.VelocityChange);
				}

				if (Input.GetKey(InputMain.jump))
				{					
					Jetpack();
				}
				else
				{
					if(isFlying)
					{
						AudioMain.StopPlay();
					}
				}				
			}
		}

		// Sets isGrounded based on a raycast sent straigth down from the player object
		private void CheckGround()
		{
			Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
			Vector3 direction = transform.TransformDirection(Vector3.down);
			float distance = .75f;

			if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
			{				
				isGrounded = true;
				isFlying = false;

				if (hit.collider.gameObject.name == "GroundBoxCollider")
				{
					onWater = true;
				}
				else
				{
					onWater = false;
				}
			}
			else
			{
				isGrounded = false;
				//isFlying = true;
			}
		}

		private void Jetpack()
		{
			rb.AddForce(0f, InputMain.jetpackPower, 0f, ForceMode.VelocityChange);
			isGrounded = false;
			isFlying = true;
		}
				
		private void HeadBob()
		{
			if (isWalking)
			{
				// Calculates HeadBob speed during sprint
				if (isSprinting)
				{
					timer += Time.deltaTime * (bobSpeed + InputMain.sprintSpeed);
				}				
				// Calculates HeadBob speed during walking
				else
				{
					timer += Time.deltaTime * bobSpeed;
				}
				// Applies HeadBob movement
				joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
			}
			else
			{
				// Resets when play stops moving
				timer = 0;
				joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
			}
		}
	}
}