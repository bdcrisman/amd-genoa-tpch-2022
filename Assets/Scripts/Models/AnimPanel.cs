using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimPanel : MonoBehaviour {
    public EventHandler<float> GraphUpdated;

    [SerializeField] private Transform _graphsParent;
    
    private List<BarGraph> _graphs;
    private DataModel _data;
    private float _totalDurationSec;
    private bool _isAmd;
    private bool _isRunning;

    private void Start() {
        InitGraphs();
    }

    public void Setup(bool isAmd, DataModel data, float totalDurationSec) {
        _isAmd = isAmd;
        _data = data;
        _totalDurationSec = totalDurationSec;
    }

    public void RunDemo() {
        if (_isRunning) return;
        _isRunning = true;
     
        foreach(var g in _graphs) {
            g.RiseOverTime();
        }
    }    

    private void InitGraphs() {
        var maxDuration = _data.Durations.Max();
        var maxValue = _data.QueriesPerHour.Max();
        var min = Mathf.Min(_data.Durations.Count, _graphs.Count);

        for (var i = 0; i < min; ++i) {
            var qph = _data.QueriesPerHour[i];
            var t = _data.Durations[i];

            _graphs[i].Setup(
                qph,
                qph > 0 ? maxValue / qph : 0f,
                (t / maxDuration) * _totalDurationSec);

            _graphs[i].RawValueUpdated += OnGraphRawValueUpdated;
        }
    }

    private void OnGraphRawValueUpdated(object s, float val) {
        GraphUpdated?.Invoke(s, val);
    }
}
