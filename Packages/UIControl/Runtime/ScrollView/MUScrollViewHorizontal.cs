using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MuHua.UIControl {
    public class MUScrollViewHorizontal : VisualElement {
        public new class UxmlFactory : UxmlFactory<MUScrollViewHorizontal, UxmlTraits> { }
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
            public UxmlIntAttributeDescription Count = new UxmlIntAttributeDescription {
                name = "count"
            };
            public UxmlStringAttributeDescription AssetPath = new UxmlStringAttributeDescription {
                name = "asset-path"
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                MUScrollViewHorizontal scrollView = (MUScrollViewHorizontal)ve;
                scrollView.MouseWheelScrollSize = MouseWheelScrollSize.GetValueFromBag(bag, cc);
                scrollView.scroller.MouseWheelScrollSize = MouseWheelScrollSize.GetValueFromBag(bag, cc);
                scrollView.Elasticity = Elasticity.GetValueFromBag(bag, cc);
                scrollView.SlidingValue = SlidingValue.GetValueFromBag(bag, cc);
                scrollView.AssetPath = AssetPath.GetValueFromBag(bag, cc);
#if UNITY_EDITOR
                VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(scrollView.AssetPath);
                scrollView.container.Clear();
                if (asset == null) { return; }
                int count = Count.GetValueFromBag(bag, cc);
                for (int i = 0; i < count; i++) {
                    scrollView.container.Add(asset.Instantiate());
                }
#endif
            }
        }
        public MUScrollerHorizontal scroller = new MUScrollerHorizontal();
        public VisualElement viewport = new VisualElement();
        public VisualElement container = new VisualElement();

        private bool isDragger;
        private float mousePosition;

        public string AssetPath { get; set; }
        public float Elasticity { get; set; }
        public float MouseWheelScrollSize { get; set; }
        public float SlidingValue { get; set; }
        public float MaxPosition {
            get => viewport.resolvedStyle.width - container.resolvedStyle.width;
        }

        public MUScrollViewHorizontal() {
            //设置名称
            viewport.name = "viewport";
            container.name = "container";
            //设置USS类名
            AddToClassList("horizontal-scroll-view");
            viewport.AddToClassList("horizontal-scroll-view-viewport");
            container.AddToClassList("horizontal-scroll-view-container");

            scroller.ClearClassList();
            scroller.AddToClassList("horizontal-scroll-view-scroller");

            VisualElement dragger = scroller.dragger;
            dragger.ClearClassList();
            dragger.AddToClassList("horizontal-scroll-view-dragger");
            //设置层级结构
            hierarchy.Add(viewport);
            hierarchy.Add(scroller);
            viewport.Add(container);
            //设置事件
            scroller.SlidingValueChanged += Scroller_SlidingValueChanged; ;
            viewport.RegisterCallback<PointerDownEvent>(DownDragger);
            viewport.RegisterCallback<PointerMoveEvent>(DragDragger);
            viewport.RegisterCallback<WheelEvent>(WheelDragger);
            viewport.RegisterCallback<PointerUpEvent>(ReleaseDragger);
            viewport.RegisterCallback<PointerLeaveEvent>(ReleaseDragger);
            container.RegisterCallback<TransitionEndEvent>(evt => { ElasticRestoration(); });
        }
        private void Scroller_SlidingValueChanged(float obj) {
            SlidingValue = obj;
            container.transform.position = new Vector3(MaxPosition * SlidingValue, 0);
        }
        private void DownDragger(PointerDownEvent evt) {
            isDragger = true;
            mousePosition = evt.position.x - SlidingValue * MaxPosition;
        }
        private void DragDragger(PointerMoveEvent evt) {
            if (!isDragger) { return; }
            float offset = evt.position.x - mousePosition;
            offset = Mathf.Clamp(offset, MaxPosition - Elasticity, Elasticity);
            SlidingValue = offset / MaxPosition;
            mousePosition = evt.position.x - SlidingValue * MaxPosition;
            container.transform.position = new Vector3(MaxPosition * SlidingValue, 0);
            scroller.UpdatePosition(SlidingValue);
        }
        private void WheelDragger(WheelEvent evt) {
            float value = Mathf.Clamp(evt.delta.y, -1, 1);
            float position = SlidingValue * MaxPosition;
            float offset = position + value * MouseWheelScrollSize;
            offset = Mathf.Clamp(offset, MaxPosition - Elasticity, Elasticity);
            SlidingValue = offset / MaxPosition;
            container.transform.position = new Vector3(MaxPosition * SlidingValue, 0);
            scroller.UpdatePosition(SlidingValue);
        }
        private void ReleaseDragger(PointerUpEvent evt) {
            isDragger = false;
            ElasticRestoration();
        }
        private void ReleaseDragger(PointerLeaveEvent evt) {
            isDragger = false;
            ElasticRestoration();
        }
        private void ElasticRestoration() {
            container.schedule.Execute(() => {
                SlidingValue = Mathf.Clamp(SlidingValue, 0, 1);
                container.transform.position = new Vector3(MaxPosition * SlidingValue, 0);
            }).StartingIn(1);
        }
    }
}
