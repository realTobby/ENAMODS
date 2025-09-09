using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ENAMODS.Utils
{
    public sealed class PlayerLocator : MonoBehaviour
    {
        Coroutine _loop;

        void OnEnable()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            SceneManager.sceneLoaded += OnLoaded;
            _loop = StartCoroutine(ScanLoop());
            DevLog.Info("[Locator] Enabled");
        }
        void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            SceneManager.sceneLoaded -= OnLoaded;
            if (_loop != null) StopCoroutine(_loop);
        }

        void OnSceneChanged(Scene _, Scene next) { ModEntry.SceneName = next.name; ModEntry.Player = null; ModEntry.Status = "Scene changed → reacquire player…"; }
        void OnLoaded(Scene scene, LoadSceneMode __) { ModEntry.SceneName = scene.name; ModEntry.Player = null; ModEntry.Status = "Scene loaded → reacquire player…"; }

        IEnumerator ScanLoop()
        {
            while (true)
            {
                while (ModEntry.Player == null)
                {
                    TryFind();
                    if (ModEntry.Player != null) { ModEntry.Status = "Player found."; DevLog.Info($"[Locator] Player root: {ModEntry.Player.name}"); break; }
                    yield return new WaitForSeconds(0.5f);
                }
                while (ModEntry.Player != null) yield return new WaitForSeconds(1f);
            }
        }

        static void TryFind()
        {
            var tagged = GameObject.FindGameObjectWithTag("Player");
            if (tagged) { ModEntry.Player = tagged.transform; return; }

            var cc = Object.FindObjectOfType<CharacterController>();
            if (cc) { ModEntry.Player = cc.transform; return; }

            var cam = Camera.main;
            if (cam && cam.transform.root && !cam.transform.root.name.ToLower().Contains("menu"))
            { ModEntry.Player = cam.transform.root; return; }

            foreach (var t in Object.FindObjectsOfType<Transform>())
                if (t.name.ToLower().Contains("player")) { ModEntry.Player = t; return; }
        }

        public static bool OwnedByPlayer(Transform t)
        {
            var p = ModEntry.Player;
            return p && (t == p || t.IsChildOf(p));
        }
    }
}
