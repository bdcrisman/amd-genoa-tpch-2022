using UnityEngine;
using UnityEngine.UI;

public class Database : SimpleAnimation {
    [SerializeField] private Image _glow;

    public override void Stop() {
        base.Stop();
        var col = _glow.color;
        col.a = 0;
        _glow.color = col;
    }
}
