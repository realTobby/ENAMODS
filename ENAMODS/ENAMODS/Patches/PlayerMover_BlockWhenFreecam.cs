using System.Reflection;
using ENAMODS.Utils;
using HarmonyLib;
using UnityEngine;

namespace ENAMODS.Patches
{
    [HarmonyPatch]
    public static class PlayerMover_BlockWhenFreecam
    {
        static MethodBase TargetMethod()
            => AccessTools.Method(AccessTools.TypeByName("JoelG.ENA4.PlayerMover"), "FixedUpdatePlayerControl");

        [HarmonyTargetMethod] static MethodBase TM() => TargetMethod();

        [HarmonyPrefix]
        static bool Prefix(object __instance)
        {
            var c = __instance as Component;
            if (c && PlayerLocator.OwnedByPlayer(c.transform) && FreeCamController.IsActive)
                return false; // skip original while freecam is active
            return true;
        }
    }
}
