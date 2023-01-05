using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[DisallowMultipleComponent]
[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(RawImage))]
public class Video : MonoBehaviour {
    private VideoPlayer _vp;
    private RawImage _rawImg;
    private bool _isRunning;

    private void Awake() {
        _vp = GetComponent<VideoPlayer>();
        _rawImg = GetComponent<RawImage>();

        Init();
    }

    private IEnumerator Start() {
        _vp.Prepare();

        while (!_vp.isPrepared)
            yield return null;

        _rawImg.texture = _vp.texture;
        _vp.isLooping = false;
    }

    public IEnumerator SetupCo(float totalDurationSec) {
        yield break;
        while (_vp.length <= 0) {
            yield return null;
        }

        var ratio = totalDurationSec / (float)_vp.length;
        if (ratio < 1) yield break;

        print($"{_vp.playbackSpeed} :: {ratio}");

        _vp.playbackSpeed /= ratio;
    }

    public void Run() {
        if (_isRunning) return;
        _isRunning = true;

        StartCoroutine(FadeIn());
        //StartCoroutine(RunCo());
        _vp.Play();
    }

    public void Stop() {
        _isRunning = false;
        _vp.Pause();
    }

    private IEnumerator RunCo() {
        var isForward = true;

        while (_isRunning) {
            print($"playing: {(isForward ? "forward" : "reverse")}");
            _vp.Play();

            while(_vp.isPlaying) {
                yield return null;
            }

            isForward = !isForward;
            _vp.playbackSpeed = isForward ? 1 : -1;
        }
    }

    private void Init() {
        var col = _rawImg.color;
        col.a = 0f;
        _rawImg.color = col;
    }

    private IEnumerator FadeIn() {
        var t = 0f;
        var duration = 0.5f;
        var begin = _rawImg.color;
        var end = Color.white;

        while (t < duration) {
            _rawImg.color = Color.Lerp(begin, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        _rawImg.color = end;
    }
}
