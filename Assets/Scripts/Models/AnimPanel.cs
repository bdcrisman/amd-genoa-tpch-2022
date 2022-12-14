using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPanel : MonoBehaviour {
    private DataModel _data;
    private bool _isAmd;
    private bool _isRunning;

    public void Setup(bool isAmd, DataModel data) {
        _isAmd = isAmd;
        _data = data;
    }

    public void RunDemo() {
        if (_isRunning) return;
        _isRunning = true;
        StartCoroutine(RunDemoCo());
    }

    private IEnumerator RunDemoCo() {
        yield return null;
    }
}
