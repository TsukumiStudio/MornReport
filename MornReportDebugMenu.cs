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
            _senderNameInput = MornReportCore.SenderName;
            _recordSecondsInput = MornReportCore.RecordSeconds.ToString("F0");
            _bufferSizeMBInput = (MornReportCore.BufferSize / (1024 * 1024)).ToString();
            yield return ("レポート", () =>
            {
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Label("--- 送信者 ---");
                    _senderNameInput = GUILayout.TextField(_senderNameInput);
                    if (_senderNameInput != MornReportCore.SenderName)
                    {
                        MornReportCore.SenderName = _senderNameInput;
                    }

                    var includeSystemInfo = GUILayout.Toggle(MornReportCore.IncludeSystemInfo, "システム情報を添える");
                    if (includeSystemInfo != MornReportCore.IncludeSystemInfo)
                    {
                        MornReportCore.IncludeSystemInfo = includeSystemInfo;
                    }

                    GUILayout.Space(10);
                    GUILayout.Label("--- 録画設定 ---");
                    GUILayout.Label($"録画秒数: {MornReportCore.RecordSeconds}秒");
                    _recordSecondsInput = GUILayout.TextField(_recordSecondsInput);
                    if (float.TryParse(_recordSecondsInput, out var seconds) && seconds > 0)
                    {
                        MornReportCore.RecordSeconds = seconds;
                    }

                    GUILayout.Label($"バッファサイズ: {MornReportCore.BufferSize / (1024 * 1024)}MB");
                    _bufferSizeMBInput = GUILayout.TextField(_bufferSizeMBInput);
                    if (long.TryParse(_bufferSizeMBInput, out var mb) && mb > 0)
                    {
                        MornReportCore.BufferSize = mb * 1024 * 1024;
                    }
                }
            });
        }
    }
}
