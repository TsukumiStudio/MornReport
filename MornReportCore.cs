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
        private static RealtimeInstantReplaySession _session;
        private static float _seconds;

        /// <summary>録画を開始する</summary>
        public static void Start()
        {
            _session?.Dispose();
            var options = RealtimeEncodingOptions.Default;
            options.MaxMemoryUsageBytesForCompressedFrames = MornReportUtil.BufferSize;
            options.ForceReadback = true;
            options.VideoLagAdjustmentThreshold = double.MaxValue;
            _session = new RealtimeInstantReplaySession(
                options,
                onException: e => Debug.LogError($"[MornReport] 録画エラー: {e}")
            );
            _seconds = MornReportUtil.RecordSeconds;
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
            var senderLine = MornReportUtil.SenderName;
            if (MornReportUtil.IncludeSystemInfo)
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
