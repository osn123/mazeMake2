using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
#if true
    public static InputManager Instance { get; private set; }
    public MoveTest MoveTest { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInput();
        } else {
            Destroy(gameObject);
        }
    }

    private void InitializeInput() {
        MoveTest = new MoveTest();
        MoveTest.Enable();
    }
#endif
}