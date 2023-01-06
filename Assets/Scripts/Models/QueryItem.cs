using System.Collections;
using UnityEngine;

public enum QueryType {
    PricingSummary,
    LowestCostSupplier,
    TransportationPriority,
    ReturnReport
}

public struct QueryItemColors {
    public Color MainBackgroundColor;
    public Color BaseColor;
    public Color GlowColor;
    public Color QuestionMarkColor;
}

[DisallowMultipleComponent]
public class QueryItem : MonoBehaviour {
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _mainBackground;
    [SerializeField] private SpriteRenderer _border;
    [SerializeField] private SpriteRenderer _base;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _questionMark;

    [Header("Query Type Containers")]
    [SerializeField] private SpriteRenderer _pricingSummary;
    [SerializeField] private SpriteRenderer _lowestCostSupplier;
    [SerializeField] private SpriteRenderer _transportationPriority;
    [SerializeField] private SpriteRenderer _returnReport;

    private SpriteRenderer _currentItem;

    public void Setup(int layer, Vector3 pos, QueryType qt, QueryItemColors colors) {
        // set layers
        gameObject.layer = layer;

        _mainBackground.gameObject.layer = layer;
        _border.gameObject.layer = layer;
        _base.gameObject.layer = layer;
        _glow.gameObject.layer = layer;
        _questionMark.gameObject.layer = layer;
        _pricingSummary.gameObject.layer = layer;
        _lowestCostSupplier.gameObject.layer = layer;
        _transportationPriority.gameObject.layer = layer;
        _returnReport.gameObject.layer = layer;

        _pricingSummary.gameObject.layer = layer;
        _lowestCostSupplier.gameObject.layer = layer;
        _transportationPriority.gameObject.layer = layer;
        _returnReport.gameObject.layer = layer;

        // set position
        transform.position = pos;

        // set colors
        _mainBackground.color = colors.MainBackgroundColor;
        _base.color = colors.BaseColor;
        _glow.color = colors.GlowColor;
        _questionMark.color = colors.QuestionMarkColor;

        // set activity
        _pricingSummary.gameObject.SetActive(qt == QueryType.PricingSummary);
        _lowestCostSupplier.gameObject.SetActive(qt == QueryType.LowestCostSupplier);
        _transportationPriority.gameObject.SetActive(qt == QueryType.TransportationPriority);
        _returnReport.gameObject.SetActive(qt == QueryType.ReturnReport);

        // set current item
        _currentItem = qt switch {
            QueryType.PricingSummary => _pricingSummary, //.transform.GetChild(0).GetComponent<SpriteRenderer>(),
            QueryType.LowestCostSupplier=> _lowestCostSupplier, //.transform.GetChild(0).GetComponent<SpriteRenderer>(),
            QueryType.TransportationPriority => _transportationPriority, //.transform.GetChild(0).GetComponent<SpriteRenderer>(),
            QueryType.ReturnReport => _returnReport, //.transform.GetChild(0).GetComponent<SpriteRenderer>(),
            _ => _pricingSummary, //.transform.GetChild(0).GetComponent<SpriteRenderer>()
        };

        _currentItem.gameObject.layer = layer;
    }

    public void OnStart() {
        StartCoroutine(FadeOut(_mainBackground));
        StartCoroutine(FadeOut(_border));
        StartCoroutine(FadeOut(_base));
        StartCoroutine(FadeOut(_glow));
        StartCoroutine(FadeOut(_questionMark));
        StartCoroutine(FadeOut(_currentItem));
    }

    public void OnFinish() {
        try {
            Destroy(gameObject);
        } catch { }
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
