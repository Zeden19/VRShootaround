using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playBounce : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip bounceClip;
    public bool isColliding = false;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
      isColliding = false;  
    }

    void OnCollisionExit(Collision collision) {
        if (isColliding) return;
        isColliding = true;
        if (collision.gameObject.CompareTag("Floor")) {
            audioSource.PlayOneShot(bounceClip);
        }
        
    }
}
