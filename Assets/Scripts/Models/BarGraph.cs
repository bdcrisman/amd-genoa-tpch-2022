using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class BarGraph : MonoBehaviour {
    public EventHandler<float> RawValueUpdated;
    public EventHandler Finished;

    public bool IsRunning { get => _isRunning; }

    [SerializeField] private bool _canPrint;

    const float _minY = -4.88f;

    private DataStream _dataStream;
    private Transform _transform;
    private Vector3 _dataStreamDest;
    private Vector3 _maxScale;
    private Vector3 _maxPos;
    private float _rawDataValue;
    private float _riseDuration;
    private bool _isRunning;
    private bool _isAMD;

    private void Awake() {
        _transform = transform;
        _dataStream = GetComponent<DataStream>();
    }

    public Vector3 GetTopWorldPos() {
        return _transform.GetChild(0).position;
    }

    public void Setup(bool isAMD, float rawDataValue, float maxHeightRatio, float riseDuration, GameObject dataStreamPrefab, Transform dataStreamParent, Vector3 dataStreamDest) {
        _isAMD = isAMD;
        _rawDataValue = rawDataValue;
        _riseDuration = riseDuration;
        _dataStream.Setup(dataStreamPrefab, dataStreamParent, dataStreamDest);

        SetupMaxYPos(maxHeightRatio);
        InitPos();
        //SetupMaxScale(maxHeightRatio);
        //InitScale();
    }

    public void RiseOverTime() {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(RiseOverTimeCo());
        StartCoroutine(RunDataStreamsCo());
    }

    private IEnumerator RunDataStreamsCo() {
        var wait = new WaitForSeconds(_isAMD ? 0.25f : 0.75f);
        while (_isRunning) {
            _dataStream.CreateDataStream(_transform.GetChild(0).position);
            yield return wait;
        }
    }

    private IEnumerator RiseOverTimeCo() {
        var t = 0f;
        var beginPos = _transform.localPosition;
        var beginScale = _transform.localScale;

        while (t < _riseDuration) {
            var speed = t / _riseDuration;
            OnPositionUpdated(Vector3.Lerp(beginPos, _maxPos, speed));
            //OnScaleUpdated(Vector3.Lerp(beginScale, _maxScale, speed));
            OnRawValueUpdated(Mathf.Lerp(0, _rawDataValue, speed));
            t += Time.deltaTime;
            yield return null;
        }

        //OnScaleUpdated(_maxScale);
        OnPositionUpdated(_maxPos);
        OnRawValueUpdated(_rawDataValue);

        Finished?.Invoke(this, EventArgs.Empty);
        _isRunning = false;
    }

    private void SetupMaxYPos(float maxHeightRatio) {
        _maxPos = _transform.localPosition;
        _maxPos.y = _minY + Mathf.Abs(_minY * maxHeightRatio);
        //print($"{_minY} * {maxHeightRatio} - {_minY} = {(_minY * maxHeightRatio) - _minY}");
    }

    private void SetupMaxScale(float maxHeightRatio) {
        _maxScale = _transform.localScale;
        _maxScale.z = _transform.localScale.z * maxHeightRatio;
    }

    private void InitPos() {
        var pos = _transform.localPosition;
        pos.y = _minY;
        _transform.localPosition = pos;
    }

    private void InitScale() {
        var scale = _transform.localScale;
        scale.z = 0f;
        _transform.localScale = scale;
    }

    private void OnPositionUpdated(Vector3 pos) {
        _transform.localPosition = pos;
    }

    private void OnScaleUpdated(Vector3 scale) {
        _transform.localScale = scale;
    }

    private void OnRawValueUpdated(float val) {
        RawValueUpdated?.Invoke(this, val);
    }
}
