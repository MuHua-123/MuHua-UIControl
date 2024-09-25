using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using MuHua.UIControl;

namespace MuHuaEditor.UIControl {
    public class CreateUSS : Editor {
        public static readonly string USS = "Packages/com.muhua.uicontrol/Assets/USS/";
        /// <summary>
        /// 创建自定义控件的USS
        /// </summary>
        /// <param name="ussResourcesPath"></param>
        /// <param name="name"></param>
        private static void USSCreate(string usspath, string name) {
            string selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string filePath = $"{selectPath}/{name}.uss";
            AssetDatabase.CopyAsset(usspath + ".uss", filePath);
            AssetDatabase.Refresh();
            Object asset = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));
            Selection.activeObject = asset;
        }
        [MenuItem("Assets/Create/UI Toolkit/VerticalScrollView USS")]
        private static void USSVerticalScrollView() {
            USSCreate(USS + "VerticalScrollView", typeof(VerticalScrollView).Name);
        }
        [MenuItem("Assets/Create/UI Toolkit/PopupPrompt USS")]
        private static void USSPopupPrompt() {
            USSCreate(USS + "PopupPrompt", typeof(PopupPrompt).Name);
        }
    }
}
