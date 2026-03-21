using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornLib
{
    /// <summary>MornReportの設定をEditorPrefsで永続化するユーティリティ</summary>
    public static class MornReportUtil
    {
        private const string KeyPrefix = "MornReport_";
        private const string KeyRecordSeconds = KeyPrefix + "RecordSeconds";
        private const string KeyBufferSizeMB = KeyPrefix + "BufferSizeMB";
        private const string KeySenderName = KeyPrefix + "SenderName";
        private const string KeyIncludeSystemInfo = KeyPrefix + "IncludeSystemInfo";
        private const string KeyEnabled = KeyPrefix + "Enabled";
        private const float DefaultRecordSeconds = 120f;
        private const int DefaultEnabled = 1;
        private const int DefaultBufferSizeMB = 120;
        private const string DefaultSenderName = "名無しの開発者";
        private const int DefaultIncludeSystemInfo = 1;

        public static float RecordSeconds
        {
            get
            {
#if UNITY_EDITOR
                return EditorPrefs.GetFloat(KeyRecordSeconds, DefaultRecordSeconds);
#else
                return DefaultRecordSeconds;
#endif
            }
            set
            {
#if UNITY_EDITOR
                EditorPrefs.SetFloat(KeyRecordSeconds, value);
#endif
            }
        }

        public static long BufferSize
        {
            get
            {
#if UNITY_EDITOR
                return (long)EditorPrefs.GetInt(KeyBufferSizeMB, DefaultBufferSizeMB) * 1024 * 1024;
#else
                return (long)DefaultBufferSizeMB * 1024 * 1024;
#endif
            }
            set
            {
#if UNITY_EDITOR
                EditorPrefs.SetInt(KeyBufferSizeMB, (int)(value / (1024 * 1024)));
#endif
            }
        }

        public static string SenderName
        {
            get
            {
#if UNITY_EDITOR
                return EditorPrefs.GetString(KeySenderName, DefaultSenderName);
#else
                return DefaultSenderName;
#endif
            }
            set
            {
#if UNITY_EDITOR
                EditorPrefs.SetString(KeySenderName, value);
#endif
            }
        }

        public static bool Enabled
        {
            get
            {
#if UNITY_EDITOR
                return EditorPrefs.GetInt(KeyEnabled, DefaultEnabled) != 0;
#else
                return true;
#endif
            }
            set
            {
#if UNITY_EDITOR
                EditorPrefs.SetInt(KeyEnabled, value ? 1 : 0);
#endif
            }
        }

        public static bool IncludeSystemInfo
        {
            get
            {
#if UNITY_EDITOR
                return EditorPrefs.GetInt(KeyIncludeSystemInfo, DefaultIncludeSystemInfo) != 0;
#else
                return true;
#endif
            }
            set
            {
#if UNITY_EDITOR
                EditorPrefs.SetInt(KeyIncludeSystemInfo, value ? 1 : 0);
#endif
            }
        }
    }
}
