using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

public class InputFieldText : MonoBehaviour {
    public UIDocument document;
    private VisualElement rootElement;
    private void Awake() {
        rootElement = document.rootVisualElement;
        rootElement.Q<MULongField>().label = "ssssssssss";
        rootElement.Q<MULongField>().value = 494651;
    }
}
