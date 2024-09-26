using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MuHua.UIControl {
    public class MUFloatField : FloatField {
        public new class UxmlFactory : UxmlFactory<MUFloatField, UxmlTraits> { }
        public new class UxmlTraits : FloatField.UxmlTraits { }
        public MUFloatField()  {
            ClearClassList();
            AddToClassList("input-field");

            labelElement.ClearClassList();
            labelElement.AddToClassList("unity-text-element");
            labelElement.AddToClassList("input-field-label");

            VisualElement inputElement = this.Q<VisualElement>("unity-text-input");
            inputElement.ClearClassList();
            inputElement.AddToClassList("input-field-box");

            VisualElement textElement = inputElement.Q<VisualElement>("");
            textElement.ClearClassList();
            textElement.AddToClassList("input-field-text");
        }
    }
}
