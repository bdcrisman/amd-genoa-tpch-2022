using System.Collections.Generic;
using UnityEngine;

public class Processors : SimpleAnimation {
    [SerializeField] private List<SpriteRenderer> _glowImgs;

    public override void Stop() {
        base.Stop();
        var col = _glowImgs[0].color;
        col.a = 0;
        _glowImgs.ForEach(x => x.color = col);
    }
}
