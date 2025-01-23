using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 加载面板
/// </summary>
public class PageLoading : ModulePage {
    public Transform donut;
    private bool isEnable;
    private Vector3 eulerAngles;

    public override VisualElement Element => throw new System.NotImplementedException();

    protected void Update() {
        if (!isEnable) { return; }
        eulerAngles.z -= Time.deltaTime * 360;
        donut.eulerAngles = eulerAngles;
        if (eulerAngles.z < -360) { eulerAngles.z -= 360; }
    }

    /// <summary> 启用 </summary>
    public void Enable(bool active) {
        isEnable = active;
        SonActive(transform, active);
    }
    /// <summary> 批量操作 </summary>
    public void SonActive(Transform parent, bool active) {
        foreach (Transform item in parent) {
            item.gameObject.SetActive(active);
        }
    }
}
