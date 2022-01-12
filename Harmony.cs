using MelonLoader;
using Harmony;
using Placemaker;

namespace LittleFirstPerson
{
	public class Harmony_Main
	{
		[HarmonyLib.HarmonyPatch(typeof(HoverData), "SetHover")]
		public class SetHoverGetCoords
		{
			public static void Postfix(ref HoverData __instance)
			{
				InputMain.mousePos = __instance.pointerHitPos;
			}
		}
	}
}
