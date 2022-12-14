using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour {
    [SerializeField] private AnimPanel _amdAnimPanel;
    [SerializeField] private AnimPanel _compAnimPanel;

    public void Setup(SetupConfigModel setup, DataModel amdData, DataModel compData) {
        _amdAnimPanel.Setup(true, amdData);
        _compAnimPanel.Setup(false, compData);
    }

    public void RunDemo() {
        _amdAnimPanel.RunDemo();
        _compAnimPanel.RunDemo();
    }
}
