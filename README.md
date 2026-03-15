# MornReport

InstantReplayを利用した画面録画とDiscord Webhookへの動画送信を行うライブラリ

## 依存関係

- UniTask
- MornGlobal
- [InstantReplay](https://github.com/CyberAgentGameEntertainment/InstantReplay)

## 使い方

### 録画開始

```csharp
// デフォルト（10秒）で録画開始
MornReportCore.Start();

// 録画秒数を指定して開始
MornReportCore.Start(seconds: 30f);
```

### Discord Webhookへ送信

```csharp
// 録画を停止し、動画をDiscord Webhookに送信
await MornReportCore.SendAsync("https://discord.com/api/webhooks/...");
```

## 主要クラス

| クラス | 機能 |
|---|---|
| `MornReportCore` | 録画の開始・停止・Discord Webhook送信 |
