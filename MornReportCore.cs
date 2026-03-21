using System;
using System.IO;
using Cysharp.Threading.Tasks;
using InstantReplay;
using UnityEngine;
using UnityEngine.Networking;

namespace MornLib
{
    /// <summary>InstantReplayを利用した画面録画とDiscord Webhook送信</summary>
    public static class MornReportCore
    {
        private const float DefaultSeconds = 120f;
        private const long DefaultBufferSize = 120 * 1024 * 1024;
        private const string DefaultSenderName = "名無しの開発者";
        private static RealtimeInstantReplaySession _session;
        private static float _seconds;
        public static float RecordSeconds { get; set; } = DefaultSeconds;
        public static long BufferSize { get; set; } = DefaultBufferSize;
        public static string SenderName { get; set; } = DefaultSenderName;
        public static bool IncludeSystemInfo { get; set; } = true;

        /// <summary>録画を開始する</summary>
        public static void Start()
        {
            _session?.Dispose();
            var options = RealtimeEncodingOptions.Default;
            options.MaxMemoryUsageBytesForCompressedFrames = BufferSize;
            _session = new RealtimeInstantReplaySession(options);
            _seconds = RecordSeconds;
            Debug.Log($"[MornReport] 録画を開始しました（{_seconds}秒）");
        }

        /// <summary>録画を停止し、Discord Webhookに動画を送信する</summary>
        public static async UniTask SendAsync(string webhookUrl, string message = null)
        {
            if (_session == null)
            {
                Debug.LogError("[MornReport] Start()が呼ばれていません");
                return;
            }

            var path = await _session.StopAndExportAsync(_seconds);
            _session.Dispose();
            _session = null;
            Debug.Log($"[MornReport] 動画をエクスポートしました: {path}");
            var fileData = File.ReadAllBytes(path);
            var content = BuildContent(message);
            var form = new WWWForm();
            form.AddField("content", content);
            form.AddBinaryData("file", fileData, "report.mp4", "video/mp4");
            using var request = UnityWebRequest.Post(webhookUrl, form);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[MornReport] 送信に失敗しました: {request.error}");
                return;
            }

            Debug.Log("[MornReport] 動画を送信しました");
        }

        private static string BuildContent(string message)
        {
            var senderLine = SenderName;
            if (IncludeSystemInfo)
            {
                senderLine += $" ({SystemInfo.deviceName} / {SystemInfo.operatingSystem})";
            }

            var dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var content = $"{senderLine}\n{dateTime}";
            if (message != null)
            {
                content += $"\n{message}";
            }

            return content;
        }
    }
}
