using System;
using MelonLoader;
using Harmony;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Placemaker;

namespace LittleFirstPerson
{
	public static class InputMain
	{
		public static Vector3 mousePos = new Vector3(0,0,0);
		public static Vector3 positionOffset = new Vector3(0, 1, 0);

		public static KeyCode enterFP;
		public static KeyCode enterBuild;

		public static KeyCode forward;
		public static KeyCode back;
		public static KeyCode left;
		public static KeyCode right;
		public static KeyCode jump;
		public static KeyCode zoom;
		public static KeyCode sprint;

		public static float jetpackPower;
		public static float sprintSpeed;
		public static float walkSpeed;
		public static float cameraFOV;
		public static float sprintFOV;
		public static bool headbobEnabled;
		public static bool pointerEnabled;

		public static bool jetpackAudio;
		public static bool footstepAudio;


		public static void GetInput()
		{
			if (MyModUI.isInitialized)
			{
				if (Input.GetKeyDown(enterFP))
				{
					if (!LittleFirstPersonMain.fpsPlayer)
					{
						LittleFirstPersonMain.fpsPlayer = UnityEngine.Object.Instantiate(LittleFirstPersonMain.fpsPlayerPrefab);						
						LittleFirstPersonMain.originalCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
						LittleFirstPersonMain.fpsCamera = LittleFirstPersonMain.fpsPlayer.GetComponentInChildren<Camera>();
						LittleFirstPersonMain.fpsPlayer.AddComponent<FirstPersonController>();
						Vector3 tempCamPosition = LittleFirstPersonMain.fpsCamera.transform.position;
						LittleFirstPersonMain.fpsCamera.CopyFrom(LittleFirstPersonMain.originalCamera);
						LittleFirstPersonMain.fpsCamera.transform.position = tempCamPosition;
						LittleFirstPersonMain.fpsCamera.nearClipPlane = 0.03f;
						AudioMain.playerSource = LittleFirstPersonMain.fpsPlayer.GetComponent<AudioSource>();

						UnityEngine.Object.DontDestroyOnLoad(LittleFirstPersonMain.fpsPlayer);							
					}

					if (LittleFirstPersonMain.fpsPlayer)
					{
						if (!LittleFirstPersonMain.fpsActive)
						{
							LittleFirstPersonMain.fpsPlayer.transform.position = InputMain.mousePos + positionOffset;
							LittleFirstPersonMain.originalCamera.enabled = false;
							LittleFirstPersonMain.fpsCamera.gameObject.SetActive(true);
							LittleFirstPersonMain.fpsCamera.tag = "MainCamera";
							LittleFirstPersonMain.originalCamera.tag = "NotMainCamera";
							LittleFirstPersonMain.fpsActive = true;

							Cursor.lockState = CursorLockMode.Locked;
							Cursor.visible = false;
						}
						else
						{
							LittleFirstPersonMain.fpsCamera.gameObject.SetActive(false);
							LittleFirstPersonMain.originalCamera.enabled = true;
							LittleFirstPersonMain.fpsCamera.tag = "NotMainCamera";
							LittleFirstPersonMain.originalCamera.tag = "MainCamera";
							LittleFirstPersonMain.fpsActive = false;

							Cursor.lockState = CursorLockMode.None;
							Cursor.visible = true;
							LittleFirstPersonMain.buildMode = false;
						}
					}
				}

				if (Input.GetKeyDown(enterBuild))
				{
					if (LittleFirstPersonMain.fpsActive)
					{
						if (LittleFirstPersonMain.buildMode)
						{
							Cursor.lockState = CursorLockMode.Locked;
							Cursor.visible = false;
							LittleFirstPersonMain.buildMode = false;
						}
						else
						{
							Cursor.lockState = CursorLockMode.Confined;
							Cursor.visible = true;

							LittleFirstPersonMain.buildMode = true;
						}
					}
				}
			}
		}
	}
}
