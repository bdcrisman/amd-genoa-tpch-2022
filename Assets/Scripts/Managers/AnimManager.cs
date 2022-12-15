using UnityEngine;

public class AnimManager : MonoBehaviour {
    [SerializeField] private AnimPanel _compAnimPanel;
    [SerializeField] private AnimPanel _amdAnimPanel;

    public void Setup(SetupConfigModel setup, DataModel amdData, DataModel compData) {
        _amdAnimPanel.Setup(true, setup, amdData);
        _compAnimPanel.Setup(false, setup, compData);
    }

    public void RunDemo() {
        _amdAnimPanel.RunDemo();
        _compAnimPanel.RunDemo();
    }
}
