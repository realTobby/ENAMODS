using System.Collections.Generic;
using System.Diagnostics;
using MelonLoader;
using UnityEngine;

namespace ENAMODS.Utils
{
    public static class DevLog
    {
        public struct Line { public float t; public LogType type; public string msg; }

        const int MAX = 300;
        static readonly object _lock = new object();
        static readonly List<Line> _lines = new List<Line>(MAX);
        static readonly Stopwatch _sw = Stopwatch.StartNew();
        public static bool CaptureEnabled = true;

        // Force init by reading Count (use in OnInitializeMelon)
        public static int Count { get { lock (_lock) return _lines.Count; } }

        static DevLog()
        {
            Application.logMessageReceived += OnUnityLog;
            Application.logMessageReceivedThreaded += OnUnityLog;
            Add(LogType.Log, "[ENAMODS] log hook alive");
        }

        static void OnUnityLog(string condition, string stackTrace, LogType type)
        {
            Add(type, condition);
        }

        public static void Info(string m) { MelonLogger.Msg(m); Add(LogType.Log, "[ENAMODS] " + m); }
        public static void Warn(string m) { MelonLogger.Warning(m); Add(LogType.Warning, "[ENAMODS] " + m); }
        public static void Error(string m) { MelonLogger.Error(m); Add(LogType.Error, "[ENAMODS] " + m); }

        static void Add(LogType type, string msg)
        {
            if (!CaptureEnabled) return;
            var line = new Line { t = (float)_sw.Elapsed.TotalSeconds, type = type, msg = msg };
            lock (_lock)
            {
                if (_lines.Count >= MAX) _lines.RemoveAt(0);
                _lines.Add(line);
            }
        }

        public static Line? Last()
        {
            lock (_lock)
            {
                if (_lines.Count == 0) return null;
                return _lines[_lines.Count - 1];
            }
        }
    }
}
