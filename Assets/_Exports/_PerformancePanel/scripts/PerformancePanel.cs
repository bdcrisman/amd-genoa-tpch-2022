using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PerformancePanel : MonoBehaviour {
    public EventHandler Finished;

    [SerializeField] private TextMeshProUGUI _delta;
    [SerializeField] private TextMeshProUGUI _message;

    private Animator _anim;
    private bool _isRunning;

    private void Awake() {
        _anim = GetComponent<Animator>();
        _delta.text = _message.text = string.Empty;
    }

    /// <summary>
    /// Sets up the panel.
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="message"></param>
    /// <param name="units"></param>
    public void Setup(string delta, string message, string units) {
        _delta.text = $"{delta}{units}";
        _message.text = message;
    }

    /// <summary>
    /// Activates the corresponding animation.
    /// </summary>
    /// <param name="isRight"></param>
    public void Activate(bool isRight) {
        if (_isRunning) return;
        _isRunning = true;

        _anim.SetTrigger(isRight ? "right" : "left");
    }

    /// <summary>
    /// Triggered by the animation when finished.
    /// </summary>
    public void AnimFinished() {
        Finished?.Invoke(this, EventArgs.Empty);
    }
}
