using System;
using System.Collections.Generic;
using UnityEngine;

public class DataStreamManager : MonoBehaviour {
    [SerializeField] private List<StreamSpawner> _processorStreamSpawners;
    [SerializeField] private StreamSpawner _databaseStreamSpawner;
    [SerializeField] private StreamSpawner _scoreStreamSpawner;

    private bool _isRunning;

    private void OnEnable() {
        _processorStreamSpawners.ForEach(x => x.Finished += OnProcessorSpawnerFinished);
        _databaseStreamSpawner.Finished += OnDatabaseSpawnerFinished;
    }

    private void OnDisable() {
        _processorStreamSpawners.ForEach(x => x.Finished -= OnProcessorSpawnerFinished);
        _databaseStreamSpawner.Finished -= OnDatabaseSpawnerFinished;
    }

    private void OnDatabaseSpawnerFinished(object sender, EventArgs e) {
        _scoreStreamSpawner.Run();
    }

    private void OnProcessorSpawnerFinished(object sender, EventArgs e) {
        _databaseStreamSpawner.Run();
    }

    public void Setup(float delta) {
        _processorStreamSpawners.ForEach(x => x.Setup(delta));
        _databaseStreamSpawner.Setup(delta);
        _scoreStreamSpawner.Setup(delta);
    }

    public void Run() {
        if (_isRunning) return;
        _isRunning = true;

        _processorStreamSpawners.ForEach(x => x.Run());
    }

    public void Stop() {
        _processorStreamSpawners.ForEach(x => x.Stop());
        _databaseStreamSpawner.Stop();
        _scoreStreamSpawner.Stop();
    }
}
