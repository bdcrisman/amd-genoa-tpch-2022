using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimPanel : MonoBehaviour {
    public EventHandler<Dictionary<BarGraph, float>> GraphUpdated;

    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private Transform _dataStreamParent;
    [SerializeField] private GameObject _dataStreamPrefab;
    [SerializeField] private DataStreamManager _dataStreamManager;
    [SerializeField] private Video _video;

    private Dictionary<BarGraph, float> _graphsMap = new();
    private List<BarGraph> _graphs;
    private DataModel _data;
    private float _totalDurationSec;
    private float _finalScore;
    private bool _isAMD;
    private bool _isRunning;

    private void OnEnable() {
        _scorePanel.Finished += OnScoreFinished;
    }

    private void OnDisable() {
        _scorePanel.Finished -= OnScoreFinished;
    }

    public void Setup(bool isAmd, SetupConfigModel setup, DataModel data) {
        _isAMD = isAmd;
        _data = data;
        _totalDurationSec = setup.DurationSec;
        _finalScore = isAmd ? setup.AmdFinalDataValue : setup.CompFinalDataValue;

        _dataStreamManager.Setup(_isAMD ? 1 : setup.AmdFinalDataValue / setup.CompFinalDataValue);
        _scorePanel.Setup(setup.ScoreLabel);
    }

    public void RunDemo() {
        if (_isRunning) return;
        _isRunning = true;

        _video.Run();
        _dataStreamManager.Run();
        _scorePanel.Run(_totalDurationSec, _finalScore);
    }

    private void OnScoreFinished(object sender, EventArgs e) {
        _dataStreamManager.Stop();
    }
}
