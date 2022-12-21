using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamSpawner : MonoBehaviour {
    const float BaseStreamSpeed = 0.5f;

    public EventHandler Finished;

    [SerializeField] private List<Transform> _paths;
    [SerializeField] private GameObject _streamPrefab;
    [SerializeField] private bool _isProcessorSpawner;

    private float _streamSpeed;
    private float _trailTime;
    private bool _isRunning;
    private bool _isFinished;
    private bool _canSpawn;

    private void OnEnable() {
        _trailTime = _streamPrefab.GetComponent<TrailRenderer>().time;
    }

    public void Setup(float speedMultiplier) {
        _streamSpeed = BaseStreamSpeed / speedMultiplier;
    }

    public void Run() {
        if (_isRunning || _isFinished) return;
        _isRunning = true;
        _canSpawn = true;

        StartCoroutine(RunLoopCo());
    }

    public void Stop() {
        _isFinished = true;
        _isRunning = false;
    }

    private IEnumerator RunLoopCo() {
        while (_isRunning) {
            _canSpawn = false;

            var stream = Instantiate(_streamPrefab, transform);
            stream.layer = gameObject.layer;
            stream.transform.position = _isProcessorSpawner ? transform.position : _paths[0].position;
            TraversePaths(new Hashtable { { "stream", stream }, { "index", 0 } });

            while (!_canSpawn) {
                yield return null;
            }
        }
    }

    private void TraversePaths(Hashtable h) {
        var stream = (GameObject)h["stream"];
        var i = (int)h["index"];

        if (!_isRunning || i >= _paths.Count) {
            OnFinish(stream);
            return;
        }

        h["index"] = i + 1;

        iTween.MoveTo(stream, iTween.Hash(
            "islocal", false,
            "position", _paths[i].position,
            "time", _streamSpeed,
            "easetype", "linear",
            "oncomplete", "TraversePaths",
            "oncompleteparams", h,
            "oncompletetarget", gameObject));
    }

    private void OnFinish(GameObject go) {
        try {
            StartCoroutine(WaitToDestroyCo(go, _isRunning ? _trailTime : 0));
        } finally {
            if (_isRunning) {
                _canSpawn = true;
                Finished?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private IEnumerator WaitToDestroyCo(GameObject go, float delay) {
        yield return new WaitForSeconds(delay);
        try {
            Destroy(go);
        } catch { }
    }
}
