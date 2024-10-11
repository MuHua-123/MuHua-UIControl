using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuHua.UIControl {
    public class MUFoldout : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUFoldout, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription Text = new UxmlStringAttributeDescription {
                name = "text",
                defaultValue = "标题"
            };
            private UxmlBoolAttributeDescription Active = new UxmlBoolAttributeDescription {
                name = "active",
                defaultValue = false
            };
            public UxmlIntAttributeDescription Count = new UxmlIntAttributeDescription {
                name = "count"
            };
            public UxmlStringAttributeDescription AssetPath = new UxmlStringAttributeDescription {
                name = "asset-path"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUFoldout foldout = (MUFoldout)ve;
                foldout.Text = Text.GetValueFromBag(bag, cc);
                foldout.Active = Active.GetValueFromBag(bag, cc);
                foldout.UpdateVisualElement();
                foldout.AssetPath = AssetPath.GetValueFromBag(bag, cc);
#if UNITY_EDITOR
                VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(foldout.AssetPath);
                foldout.container.Clear();
                if (asset == null) { return; }
                int count = Count.GetValueFromBag(bag, cc);
                for (int i = 0; i < count; i++) {
                    foldout.container.Add(asset.Instantiate());
                }
#endif
            }
        }
        public Label label = new Label();
        public VisualElement title = new VisualElement();
        public VisualElement image = new VisualElement();
        public VisualElement container = new VisualElement();

        public string Text { get; set; }
        public bool Active { get; set; }
        public string AssetPath { get; set; }
        private void UpdateVisualElement() {
            label.text = Text;
            container.style.display = Active ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public MUFoldout() {
            //设置名称
            title.name = "title";
            image.name = "image";
            label.name = "label";
            container.name = "container";
            //设置USS类名
            AddToClassList("foldout");
            title.AddToClassList("foldout-title");
            image.AddToClassList("foldout-title-image");
            label.AddToClassList("foldout-title-label");
            container.AddToClassList("foldout-container");
            //设置层级结构
            hierarchy.Add(title);
            hierarchy.Add(container);
            title.Add(image);
            title.Add(label);
            //设置事件
            title.RegisterCallback<ClickEvent>(TriggerClick);
        }
        private void TriggerClick(ClickEvent evt) {
            Active = !Active;
            UpdateVisualElement();
        }
    }
}
