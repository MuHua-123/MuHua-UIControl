using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 进度面板
/// </summary>
public class UIProgressPanel : ModuleUIPanel {
    public override VisualElement Element => UIPage.Q<VisualElement>("Progress");
    public MUSliderHorizontal Slider => Element.Q<MUSliderHorizontal>("Slider");

    /// <summary> 启用 </summary>
    public void Enable(bool active, float value) {
        Element.EnableInClassList("prompt-panel-hide", !active);
        Slider.Value = value;
    }
}
