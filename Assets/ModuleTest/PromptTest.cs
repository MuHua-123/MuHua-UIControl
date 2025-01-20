using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptTest : MonoBehaviour {
    public void OnOpen(InputValue inputValue) {
        SinglePrompt.I.Loading(true);
        //SinglePrompt.I.Progress(true, 0.6f);
        //SinglePrompt.I.Sending3(true, "sss");
    }
    public void OnClose(InputValue inputValue) {
        SinglePrompt.I.Loading(false);
        //SinglePrompt.I.Progress(false);
        //SinglePrompt.I.Sending3(false);
    }
}
