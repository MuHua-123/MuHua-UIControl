using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuHua.UIControl {
    public class MUScrollViewVertical : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUScrollViewVertical, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlIntAttributeDescription MouseWheelScrollSize = new UxmlIntAttributeDescription {
                name = "mouse-wheel-scroll-size",
                defaultValue = 18
            };
            private UxmlFloatAttributeDescription Elasticity = new UxmlFloatAttributeDescription {
                name = "elasticity",
                defaultValue = 10
            };
            private UxmlFloatAttributeDescription SlidingValue = new UxmlFloatAttributeDescription {
                name = "sliding-value",
                defaultValue = 0
            };
            public UxmlStringAttributeDescription AssetPath = new UxmlStringAttributeDescription {
                name = "Asset-Path"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUScrollViewVertical scrollView = (MUScrollViewVertical)ve;
                scrollView.MouseWheelScrollSize = MouseWheelScrollSize.GetValueFromBag(bag, cc);
                scrollView.scroller.MouseWheelScrollSize = MouseWheelScrollSize.GetValueFromBag(bag, cc);
                scrollView.Elasticity = Elasticity.GetValueFromBag(bag, cc);
                scrollView.SlidingValue = SlidingValue.GetValueFromBag(bag, cc);
                scrollView.AssetPath = AssetPath.GetValueFromBag(bag, cc);
#if UNITY_EDITOR
                VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(scrollView.AssetPath);
                if (asset != null) {
                    scrollView.container.Clear();
                    scrollView.container.Add(asset.Instantiate());
                }
                else { scrollView.container.Clear(); }
#endif
            }
        }
        public MUScrollerVertical scroller = new MUScrollerVertical();
        public VisualElement viewport = new VisualElement();
        public VisualElement container = new VisualElement();

        private bool isDragger;
        private float mousePosition;

        public string AssetPath { get; set; }
        public float Elasticity { get; set; }
        public float MouseWheelScrollSize { get; set; }
        public float SlidingValue { get; set; }
        public float MaxPosition {
            get => viewport.resolvedStyle.height - container.resolvedStyle.height;
        }

        public MUScrollViewVertical() {
            //设置名称
            viewport.name = "viewport";
            container.name = "container";
            //设置USS类名
            AddToClassList("vertical-scroll-view");
            viewport.AddToClassList("vertical-scroll-view-viewport");
            container.AddToClassList("vertical-scroll-view-container");

            scroller.ClearClassList();
            scroller.AddToClassList("vertical-scroll-view-scroller");

            VisualElement dragger = scroller.dragger;
            dragger.ClearClassList();
            dragger.AddToClassList("vertical-scroll-view-dragger");
            //设置层级结构
            hierarchy.Add(viewport);
            hierarchy.Add(scroller);
            viewport.Add(container);
            //设置事件
            scroller.SlidingValueChanged += Scroller_SlidingValueChanged;
            viewport.RegisterCallback<PointerDownEvent>(DownDragger);
            viewport.RegisterCallback<PointerMoveEvent>(DragDragger);
            viewport.RegisterCallback<WheelEvent>(WheelDragger);
            viewport.RegisterCallback<PointerUpEvent>(ReleaseDragger);
            viewport.RegisterCallback<PointerLeaveEvent>(ReleaseDragger);
        }
        private void Scroller_SlidingValueChanged(float obj) {
            SlidingValue = obj;
            container.transform.position = new Vector3(0, MaxPosition * SlidingValue);
        }
        private void DownDragger(PointerDownEvent evt) {
            isDragger = true;
            mousePosition = evt.position.y - SlidingValue * MaxPosition;
        }
        private void DragDragger(PointerMoveEvent evt) {
            if (!isDragger) { return; }
            float offset = evt.position.y - mousePosition;
            offset = Mathf.Clamp(offset, MaxPosition - Elasticity, Elasticity);
            SlidingValue = offset / MaxPosition;
            mousePosition = evt.position.y - SlidingValue * MaxPosition;
            container.transform.position = new Vector3(0, MaxPosition * SlidingValue);
            scroller.UpdatePosition(SlidingValue);
        }
        private void WheelDragger(WheelEvent evt) {
            float value = Mathf.Clamp(evt.delta.y, -1, 1);
            float position = SlidingValue * MaxPosition;
            float offset = position + value * MouseWheelScrollSize;
            offset = Mathf.Clamp(offset, MaxPosition - Elasticity, Elasticity);
            SlidingValue = offset / MaxPosition;
            container.transform.position = new Vector3(0, MaxPosition * SlidingValue);
            scroller.UpdatePosition(SlidingValue);
            container.schedule.Execute(() => {
                SlidingValue = Mathf.Clamp(SlidingValue, 0, 1);
                container.transform.position = new Vector3(0, MaxPosition * SlidingValue);
            }).StartingIn(1);
        }
        private void ReleaseDragger(PointerUpEvent evt) {
            isDragger = false;
            container.schedule.Execute(() => {
                SlidingValue = Mathf.Clamp(SlidingValue, 0, 1);
                container.transform.position = new Vector3(0, MaxPosition * SlidingValue);
            }).StartingIn(1);
        }
        private void ReleaseDragger(PointerLeaveEvent evt) {
            isDragger = false;
            container.schedule.Execute(() => {
                SlidingValue = Mathf.Clamp(SlidingValue, 0, 1);
                container.transform.position = new Vector3(0, MaxPosition * SlidingValue);
            }).StartingIn(1);
        }
    }
}
