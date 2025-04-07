using UnityEngine;
using UnityEngine.Events;

public class OnBucketMade : MonoBehaviour {
    [Tooltip("When bucket has been made")]
    public UnityEvent OnBucket = new UnityEvent();

    bool isColliding = false;

    private void OnTriggerExit(Collider other) {
        if(isColliding) return;
        isColliding = true;

        if (other.CompareTag("Basketball")) {
            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            
            // Ensure the ball is moving downward when it enters
            if (ballRb != null && ballRb.velocity.y < 0) {
                OnBucket.Invoke();
            }
        }
    }

    private void Update() {
      isColliding = false;  
    }
}
