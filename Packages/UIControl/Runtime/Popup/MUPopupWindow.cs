using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua.UIControl {
    public class MUPopupWindow : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUPopupWindow, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            public UxmlStringAttributeDescription TitleText = new UxmlStringAttributeDescription {
                name = "Title-Text", defaultValue = "标题"
            };
            public UxmlStringAttributeDescription ButtonText = new UxmlStringAttributeDescription {
                name = "Button-Text", defaultValue = "确认"
            };
            public UxmlTypeAttributeDescription<VisualElement> Content = new UxmlTypeAttributeDescription<VisualElement> {
                name = "Content", defaultValue = typeof(Label)
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUPopupWindow popup = ve as MUPopupWindow;
                popup.titleText = TitleText.GetValueFromBag(bag, cc);
                popup.buttonText = ButtonText.GetValueFromBag(bag, cc);
                Type content = typeof(Label);
                Content.TryGetValueFromBag(bag, cc, ref content);
                VisualElement element = Activator.CreateInstance(content) as VisualElement;
                popup.Open(element);
            }
        }
        //布局
        public VisualElement background = new VisualElement();
        public VisualElement top = new VisualElement();
        public VisualElement middle = new VisualElement();
        public VisualElement bottom = new VisualElement();
        //组件
        public Label title = new Label();
        public Button button = new Button();
        //参数
        public Action callback;
        public Type template { get; set; }
        public string titleText { get => title.text; set => title.text = value; }
        public string buttonText { get => button.text; set => button.text = value; }

        public MUPopupWindow() {
            //设置USS类名
            AddToClassList("popup-window");
            background.AddToClassList("popup-window-background");
            top.AddToClassList("popup-window-top");
            middle.AddToClassList("popup-window-middle");
            bottom.AddToClassList("popup-window-bottom");
            title.AddToClassList("popup-window-title");
            button.AddToClassList("popup-window-button");
            //设置层级结构
            hierarchy.Add(background);
            background.Add(top);
            background.Add(middle);
            background.Add(bottom);
            top.Add(title);
            bottom.Add(button);
            //设置事件
            button.RegisterCallback<ClickEvent>((evt) => Close());
        }
        public void Open() {
            visible = true;
        }
        public void Open(VisualElement element) {
            middle.Clear();
            middle.Add(element);
            visible = true;
        }
        public void Close() {
            callback?.Invoke();
            callback = null;
            visible = false;
        }
    }
}