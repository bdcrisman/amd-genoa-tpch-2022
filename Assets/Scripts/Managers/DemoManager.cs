using System;
using UnityEngine;

public class DemoManager : MonoBehaviour {
    private bool _canRun;
    private bool _isRunning;

    [SerializeField] private ConfigManager _configManager;
    [SerializeField] private AnimManager _animManager;

    private void OnEnable() {
        _configManager.AllLoaded += OnAllConfigLoaded;
    }

    private void OnDisable() {
        _configManager.AllLoaded -= OnAllConfigLoaded;
    }

    private void Update() {
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
        _animManager.RunDemo();
    }

    private void OnAllConfigLoaded(object sender, EventArgs e) {
        SetupDemo();
        _canRun = true;
    }

    private void SetupDemo() {
        _animManager.Setup(
            _configManager.SetupConfig,
            _configManager.AmdData,
            _configManager.CompData);
    }
}
