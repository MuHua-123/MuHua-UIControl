using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI控件
/// </summary>
public class ModulePanel {
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;
    /// <summary> 绑定的元素 </summary>
    public readonly VisualElement element;
    /// <summary> UI控件 </summary>
    public ModulePanel(VisualElement element) => this.element = element;
}