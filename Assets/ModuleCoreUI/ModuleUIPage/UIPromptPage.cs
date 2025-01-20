using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提示页面
/// </summary>
public class UIPromptPage : ModuleUIPage, IPrompt {
    public UILoadingPanel loading;
    public UIProgressPanel progress;
    public UIMessagePanel message;

    public void Awake() => GetComponent<SinglePrompt>().prompt = this;

    /// <summary> 加载页面 </summary>
    public void Loading(bool active) {
        loading.Enable(active);
    }
    /// <summary> 进度面板 </summary>
    public void Progress(bool active, float value) {
        progress.Enable(active, value);
    }
    /// <summary> 发送消息 </summary>
    public void Sending1(bool active, string value) {
        message.Clear();
        message.Sending1(active, value);
    }
    /// <summary> 发送消息 </summary>
    public void Sending2(bool active, string value, Action callback) {
        message.Clear();
        message.Sending2(active, value, callback);
    }
    /// <summary> 发送消息 </summary>
    public void Sending3(bool active, string value, Action callback1, Action callback2) {
        message.Clear();
        message.Sending3(active, value, callback1, callback2);
    }
}
