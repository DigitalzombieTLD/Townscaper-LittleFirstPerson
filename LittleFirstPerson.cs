using MelonLoader;
using System.Collections;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;


namespace LittleFirstPerson
{
    public class LittleFirstPersonMain : MelonMod
    {		
		public static Il2CppAssetBundle littleFirstPersonBundle;
		public static GameObject fpsPlayerPrefab;
		public static GameObject fpsPlayer;
		public static bool fpsActive;
		public static bool buildMode;

		public static Camera originalCamera;
		public static Camera fpsCamera;

		public static GameObject skyboxObject;
		public static GameObject skyboxCameraObject;
		public static GameObject orbitalCameraObject;

		//[DllImport("user32.dll")]
		//static extern bool SetCursorPos(int X, int Y);

		public override void OnApplicationStart()
		{
			ClassInjector.RegisterTypeInIl2Cpp<FirstPersonController>();


			MelonLogger.Msg("Loading assetbundle ...");
			littleFirstPersonBundle = Il2CppAssetBundleManager.LoadFromFile("Mods\\LittleFirstPersonBundle.unity3d");

			if (littleFirstPersonBundle == null)
			{
				MelonLogger.Msg("Failed to load assetbundle: \"LittleFirstPersonBundle.unity3d\"");
				return;
			}
			else
			{
				
			}
		}


		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if(sceneName == "Placemaker")
			{
				if(!fpsPlayerPrefab)
				{
					fpsPlayerPrefab = littleFirstPersonBundle.LoadAsset<GameObject>("FPS");
					UnityEngine.Object.DontDestroyOnLoad(fpsPlayerPrefab);
				}
			}			
		}
		
		public override void OnUpdate()
		{
			InputMain.GetInput();

			if(fpsActive)
			{
				//Cursor.visible = false;
				//Cursor.lockState = CursorLockMode.Locked;
				//var mousePos = Input.mousePosition;
				//mousePos.x -= Screen.width / 2;
				//mousePos.y -= Screen.height / 2;

				//SetCursorPos(Screen.width / 2, Screen.height / 2);
			}
		}

		public static IEnumerator DisableCursor()
		{
			yield return new WaitForEndOfFrame();
			
			if(fpsActive)
			{				
				//Cursor.lockState = CursorLockMode.Locked;
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
			}

		}
	}
}
