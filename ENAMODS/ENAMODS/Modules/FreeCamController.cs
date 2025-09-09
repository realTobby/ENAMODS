using HarmonyLib;
using UnityEngine;
using ENAMODS.Utils;

namespace ENAMODS
{
    [DefaultExecutionOrder(9500)]
    public sealed class FreeCamController : MonoBehaviour
    {
        public static bool IsActive { get; private set; }
        public static float CurrentSpeed { get; private set; } = 6f;

        Transform _player;
        Rigidbody _rb;
        Component _mover;
        bool _moverWasEnabled, _ccWasEnabled, _rbWasKinematic;

        void Update()
        {
            if (!_player && ModEntry.Player) Grab();

            if (Input.GetKeyDown(KeyCode.F1)) Toggle(!IsActive);
            if (!IsActive) return;

            var wheel = Input.mouseScrollDelta.y;
            if (Mathf.Abs(wheel) > 0.001f)
            {
                CurrentSpeed = Mathf.Clamp(CurrentSpeed * (wheel > 0 ? 1.25f : 0.8f), 0.5f, 120f);
                DevLog.Info($"[FreeCam] Speed {CurrentSpeed:0.##}");
            }

            var cam = Camera.main ? Camera.main.transform : _player;
            Vector3 input = new Vector3(
                (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
                (Input.GetKey(KeyCode.Space) ? 1 : 0) - (Input.GetKey(KeyCode.LeftControl) ? 1 : 0),
                (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
            );

            if (input.sqrMagnitude > 0f)
            {
                Vector3 dir = cam.TransformDirection(input.normalized);
                _player.position += dir * CurrentSpeed * Time.deltaTime;
            }

            if (_rb) { _rb.velocity = Vector3.zero; _rb.angularVelocity = Vector3.zero; }
        }

        void Grab()
        {
            _player = ModEntry.Player;
            if (!_player) return;
            _rb = _player.GetComponent<Rigidbody>();
            _mover = _player.GetComponent(AccessTools.TypeByName("JoelG.ENA4.PlayerMover"));
        }

        void Toggle(bool on)
        {
            if (!_player) Grab();
            if (!_player) { DevLog.Warn("[FreeCam] No player root."); return; }

            IsActive = on;
            var cc = _player.GetComponent<CharacterController>();

            if (on)
            {
                _rbWasKinematic = _rb ? _rb.isKinematic : false;
                if (_rb) { _rb.isKinematic = true; _rb.useGravity = false; _rb.velocity = Vector3.zero; }
                if (cc) { _ccWasEnabled = cc.enabled; cc.enabled = false; }
                if (_mover != null)
                {
                    var prop = _mover.GetType().GetProperty("enabled");
                    if (prop != null && prop.CanWrite)
                    { _moverWasEnabled = (bool)prop.GetValue(_mover, null); prop.SetValue(_mover, false, null); }
                }
                DevLog.Info("[FreeCam] ON (F1). Scroll = speed.");
            }
            else
            {
                if (_mover != null)
                {
                    var prop = _mover.GetType().GetProperty("enabled");
                    if (prop != null && prop.CanWrite) prop.SetValue(_mover, _moverWasEnabled, null);
                }
                if (_rb) { _rb.isKinematic = _rbWasKinematic; _rb.useGravity = true; }
                if (cc) cc.enabled = _ccWasEnabled;

                DevLog.Info("[FreeCam] OFF.");
            }
        }
    }
}
