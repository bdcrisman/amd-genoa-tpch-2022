using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleAnimation : MonoBehaviour {
    public bool IsActivationComplete { get; private set; }

    private Animator _anim;
    private float _multiplier;

    private void Awake() {
        _anim = GetComponent<Animator>();
    }

    public virtual void Setup(float multiplier) {
        _multiplier = multiplier;
    }

    public virtual void SetupDemo() {
        _anim.Play("Activate");
    }

    public virtual void RunAction() {
        _anim.speed *= _multiplier > 0 ? _multiplier : 1f;
        _anim.Play("Action");
    }

    public virtual void Stop() {
        _anim.enabled = false;
    }

    public void OnActivationComplete() {
        IsActivationComplete = true;
    }
}
