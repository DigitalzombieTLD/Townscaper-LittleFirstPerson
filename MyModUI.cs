using MelonLoader;
using UnityEngine;
using ModUI;
using System;
using UnityEngine.UI;

namespace LittleFirstPerson
{
	public static class MyModUI
	{
		public static MelonMod myMod;
		public static ModSettings myModSettings;

		public static bool isInitialized;
		public static GameObject crosshairObject;
		public static Camera fpsCam;

		public static void Initialize(MelonMod thisMod)
		{
			myModSettings = UIManager.Register(thisMod, new Color32(10, 180, 70, 255));

			myModSettings.AddKeybind("Enter FP", "General", KeyCode.F2, new Color32(60, 100, 20, 160));
			myModSettings.AddKeybind("Build mode", "General", KeyCode.F3, new Color32(60, 100, 20, 160));

			myModSettings.AddKeybind("Forward", "Movement", KeyCode.W, new Color32(10, 190, 124, 255));
			myModSettings.AddKeybind("Back", "Movement", KeyCode.S, new Color32(10, 190, 124, 255));
			myModSettings.AddKeybind("Left", "Movement", KeyCode.A, new Color32(10, 190, 124, 255));
			myModSettings.AddKeybind("Right", "Movement", KeyCode.D, new Color32(10, 190, 124, 255));

			myModSettings.AddToggle("Footstep Audio", "Audio", new Color32(10, 190, 124, 255), true, new Action<bool>(delegate (bool value) { UpdateValues(); }));

			myModSettings.AddKeybind("Zoom", "Movement", KeyCode.Q, new Color32(60, 100, 124, 255));

			myModSettings.AddKeybind("Sprint", "Movement", KeyCode.LeftShift, new Color32(230, 70, 20, 255));
			myModSettings.AddSlider("Sprint Speed", "Movement", new Color32(230, 70, 20, 255), 2f, 6f, false, 2.0f, new Action<float>(delegate (float value) { UpdateValues(); }));

			myModSettings.AddSlider("Walk Speed", "Movement", new Color32(230, 70, 20, 255), 0.2f, 6f, false, 1.0f, new Action<float>(delegate (float value) { UpdateValues(); }));

			myModSettings.AddKeybind("Jetpack", "Movement", KeyCode.Space, new Color32(200, 70, 164, 255));
			myModSettings.AddSlider("Jetpack Power", "Movement", new Color32(200, 70, 164, 255), 0.1f, 0.8f, false, 0.35f, new Action<float>(delegate (float value) { UpdateValues(); }));
			myModSettings.AddToggle("Jetpack Audio", "Audio", new Color32(200, 70, 164, 255), true, new Action<bool>(delegate (bool value) { UpdateValues(); }));				

			myModSettings.AddSlider("Normal FOV", "Camera", new Color32(100, 100, 164, 200), 60f, 130f, true, 60f, new Action<float>(delegate (float value) { UpdateValues(); }));
			myModSettings.AddSlider("Sprint FOV", "Camera", new Color32(100, 100, 164, 200), 20f, 160f, true, 70f, new Action<float>(delegate (float value) { UpdateValues(); }));

			myModSettings.AddToggle("Headbob", "Camera", new Color32(255, 179, 174, 200), true, new Action<bool>(delegate (bool value) { UpdateValues(); }));
			myModSettings.AddToggle("Pointer", "Camera", new Color32(255, 179, 174, 200), true, new Action<bool>(delegate (bool value) { UpdateValues(); }));

			myModSettings.AddButton("Apply", "Save", new Color32(180, 200, 104, 255), new Action(delegate { UpdateValues(); }));

			UpdateValues();
			isInitialized = true;
		}

		public static void UpdateValues()
		{
			myModSettings.GetValueKeyCode("Enter FP", "General", out InputMain.enterFP);
			myModSettings.GetValueKeyCode("Build mode", "General", out InputMain.enterBuild);

			myModSettings.GetValueKeyCode("Forward", "Movement", out InputMain.forward);
			myModSettings.GetValueKeyCode("Back", "Movement", out InputMain.back);
			myModSettings.GetValueKeyCode("Left", "Movement", out InputMain.left);
			myModSettings.GetValueKeyCode("Right", "Movement", out InputMain.right);
			myModSettings.GetValueKeyCode("Jetpack", "Movement", out InputMain.jump);
			myModSettings.GetValueKeyCode("Zoom", "Movement", out InputMain.zoom);
			myModSettings.GetValueKeyCode("Sprint", "Movement", out InputMain.sprint);

			myModSettings.GetValueFloat("Jetpack Power", "Movement", out InputMain.jetpackPower);
			myModSettings.GetValueFloat("Sprint Speed", "Movement", out InputMain.sprintSpeed);
			myModSettings.GetValueFloat("Walk Speed", "Movement", out InputMain.walkSpeed);
			myModSettings.GetValueFloat("Normal FOV", "Camera", out InputMain.cameraFOV);
			myModSettings.GetValueFloat("Sprint FOV", "Camera", out InputMain.sprintFOV);
			myModSettings.GetValueBool("Headbob", "Camera", out InputMain.headbobEnabled);
			myModSettings.GetValueBool("Pointer", "Camera", out InputMain.pointerEnabled);

			myModSettings.GetValueBool("Jetpack Audio", "Audio", out InputMain.jetpackAudio);
			myModSettings.GetValueBool("Footstep Audio", "Audio", out InputMain.footstepAudio);


			if (crosshairObject != null)
			{
				if (InputMain.pointerEnabled)
				{
					crosshairObject.gameObject.SetActive(true);
				}
				else
				{
					crosshairObject.gameObject.SetActive(false);
				}
			}
		}
	}
}
