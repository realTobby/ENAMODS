using ENAMODS.Utils;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ENAMODS.ModEntry), "[ENAMODS] BBQ Tools", "1.0.0", "kordesii")]

namespace ENAMODS
{
    public sealed class ModEntry : MelonMod
    {
        internal static Transform Player;
        internal static string SceneName = "";
        internal static string Status = "Booting…";

        GameObject _root;

        public override void OnInitializeMelon()
        {
            _ = ENAMODS.Utils.DevLog.Count; // force static ctor
            ENAMODS.Utils.DevLog.Info("[INIT] Bootstrapping…");
            var h = new HarmonyLib.Harmony("enamods.unitypatches");
            new PatchClassProcessor(h, typeof(Patches.PlayerMover_BlockWhenFreecam)).Patch();
        }

        public override void OnUpdate()
        {
            if (_root != null || Time.frameCount < 5) return;

            _root = new GameObject("ENAMODS_BBQTOOLS_ROOT");
            Object.DontDestroyOnLoad(_root);

            _root.AddComponent<PlayerLocator>();
            _root.AddComponent<UI.HudOverlay>();      // F9
            _root.AddComponent<FreeCamController>();  // F1

            DevLog.Info("[INIT] Root created. HUD/Console/FreeCam attached.");
            Status = "Waiting for scene…";

            
            DevLog.Info("[INIT] Bootstrapping…");
        }
    }
}
