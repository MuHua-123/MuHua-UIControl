using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua.UIControl {
    public class MUScrollerVertical : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUScrollerVertical, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlIntAttributeDescription MouseWheelScrollSize = new UxmlIntAttributeDescription {
                name = "mouse-wheel-scroll-size",
                defaultValue = 18
            };
            private UxmlFloatAttributeDescription SlidingValue = new UxmlFloatAttributeDescription {
                name = "sliding-value",
                defaultValue = 0
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUScrollerVertical scroller = (MUScrollerVertical)ve;
                scroller.MouseWheelScrollSize = MouseWheelScrollSize.GetValueFromBag(bag, cc);
                scroller.SlidingValue = SlidingValue.GetValueFromBag(bag, cc);
                scroller.UpdatePosition();
            }
        }
        public event Action<float> SlidingValueChanged;
        public VisualElement dragger = new VisualElement();

        private bool isDragger;
        private float mousePosition;

        public float MouseWheelScrollSize { get; set; }
        public float SlidingValue { get; set; }
        public float MaxPosition {
            get => resolvedStyle.height - dragger.resolvedStyle.height;
        }

        public void UpdatePosition(float value, bool callback = false) {
            SlidingValue = Mathf.Clamp(value, 0, 1);
            UpdatePosition(callback);
        }
        public void UpdatePosition(bool callback = true) {
            dragger.transform.position = new Vector3(0, MaxPosition * SlidingValue);
            if (callback) { SlidingValueChanged?.Invoke(SlidingValue); }
        }

        public MUScrollerVertical() {
            //设置名称
            dragger.name = "dragger";
            //设置USS类名
            AddToClassList("vertical-scroller");
            dragger.AddToClassList("vertical-scroller-dragger");
            //设置层级结构
            hierarchy.Add(dragger);
            //设置事件
            dragger.RegisterCallback<PointerDownEvent>(DownDragger);
            dragger.RegisterCallback<PointerMoveEvent>(DragDragger);

            RegisterCallback<PointerDownEvent>(DownScroller);
            RegisterCallback<WheelEvent>(WheelScroller);
            RegisterCallback<PointerUpEvent>(ReleaseScroller);
            RegisterCallback<PointerLeaveEvent>(ReleaseScroller);
        }
        private void DownDragger(PointerDownEvent evt) {
            isDragger = true;
            mousePosition = evt.position.y - SlidingValue * MaxPosition;
        }
        private void DragDragger(PointerMoveEvent evt) {
            if (!isDragger) { return; }
            float offset = evt.position.y - mousePosition;
            offset = Mathf.Clamp(offset, 0, MaxPosition);
            SlidingValue = offset / MaxPosition;
            mousePosition = evt.position.y - SlidingValue * MaxPosition;
            UpdatePosition();
        }
        private void DownScroller(PointerDownEvent evt) {
            float offset = evt.localPosition.y - dragger.resolvedStyle.height * 0.5f;
            offset = Mathf.Clamp(offset, 0, MaxPosition);
            SlidingValue = offset / MaxPosition;
            UpdatePosition();
        }
        private void WheelScroller(WheelEvent evt) {
            float value = Mathf.Clamp(evt.delta.y, -1, 1);
            float position = SlidingValue * MaxPosition;
            float offset = position + value * MouseWheelScrollSize;
            offset = Mathf.Clamp(offset, 0, MaxPosition);
            SlidingValue = offset / MaxPosition;
            UpdatePosition();
        }
        private void ReleaseScroller(PointerUpEvent evt) {
            isDragger = false;
        }
        private void ReleaseScroller(PointerLeaveEvent evt) {
            isDragger = false;
        }
    }
}
