using UnityEngine;

public class AnimManager : MonoBehaviour {
    [SerializeField] private AnimPanel _amdAnimPanel;
    [SerializeField] private AnimPanel _compAnimPanel;

    public void Setup(SetupConfigModel setup, DataModel amdData, DataModel compData) {
        _amdAnimPanel.Setup(true, setup, amdData);
        _compAnimPanel.Setup(false, setup, compData);
    }

    public void RunDemo() {
        _amdAnimPanel.RunDemo();
        _compAnimPanel.RunDemo();
    }
}
