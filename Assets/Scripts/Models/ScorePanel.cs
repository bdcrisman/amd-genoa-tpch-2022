using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour {
    public EventHandler Finished;

    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _value;

    private bool _isRunning;

    public void Setup(string label) {
        _label.text = label;
        _value.text = "---";
    }

    public void UpdateScore(float score) {
        _value.text = $"{score:n2}";
    }

    public void Run(float duration, float finalScore) {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(IncrementOverTimeCo(duration, finalScore));
    }

    private IEnumerator IncrementOverTimeCo(float duration, float finalScore) {
        var t = 0f;
        while (t < duration) {
            UpdateScore(Mathf.Lerp(0, finalScore, t / duration));
            t += Time.deltaTime;
            yield return null;
        }

        UpdateScore(finalScore);
        OnFinished();
    }

    private void OnFinished() {
        Finished?.Invoke(this, EventArgs.Empty);
    }
}
