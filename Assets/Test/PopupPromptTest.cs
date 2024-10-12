using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua.InputSystem;
using MuHua;

public class PopupPromptTest : MonoBehaviour {
    public UIDocument document;
    public GameObject cube;
    private VisualElement rootElement;
    private void Awake() {
        ISKeyboard.Find(KeyCode.B).OnDown += PopupPromptTest_OnDown;
        rootElement = document.rootVisualElement;
    }
    private void OnDestroy() {
        ISKeyboard.Find(KeyCode.B).OnDown -= PopupPromptTest_OnDown;
    }
    private void PopupPromptTest_OnDown() {
        rootElement.Q<MUPopupPrompt>().Open("是否隐藏Cube?", () => { cube.SetActive(false); });
    }
}
