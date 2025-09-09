using UnityEngine;
using UnityEngine.UI;
using ENAMODS.Utils;

namespace ENAMODS.UI
{
    public class HudOverlay : MonoBehaviour
    {
        Canvas _canvas;
        Text _hud, _ticker;
        bool _visible = true;

        void Start()
        {
            var go = new GameObject("ENAMODS_HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            DontDestroyOnLoad(go);

            _canvas = go.GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = short.MaxValue;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            var font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            // HUD (unchanged)
            _hud = NewText("HUD_Text", go.transform, font, 18, Color.white);
            SetRect(_hud.rectTransform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(10, -10), new Vector2(800, 80), new Vector2(0, 1));
            _hud.supportRichText = true;

            // TICKER (fix: bottom-left pivot + alignment)
            _ticker = NewText("HUD_Ticker", go.transform, font, 14, Color.white);
            _ticker.alignment = TextAnchor.LowerLeft;         // <-- important
            SetRect(_ticker.rectTransform, new Vector2(0, 0), new Vector2(0, 0), new Vector2(10, 10), new Vector2(900, 30), new Vector2(0, 0));
            _ticker.supportRichText = true;

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                _visible = !_visible;
                _canvas.enabled = _visible;
            }
            if (!_visible) return;

            // Scene + Player XYZ (RGB colored)
            var p = ModEntry.Player;
            var scene = string.IsNullOrEmpty(ModEntry.SceneName)
                ? UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
                : ModEntry.SceneName;

            string xyz = (p == null)
                ? "<color=#888888>Player not found</color>"
                : $"<color=#ff5555>X:{p.position.x:0.00}</color>  " +
                  $"<color=#55ff55>Y:{p.position.y:0.00}</color>  " +
                  $"<color=#5599ff>Z:{p.position.z:0.00}</color>";

            string freecam = ENAMODS.FreeCamController.IsActive
                ? $" | <b>FreeCam</b> ON  (speed {ENAMODS.FreeCamController.CurrentSpeed:0.##})"
                : "";

            _hud.text = $"Scene: {scene} | {ModEntry.Status}{freecam}\n{xyz}";

            // 1-line ticker with the last log entry
            var last = DevLog.Last();
            _ticker.text = last.HasValue ? $"{last.Value.t:0.00}  {last.Value.msg}" : "";
        }

        Text NewText(string name, Transform parent, Font font, int size, Color color)
        {
            var go = new GameObject(name, typeof(Text));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<Text>();
            t.font = font; t.fontSize = size; t.color = color;
            t.alignment = TextAnchor.UpperLeft;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }

        // change SetRect to accept a pivot
        void SetRect(RectTransform rt, Vector2 aMin, Vector2 aMax, Vector2 pos, Vector2 size, Vector2 pivot)
        {
            rt.anchorMin = aMin;
            rt.anchorMax = aMax;
            rt.pivot = pivot;
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
        }

    }
}
