using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimPanel : MonoBehaviour {
    public EventHandler<Dictionary<BarGraph, float>> GraphUpdated;

    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private Transform _graphsParent;
    [SerializeField] private Transform _dataStreamParent;
    [SerializeField] private Transform _dataStreamDest;

    private Dictionary<BarGraph, float> _graphsMap = new();
    private List<BarGraph> _graphs;
    private DataModel _data;
    private float _totalDurationSec;
    private bool _isAMD;
    private bool _isRunning;
    private int _finishedCount;

    public void Setup(bool isAmd, SetupConfigModel setup, DataModel data) {
        _isAMD = isAmd;
        _data = data;
        _totalDurationSec = setup.DurationSec;

        _scorePanel.Setup(setup.ScoreLabel);
        InitGraphs();
    }

    public void RunDemo() {
        if (_isRunning) return;
        _isRunning = true;

        foreach (var g in _graphs) {
            g.RiseOverTime();
        }

        StartCoroutine(DataStreamsCo());
    }

    private void InitGraphs() {
        _graphs = _graphsParent.GetComponentsInChildren<BarGraph>().ToList();
        
        var maxDuration = _data.Durations.Max();
        var maxQPH = _data.QueriesPerHour.Max();
        var min = Mathf.Min(_data.Durations.Count, _graphs.Count);

        for (var i = 0; i < min; ++i) {
            var q = _data.QueriesPerHour[i];
            var t = _data.Durations[i];
            var g = _graphs[i];
            g.Finished += OnGraphFinishedRising;
            g.RawValueUpdated += (s, e) => OnGraphRawValueUpdated(g, e);
            g.Setup(_isAMD, q, q / maxQPH, t / maxDuration * _totalDurationSec, _dataStreamParent);
        }
    }

    private void OnGraphFinishedRising(object sender, EventArgs e) {
        ++_finishedCount;
    }

    private void OnGraphRawValueUpdated(BarGraph g, float val) {
        _graphsMap[g] = val;
        _scorePanel.UpdateScore(_graphsMap.Sum(x => x.Value));
    }

    private IEnumerator DataStreamsCo() {


        while (_isRunning && _graphs.Count > _finishedCount) {
            foreach (var g in _graphs) {
                if (!g.IsRunning) continue;
                CreateDataStream(g.GetTopWorldPos(), _dataStreamDest.position);
                yield return new WaitForSeconds(_isAMD ? 0.2f : 0.5f);
            }

            yield return null;
        }
    }

    private void CreateDataStream(Vector3 src, Vector3 dest) {
        var go = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), _dataStreamParent);
        go.layer = gameObject.layer;
        go.transform.localScale *= 0.5f;
        StartCoroutine(InterpolateDataStreamCo(go.transform, src, dest));
    }

    private IEnumerator InterpolateDataStreamCo(Transform dataStream, Vector3 src, Vector3 dest) {
        var t = 0f;
        var duration = 0.25f;

        var center = (src + dest) * 0.5f;
        center += new Vector3(0, 1, 0);

        src += center;
        dest += center;


        while (t < duration) {
            dataStream.position = Vector3.Slerp(src, dest, t / duration);
            dataStream.position -= center;
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        while (t < duration) {
            dataStream.position = Vector3.Slerp(dest, src, t / duration);
            dataStream.position -= center;
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(dataStream.gameObject);
    }
}
