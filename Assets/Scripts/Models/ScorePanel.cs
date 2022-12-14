using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _value;

    public void Setup(string label) {
        _label.text = label;
        _value.text = "---";
    }

    public void UpdateScore(float score) {
        _value.text = $"{score:n2}";
    }
}
