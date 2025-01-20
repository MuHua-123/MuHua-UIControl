using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提示模块
/// </summary>
public class SinglePrompt : ModuleSingle<SinglePrompt> {
    public IPrompt prompt;

    protected override void Awake() => NoReplace();

    /// <summary> 加载页面 </summary>
    public void Loading(bool active) {
        prompt.Loading(active);
    }
    /// <summary> 进度面板 </summary>
    public void Progress(bool active, float value = 0) {
        prompt.Progress(active, value);
    }
    /// <summary> 发送消息 </summary>
    public void Sending1(bool active, string value = "") {
        prompt.Sending1(active, value);
    }
    /// <summary> 发送消息 </summary>
    public void Sending2(bool active, string value = "", Action callback = null) {
        prompt.Sending2(active, value, callback);
    }
    /// <summary> 发送消息 </summary>
    public void Sending3(bool active, string value = "", Action callback1 = null, Action callback2 = null) {
        prompt.Sending3(active, value, callback1, callback2);
    }
}
/// <summary>
/// 提示接口
/// </summary>
public interface IPrompt {
    /// <summary> 加载页面 </summary>
    public void Loading(bool active);
    /// <summary> 进度面板 </summary>
    public void Progress(bool active, float value);
    /// <summary> 发送消息 </summary>
    public void Sending1(bool active, string value);
    /// <summary> 发送消息 </summary>
    public void Sending2(bool active, string value, Action callback);
    /// <summary> 发送消息 </summary>
    public void Sending3(bool active, string value, Action callback1, Action callback2);
}