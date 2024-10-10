using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua.UIControl {
    public class MUScrollerHorizontal : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUScrollerHorizontal, UxmlTraits> { }
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
                MUScrollerHorizontal scroller = (MUScrollerHorizontal)ve;
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
            get => resolvedStyle.width - dragger.resolvedStyle.width;
        }

        public void UpdatePosition(float value, bool callback = false) {
            SlidingValue = Mathf.Clamp(value, 0, 1);
            UpdatePosition(callback);
        }
        public void UpdatePosition(bool callback = true) {
            dragger.transform.position = new Vector3(MaxPosition * SlidingValue, 0);
            if (callback) { SlidingValueChanged?.Invoke(SlidingValue); }
        }

        public MUScrollerHorizontal() {
            //设置名称
            dragger.name = "dragger";
            //设置USS类名
            AddToClassList("horizontal-scroller");
            dragger.AddToClassList("horizontal-scroller-dragger");
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
            mousePosition = evt.position.x - SlidingValue * MaxPosition;
        }
        private void DragDragger(PointerMoveEvent evt) {
            if (!isDragger) { return; }
            float offset = evt.position.x - mousePosition;
            offset = Mathf.Clamp(offset, 0, MaxPosition);
            SlidingValue = offset / MaxPosition;
            mousePosition = evt.position.x - SlidingValue * MaxPosition;
            UpdatePosition();
        }
        private void DownScroller(PointerDownEvent evt) {
            float offset = evt.localPosition.x - dragger.resolvedStyle.width * 0.5f;
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
