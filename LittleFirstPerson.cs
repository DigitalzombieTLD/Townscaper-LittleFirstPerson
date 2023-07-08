using MelonLoader;
using System.Collections;
using UnityEngine;
using ModUI;
using System.Reflection;
using System.IO;

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

		public static Vector3 maxVelocity = new Vector3(1, 0, 1);
		public static Vector3 currentVelocity = new Vector3(0, 0, 0);
		public static Vector3 minVelocity = new Vector3(-1, 0, -1);

		public static ModSettings myModSettings;

		public override void OnInitializeMelon()
		{
			LoadEmbeddedAssetBundle();
        }


		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if(sceneName == "Placemaker")
			{
				if(!fpsPlayerPrefab)
				{
					fpsPlayerPrefab = littleFirstPersonBundle.LoadAsset<GameObject>("FPS");
					AudioMain.LoadAudioFromBundle();

					UnityEngine.Object.DontDestroyOnLoad(fpsPlayerPrefab);
					MyModUI.Initialize(this);
				}				
			}			
		}
		
		public override void OnUpdate()
		{
			InputMain.GetInput();
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

        public static void LoadEmbeddedAssetBundle()
        {
            MemoryStream memoryStream;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LittleFirstPerson.Resources.LittleFirstPersonBundle");
            memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);

            littleFirstPersonBundle = Il2CppAssetBundleManager.LoadFromMemory(memoryStream.ToArray());
        }
    }
}
