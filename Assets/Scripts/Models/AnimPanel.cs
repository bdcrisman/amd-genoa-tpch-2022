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
    [SerializeField] private GameObject _dataStreamPrefab;

    [SerializeField] private DataStreamManager _dataStreamManager;

    private Dictionary<BarGraph, float> _graphsMap = new();
    private List<BarGraph> _graphs;
    private DataModel _data;
    private float _totalDurationSec;
    private float _finalScore;
    private bool _isAMD;
    private bool _isRunning;

    public void Setup(bool isAmd, SetupConfigModel setup, DataModel data) {
        _isAMD = isAmd;
        _data = data;
        _totalDurationSec = setup.DurationSec;
        _finalScore = isAmd ? setup.AmdFinalDataValue : setup.CompFinalDataValue;

        _dataStreamManager.Setup(_isAMD ? 1 : setup.AmdFinalDataValue / setup.CompFinalDataValue);
        _scorePanel.Setup(setup.ScoreLabel);
        //InitGraphs(setup.AmdFinalDataValue / setup.CompFinalDataValue);
    }

    public void RunDemo() {
        if (_isRunning) return;
        _isRunning = true;

        //foreach (var g in _graphs) {
        //    g.RiseOverTime();
        //}

        _dataStreamManager.Run();
        _scorePanel.Run(_totalDurationSec, _finalScore);
    }

    private void InitGraphs(float delta) {
        _graphs = _graphsParent.GetComponentsInChildren<BarGraph>().ToList();

        var maxDuration = _data.Durations.Max();
        var maxQPH = _data.QueriesPerHour.Max();
        var min = Mathf.Min(_data.Durations.Count, _graphs.Count);

        for (var i = 0; i < min; ++i) {
            var q = _data.QueriesPerHour[i];
            var t = _data.Durations[i];
            var g = _graphs[i];
            g.RawValueUpdated += (s, e) => OnGraphRawValueUpdated(g, e);
            g.Setup(_isAMD, q, q / maxQPH, t / maxDuration * _totalDurationSec, delta, _dataStreamPrefab, _dataStreamParent, _dataStreamDest.position);
        }
    }

    private void OnGraphRawValueUpdated(BarGraph g, float val) {
        return;

        _graphsMap[g] = val;
        _scorePanel.UpdateScore(_graphsMap.Sum(x => x.Value));
    }
}
