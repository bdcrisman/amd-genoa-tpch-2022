using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamSpawner : MonoBehaviour {
    public EventHandler Finished;

    [SerializeField] private bool _isProcessorSpawner;
    [SerializeField] private List<Transform> _paths;
    [SerializeField] private List<float> _pathTimes;
    [SerializeField] private GameObject _streamPrefab;

    private bool _isRunning;
    private bool _canSpawn;

    public void Setup(float speedMultiplier) {
        _pathTimes.ForEach(x => x *= speedMultiplier);
    }

    public void Run() {
        if (_isRunning) return;
        _isRunning = true;
        _canSpawn = true;

        StartCoroutine(RunLoopCo());
    }

    public void Stop() {
        _isRunning = false;
    }

    private IEnumerator RunLoopCo() {
        while (_isRunning) {
            _canSpawn = false;

            var stream = Instantiate(_streamPrefab, transform);
            stream.layer = gameObject.layer;
            TraversePaths(new Hashtable { { "stream", stream }, { "index", 0 } });

            while(!_canSpawn) {
                yield return null;
            }
        }
    }

    private void TraversePaths(Hashtable h) {
        var stream = (GameObject)h["stream"];
        var i = (int)h["index"];

        if (!_isRunning || i >= _paths.Count || i >= _pathTimes.Count) {
            OnFinish(stream);
            return;
        }

        h["index"] = i + 1;

        iTween.MoveTo(stream, iTween.Hash(
            "islocal", true,
            "position", _paths[i],
            "time", _pathTimes[i],
            "oncomplete", "TraversePaths",
            "oncompleteparams", h,
            "oncompletetarget", gameObject));
    }

    private void OnFinish(GameObject go) {
        try {
            Destroy(go);
            _canSpawn = true;

            if (_isProcessorSpawner) {
                Finished?.Invoke(this, EventArgs.Empty);
            }
        } catch { }
    }
}
