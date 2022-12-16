using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class BarGraph : MonoBehaviour {
    const float BaseDelay = 0.5f;
    const float MinY = -4.88f;

    public EventHandler<float> RawValueUpdated;
    public EventHandler Finished;

    public bool IsRunning { get => _isRunning; }

    [SerializeField] private bool _canPrint;

    private DataStream _dataStream;
    private Transform _transform;
    private Vector3 _maxScale;
    private Vector3 _maxPos;
    private float _rawDataValue;
    private float _riseDuration;
    private float _delta;
    private bool _isRunning;
    private bool _isAMD;

    private void Awake() {
        _transform = transform;
        _dataStream = GetComponent<DataStream>();
    }

    public void Setup(bool isAMD, float rawDataValue, float maxHeightRatio, float riseDuration, float delta, GameObject dataStreamPrefab, Transform dataStreamParent, Vector3 dataStreamDest) {
        _isAMD = isAMD;
        _rawDataValue = rawDataValue;
        _riseDuration = riseDuration;
        _delta = delta;
        _dataStream.Setup(dataStreamPrefab, dataStreamParent, dataStreamDest);


        SetupMaxYPos(maxHeightRatio);
        InitPos();
    }

    public void RiseOverTime() {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(RiseOverTimeCo());
        StartCoroutine(RunDataStreamsCo());
    }

    private IEnumerator RunDataStreamsCo() {
        var wait = new WaitForSeconds(_isAMD ? BaseDelay / _delta : BaseDelay);
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
            OnRawValueUpdated(Mathf.Lerp(0, _rawDataValue, speed));
            t += Time.deltaTime;
            yield return null;
        }

        OnPositionUpdated(_maxPos);
        OnRawValueUpdated(_rawDataValue);

        Finished?.Invoke(this, EventArgs.Empty);
        _isRunning = false;
    }

    private void SetupMaxYPos(float maxHeightRatio) {
        _maxPos = _transform.localPosition;
        _maxPos.y = MinY + Mathf.Abs(MinY * maxHeightRatio);
    }

    private void InitPos() {
        var pos = _transform.localPosition;
        pos.y = MinY;
        _transform.localPosition = pos;
    }

    private void OnPositionUpdated(Vector3 pos) {
        _transform.localPosition = pos;
    }

    private void OnRawValueUpdated(float val) {
        RawValueUpdated?.Invoke(this, val);
    }
}
