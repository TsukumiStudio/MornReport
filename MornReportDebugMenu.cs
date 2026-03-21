using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornReportDebugMenu), menuName = "Morn/" + nameof(MornReportDebugMenu))]
    public sealed class MornReportDebugMenu : MornDebugMenuBase
    {
        private string _senderNameInput;
        private string _recordSecondsInput;
        private string _bufferSizeMBInput;

        public override IEnumerable<(string key, Action action)> GetMenuItems()
        {
            _senderNameInput = MornReportUtil.SenderName;
            _recordSecondsInput = MornReportUtil.RecordSeconds.ToString("F0");
            _bufferSizeMBInput = (MornReportUtil.BufferSize / (1024 * 1024)).ToString();
            yield return ("レポート", () =>
            {
                using (new GUILayout.VerticalScope())
                {
                    var enabled = GUILayout.Toggle(MornReportUtil.Enabled, "レポートを送信する");
                    if (enabled != MornReportUtil.Enabled)
                    {
                        MornReportUtil.Enabled = enabled;
                    }

                    GUILayout.Space(10);
                    GUI.enabled = MornReportUtil.Enabled;
                    GUILayout.Label("--- 送信者 ---");
                    var newName = GUILayout.TextField(_senderNameInput);
                    if (newName != _senderNameInput)
                    {
                        _senderNameInput = newName;
                        MornReportUtil.SenderName = newName;
                    }

                    var includeSystemInfo = GUILayout.Toggle(MornReportUtil.IncludeSystemInfo, "システム情報を添える");
                    if (includeSystemInfo != MornReportUtil.IncludeSystemInfo)
                    {
                        MornReportUtil.IncludeSystemInfo = includeSystemInfo;
                    }

                    GUILayout.Space(10);
                    GUILayout.Label("--- 録画設定 ---");
                    GUILayout.Label($"録画秒数: {MornReportUtil.RecordSeconds}秒");
                    var newSeconds = GUILayout.TextField(_recordSecondsInput);
                    if (newSeconds != _recordSecondsInput)
                    {
                        _recordSecondsInput = newSeconds;
                        if (float.TryParse(newSeconds, out var seconds) && seconds > 0)
                        {
                            MornReportUtil.RecordSeconds = seconds;
                        }
                    }

                    GUILayout.Label($"バッファサイズ: {MornReportUtil.BufferSize / (1024 * 1024)}MB");
                    var newMB = GUILayout.TextField(_bufferSizeMBInput);
                    if (newMB != _bufferSizeMBInput)
                    {
                        _bufferSizeMBInput = newMB;
                        if (long.TryParse(newMB, out var mb) && mb > 0)
                        {
                            MornReportUtil.BufferSize = mb * 1024 * 1024;
                        }
                    }

                    GUI.enabled = true;
                }
            });
        }
    }
}
