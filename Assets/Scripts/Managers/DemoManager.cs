using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour {
    private bool _canRun;
    private bool _isRunning;

    [SerializeField] private ConfigManager _configManager;

    private void OnEnable() {
        _configManager.AllLoaded += OnAllConfigLoaded;
    }

    private void OnDisable() {
        _configManager.AllLoaded -= OnAllConfigLoaded;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.F5)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        if (_canRun && !_isRunning && Input.GetKeyUp(KeyCode.Space) ) {
            _isRunning = true;
            RunDemo();
        }
    }

    private void RunDemo() {

    }

    private void OnAllConfigLoaded(object sender, EventArgs e) {
        SetupDemo();
        _canRun = true;
    }

    private void SetupDemo() {
        print("totes");
    }
}
