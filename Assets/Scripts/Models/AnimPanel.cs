using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPanel : MonoBehaviour {
    public EventHandler Finished;

    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private Transform _dataStreamParent;
    [SerializeField] private GameObject _dataStreamPrefab;
    [SerializeField] private DataStreamManager _dataStreamManager;
    [SerializeField] private Video _video;
    [SerializeField] private SimpleAnimation _processors;
    [SerializeField] private SimpleAnimation _database;

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

    public void Setup(bool isAmd, SetupConfigModel setup) {
        _isAMD = isAmd;
        _totalDurationSec = setup.DurationSec;
        _finalScore = _isAMD ? setup.AmdFinalDataValue : setup.CompFinalDataValue;
        _scorePanel.Setup(setup.ScoreLabel);

        var multiplier = _isAMD ? setup.AmdFinalDataValue / setup.CompFinalDataValue : 1f;
        _database.Setup(multiplier);
        _processors.Setup(multiplier);
        _dataStreamManager.Setup(multiplier);

        StartCoroutine(_video.SetupCo(setup.DurationSec));
    }

    public void RunDemo() {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(SetupDemoAndWaitForActivationsThenRunDemoCo());
    }

    private void OnScoreFinished(object sender, EventArgs e) {
        _dataStreamManager.Stop();
        _processors.Stop();
        _database.Stop();
        _video.Stop();

        Finished?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator SetupDemoAndWaitForActivationsThenRunDemoCo() {
        _processors.SetupDemo();
        _database.SetupDemo();

        while (!_processors.IsActivationComplete || !_database.IsActivationComplete) {
            yield return null;
        }

        yield return StartCoroutine(_dataStreamManager.SetupDemoCo());

        _video.Run();
        _dataStreamManager.Run();
        _scorePanel.Run(_totalDurationSec, _finalScore);

        _processors.RunAction();
        _database.RunAction();
    }
}
