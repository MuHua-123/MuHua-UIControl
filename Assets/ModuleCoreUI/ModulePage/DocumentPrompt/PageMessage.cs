using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 消息面板
/// </summary>
public class PageMessage : ModulePage {
    private Action callback1;
    private Action callback2;
    private Action callback3;

    public override VisualElement Element => document.Q<VisualElement>("Message");
    public VisualElement Mask => Element.Q<VisualElement>("Mask");
    public VisualElement Message1 => Element.Q<VisualElement>("Message1");
    public VisualElement Message2 => Element.Q<VisualElement>("Message2");
    public VisualElement Message3 => Element.Q<VisualElement>("Message3");

    public Label Message1Content => Message1.Q<Label>("Content");
    public Label Message2Content => Message2.Q<Label>("Content");
    public Label Message3Content => Message3.Q<Label>("Content");

    public Button Button1 => Message2.Q<Button>("Button1");
    public Button Button2 => Message3.Q<Button>("Button2");
    public Button Button3 => Message3.Q<Button>("Button3");

    public void Awake() {
        Button1.clicked += () => { callback1?.Invoke(); Clear(); };
        Button2.clicked += () => { callback2?.Invoke(); Clear(); };
        Button3.clicked += () => { callback3?.Invoke(); Clear(); };
    }

    /// <summary> 发送消息 </summary>
    public void Sending1(bool active, string value) {
        Mask.EnableInClassList("message-panel-hide", !active);
        Message1.EnableInClassList("message-panel-hide", !active);
        Message1Content.text = value;
    }
    /// <summary> 发送消息 </summary>
    public void Sending2(bool active, string value, Action callback) {
        Mask.EnableInClassList("message-panel-hide", !active);
        Message2.EnableInClassList("message-panel-hide", !active);
        Message2Content.text = value;
        callback1 = callback;
    }
    /// <summary> 发送消息 </summary>
    public void Sending3(bool active, string value, Action callback1, Action callback2) {
        Mask.EnableInClassList("message-panel-hide", !active);
        Message3.EnableInClassList("message-panel-hide", !active);
        Message3Content.text = value;
        this.callback2 = callback1;
        this.callback3 = callback2;
    }
    /// <summary> 清空消息 </summary>
    public void Clear() {
        Mask.EnableInClassList("message-panel-hide", true);
        Message1.EnableInClassList("message-panel-hide", true);
        Message2.EnableInClassList("message-panel-hide", true);
        Message3.EnableInClassList("message-panel-hide", true);
    }
}
