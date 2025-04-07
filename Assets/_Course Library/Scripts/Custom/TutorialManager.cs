using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialManager : MonoBehaviour {
    public GameObject teleportBackground;
    public GameObject shootingBackground;
    public GameObject recallingBacground;

    public UnityEvent onTeleport;
    public UnityEvent onShoot;
    public UnityEvent onRecall;

    public void Teleported() {
        if (!teleportBackground.activeSelf) return;
        onTeleport.Invoke();
    }

    public void Shot() {
        if (!shootingBackground.activeSelf) return;
        onShoot.Invoke();
    }

    public void Recalled() {
        if (!recallingBacground.activeSelf) return;
        onRecall.Invoke();
    }
}
