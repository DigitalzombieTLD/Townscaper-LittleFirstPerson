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

		public static void GetInput()
		{		
			if(Input.GetKeyDown(KeyCode.F2))
			{
				if (!LittleFirstPersonMain.fpsPlayer)
				{
					LittleFirstPersonMain.fpsPlayer = UnityEngine.Object.Instantiate(LittleFirstPersonMain.fpsPlayerPrefab);
					LittleFirstPersonMain.fpsPlayer.AddComponent<FirstPersonController>();


					//LittleFirstPersonMain.originalCamera = Camera.main;
					LittleFirstPersonMain.originalCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();


					LittleFirstPersonMain.fpsCamera = LittleFirstPersonMain.fpsPlayer.GetComponentInChildren<Camera>();
					
					Vector3 tempCamPosition = LittleFirstPersonMain.fpsCamera.transform.position;

					LittleFirstPersonMain.fpsCamera.CopyFrom(LittleFirstPersonMain.originalCamera);

					LittleFirstPersonMain.fpsCamera.transform.position = tempCamPosition;
					LittleFirstPersonMain.fpsCamera.nearClipPlane = 0.03f;

					UnityEngine.Object.DontDestroyOnLoad(LittleFirstPersonMain.fpsPlayer);
					//SceneManager.MoveGameObjectToScene(LittleFirstPersonMain.fpsPlayer, SceneManager.GetSceneByName("Placemaker"));								
				}
				
				if(LittleFirstPersonMain.fpsPlayer)
				{
					if(!LittleFirstPersonMain.fpsActive)
					{
						LittleFirstPersonMain.fpsPlayer.transform.position = InputMain.mousePos + positionOffset;
						
						//LittleFirstPersonMain.originalCamera.gameObject.SetActive(false);

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
						//LittleFirstPersonMain.originalCamera.gameObject.SetActive(true);
						LittleFirstPersonMain.originalCamera.enabled = true;
						LittleFirstPersonMain.fpsCamera.tag = "NotMainCamera";
						LittleFirstPersonMain.originalCamera.tag = "MainCamera";

						LittleFirstPersonMain.fpsActive = false;
						//MelonCoroutines.Stop(LittleFirstPersonMain.DisableCursor());
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
						LittleFirstPersonMain.buildMode = false;
					}
				}				
			}

			if (Input.GetKeyDown(KeyCode.F3))
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
