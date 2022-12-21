using System.Collections;
using UnityEngine;

public class QueryObject : MonoBehaviour {
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _base;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _questionMark;

    [Header("Colors")]
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _glowColor;
    [SerializeField] private Color _questionMarkColor;

    private void OnEnable() {
        SetColors();
    }

    public void Setup(int layer, Vector3 pos) {
        gameObject.layer = layer;
        _base.gameObject.layer = layer;
        _glow.gameObject.layer = layer;
        _questionMark.gameObject.layer = layer;

        transform.position = pos;
    }

    public void OnStart() {
        StartCoroutine(FadeOut(_base));
        StartCoroutine(FadeOut(_glow));
        StartCoroutine(FadeOut(_questionMark));
    }

    public void OnFinish() {
        Destroy(gameObject);
    }

    private void SetColors() {
        _base.color = _baseColor;
        _glow.color = _glowColor;
        _questionMark.color = _questionMarkColor;
    }

    private IEnumerator FadeOut(SpriteRenderer s) {
        var t = 0f;
        var duration = 1f;
        var beginCol = s.color;
        var endCol = new Color(beginCol.r, beginCol.g, beginCol.b, 0); 

        while (t < duration) {
            s.color = Color.Lerp(beginCol, endCol, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
