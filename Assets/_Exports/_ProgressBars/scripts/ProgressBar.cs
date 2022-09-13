using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private TextMeshProUGUI _percent;

    private void Awake() {
        _fill.fillAmount = 0;
        _percent.text = "0%";
    }

    /// <summary>
    /// Sets the progress bar fill.
    /// </summary>
    /// <param name="v"></param>
    public void Set(float v) {
        _fill.fillAmount = v;
        if (_percent) _percent.text = $"{v*100:n0}%";
    }   

    /// <summary>
    /// Sets the progress bar font to bold.
    /// </summary>
    public void SetFontBold() {
        _percent.fontStyle = FontStyles.Bold;
    }
}
