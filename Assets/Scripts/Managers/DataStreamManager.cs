using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataStreamManager : MonoBehaviour {
    [SerializeField] private Transform _leftProcessorStreamsParent;
    [SerializeField] private Transform _rightProcessorStreamsParent;
    [SerializeField] private StreamSpawner _leftDatabaseStreamSpawner;
    [SerializeField] private StreamSpawner _rightDatabaseStreamSpawner;
    [SerializeField] private StreamSpawner _leftScoreStreamSpawner;
    [SerializeField] private StreamSpawner _rightScoreStreamSpawner;

    private List<StreamSpawner> _leftProcessorStreamSpawners;
    private List<StreamSpawner> _rightProcessorStreamSpawners;
    private bool _isRunning;

    private void OnDisable() {
        _leftProcessorStreamSpawners.ForEach(x => x.Finished -= (s, e) => OnProcessorSpawnerFinished(true));
        _rightProcessorStreamSpawners.ForEach(x => x.Finished -= (s, e) => OnProcessorSpawnerFinished(false));
        
        _leftDatabaseStreamSpawner.Finished -= (s, e) => OnDatabaseSpawnerFinished(true);
        _rightDatabaseStreamSpawner.Finished -= (s, e) => OnDatabaseSpawnerFinished(false);
    }

    private void OnDatabaseSpawnerFinished(bool isLeft) {
        if (isLeft)
            _leftScoreStreamSpawner.Run();
        else
            _rightScoreStreamSpawner.Run();
    }

    private void OnProcessorSpawnerFinished(bool isLeft) {
        if (isLeft)
            _leftDatabaseStreamSpawner.Run();
        else
            _rightDatabaseStreamSpawner.Run();
    }

    public void Setup(float delta) {
        _leftProcessorStreamSpawners = _leftProcessorStreamsParent.GetComponentsInChildren<StreamSpawner>().ToList();
        _rightProcessorStreamSpawners = _rightProcessorStreamsParent.GetComponentsInChildren<StreamSpawner>().ToList();

        _leftProcessorStreamSpawners.ForEach(x => x.Finished += (s, e) => OnProcessorSpawnerFinished(true));
        _rightProcessorStreamSpawners.ForEach(x => x.Finished += (s, e) => OnProcessorSpawnerFinished(false));
        _leftDatabaseStreamSpawner.Finished += (s, e) => OnDatabaseSpawnerFinished(true);
        _rightDatabaseStreamSpawner.Finished += (s, e) => OnDatabaseSpawnerFinished(false);

        _leftProcessorStreamSpawners.ForEach(x => x.Setup(delta));
        _rightProcessorStreamSpawners.ForEach(x => x.Setup(delta));
        _leftDatabaseStreamSpawner.Setup(delta);
        _rightDatabaseStreamSpawner.Setup(delta);
        _leftScoreStreamSpawner.Setup(delta);
        _rightScoreStreamSpawner.Setup(delta);
    }

    public void Run() {
        if (_isRunning) return;
        _isRunning = true;

        _leftProcessorStreamSpawners.ForEach(x => x.Run());
        _rightProcessorStreamSpawners.ForEach(x => x.Run());
    }

    public void Stop() {
        _leftProcessorStreamSpawners.ForEach(x => x.Stop());
        _leftDatabaseStreamSpawner.Stop();
        _leftScoreStreamSpawner.Stop();

        _rightProcessorStreamSpawners.ForEach(x => x.Stop());
        _rightDatabaseStreamSpawner.Stop();
        _rightScoreStreamSpawner.Stop();
    }
}
