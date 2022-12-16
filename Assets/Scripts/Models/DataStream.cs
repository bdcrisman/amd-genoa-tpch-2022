using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class DataStream : MonoBehaviour {
    private TrailRenderer _trail;
    private GameObject _dataStreamPrefab;
    private Transform _dataStreamParent;
    private Vector3 _dataStreamDest;

    private void Awake() {
        _trail = GetComponent<TrailRenderer>();
    }

    public void Setup(GameObject dataStreamPrefab, Transform dataStreamParent, Vector3 dataStreamDest) {
        _dataStreamPrefab = dataStreamPrefab;
        _dataStreamParent = dataStreamParent;
        _dataStreamDest = dataStreamDest;
    }

    public void CreateDataStream(Vector3 src) {
        var go = Instantiate(_dataStreamPrefab, _dataStreamParent);
        go.layer = gameObject.layer;
        StartCoroutine(InterpolateDataStreamCo(go.transform, src));
    }

    private IEnumerator InterpolateDataStreamCo(Transform dataStream, Vector3 src) {
        var t = 0f;
        var duration = 0.25f;

        var center = (src + _dataStreamDest) * 0.5f;
        center -= new Vector3(0, 1, 0);

        var selSrc = src - center;
        var selDest = _dataStreamDest - center;

        while (t < duration) {
            dataStream.position = Vector3.Slerp(selSrc, selDest, t / duration);
            dataStream.position += center;
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        while (t < duration) {
            dataStream.position = Vector3.Slerp(selDest, selSrc, t / duration);
            dataStream.position += center;
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(_trail.time);
        Destroy(dataStream.gameObject);
    }
}
