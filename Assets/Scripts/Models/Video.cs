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
        _vp.isLooping = true;
    }

    public void  Run() {
        StartCoroutine(FadeIn());
        _vp.Play();
    }

    public void Stop() {
        _vp.Pause();
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
