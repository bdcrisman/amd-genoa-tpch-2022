using System;
using UnityEngine;

public class AnimManager : MonoBehaviour {
    [SerializeField] private AnimPanel _compAnimPanel;
    [SerializeField] private AnimPanel _amdAnimPanel;
    [SerializeField] private SimpleAnimation _lightingAnim;

    private void OnEnable() {
        _amdAnimPanel.Finished += OnFinished;
    }

    public void Setup(SetupConfigModel setup) {
        _amdAnimPanel.Setup(true, setup);
        _compAnimPanel.Setup(false, setup);
    }

    public void RunDemo() {
        _amdAnimPanel.RunDemo();
        _compAnimPanel.RunDemo();

        _lightingAnim.RunAction();
    }

    private void OnFinished(object sender, EventArgs e) {
        _lightingAnim.Stop();
    }
}
