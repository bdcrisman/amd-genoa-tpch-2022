using System;
using System.Collections;
using UnityEngine;

public class BarGraph : MonoBehaviour {
    public EventHandler<float> RawValueUpdated;

    private Transform _transform;
    private Vector3 _maxScale;
    private float _rawDataValue;
    private float _riseDuration;
    private bool _isRunning;
    private bool _isAMD;

    private void Awake() {
        _transform = transform;    
    }

    public void Setup(bool isAMD, float rawDataValue, float maxHeightRatio, float riseDuration) {
        _isAMD = isAMD;
        _rawDataValue = rawDataValue;
        _riseDuration = riseDuration;

        SetupMaxScale(maxHeightRatio);
        InitScale();
    }

    public void RiseOverTime() {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(RiseOverTimeCo());
    }

    private IEnumerator RiseOverTimeCo() {
        var t = 0f;
        var beginScale = _transform.localScale;

        while (t < _riseDuration) {
            var speed = t / _riseDuration;
            OnScaleUpdated(Vector3.Lerp(beginScale, _maxScale, speed));
            OnRawValueUpdated(Mathf.Lerp(0, _rawDataValue, speed));
            t += Time.deltaTime;
            yield return null;
        }

        OnScaleUpdated(_maxScale);
        OnRawValueUpdated(_rawDataValue);
    }

    private void SetupMaxScale(float maxHeightRatio) {
        _maxScale = _transform.localScale;
        _maxScale.z = _transform.localScale.z * maxHeightRatio;
    }

    private void InitScale() {
        var scale = _transform.localScale;
        scale.z = 0f;
        _transform.localScale = scale;
    }

    private void OnScaleUpdated(Vector3 scale) {
        _transform.localScale = scale;
    }

    private void OnRawValueUpdated(float val) {
        RawValueUpdated?.Invoke(this, val);
    }
}
