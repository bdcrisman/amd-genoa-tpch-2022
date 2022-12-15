using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimPanel : MonoBehaviour {
    public EventHandler<Dictionary<BarGraph, float>> GraphUpdated;

    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private Transform _graphsParent;

    private Dictionary<BarGraph, float> _graphsMap = new();
    private List<BarGraph> _graphs;
    private DataModel _data;
    private float _totalDurationSec;
    private bool _isAMD;
    private bool _isRunning;

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
    }

    private void InitGraphs() {
        _graphs = _graphsParent.GetComponentsInChildren<BarGraph>().ToList();
        print($"{_isAMD}: {_graphs.Count}: {_data.Durations.Count}");
        
        var maxDuration = _data.Durations.Max();
        var maxQPH = _data.QueriesPerHour.Max();
        var min = Mathf.Min(_data.Durations.Count, _graphs.Count);

        for (var i = 0; i < min; ++i) {
            var q = _data.QueriesPerHour[i];
            var t = _data.Durations[i];
            var g = _graphs[i];
            g.RawValueUpdated += (s, e) => OnGraphRawValueUpdated(g, e);
            g.Setup(_isAMD, q, q / maxQPH, t / maxDuration * _totalDurationSec);
        }
    }

    private void OnGraphRawValueUpdated(BarGraph g, float val) {
        _graphsMap[g] = val;
        _scorePanel.UpdateScore(_graphsMap.Sum(x => x.Value));
    }
}
