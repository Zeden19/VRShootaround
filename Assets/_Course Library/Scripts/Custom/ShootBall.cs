using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootBall : MonoBehaviour {
    public BasketballHoldTracker leftHand;
    public BasketballHoldTracker rightHand;
    public UnityEvent OnShoot = new UnityEvent();
    public bool isChallenge = false;
    public Timer timer;

    private bool leftTriggerPressed = false;
    private bool rightTriggerPressed = false;
    private bool isShooting = false;
    private bool hasBall = false;

    private GameObject heldBall;
    private Rigidbody ballRigidbody;
    private TrailRenderer ballTrail;
    private XRGrabInteractable ballGrab;

    private XRDirectInteractor dominantHand;
    private XRDirectInteractor offHand;

    private Vector3 dominantHandVelocity;
    private Vector3 offHandVelocity;
    private Vector3 dominantHandAngularVelocity;
    private Vector3 offHandAngularVelocity;

    private InputDevice leftController;
    private InputDevice rightController;

    void Start() {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update() {
        leftController.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPressed);
        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerPressed);

        hasBall = leftHand.IsHoldingBasketball() || rightHand.IsHoldingBasketball();

        if (leftHand.IsHoldingBasketball()) {
            dominantHand = leftHand.GetComponent<XRDirectInteractor>();
            offHand = rightHand.GetComponent<XRDirectInteractor>();
        } else if (rightHand.IsHoldingBasketball()) {
            dominantHand = rightHand.GetComponent<XRDirectInteractor>();
            offHand = leftHand.GetComponent<XRDirectInteractor>();
        }

        if (hasBall && dominantHand != null && dominantHand.selectTarget != null) {
            GameObject newBall = dominantHand.selectTarget.gameObject;
            if (newBall != heldBall) {
                heldBall = newBall;
                ballRigidbody = heldBall.GetComponent<Rigidbody>();
                ballTrail = heldBall.GetComponent<TrailRenderer>();
                ballGrab = heldBall.GetComponent<XRGrabInteractable>();
            }
        }

        if (hasBall) {
            leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out offHandVelocity);
            rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out dominantHandVelocity);
            leftController.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out offHandAngularVelocity);
            rightController.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out dominantHandAngularVelocity);
        }

        if (!leftTriggerPressed && !rightTriggerPressed && isShooting && heldBall != null) {
            isShooting = false;
            FireShot();
        }

        if (leftTriggerPressed && rightTriggerPressed && hasBall) {
            isShooting = true;
        }
    }

    private void FireShot() {
        if (heldBall == null || ballRigidbody == null) return;

        Vector3 baseVelocity = dominantHandVelocity * 0.8f+ offHandVelocity * 0.2f;

        Transform hoopTarget = FindBasketTarget();
        Vector3 finalVelocity = baseVelocity;

        if (hoopTarget != null) {
            Vector3 hoopPosition = hoopTarget.position;
            Vector3 ballPosition = heldBall.transform.position;

            Vector3 horizontalDisplacement = new Vector3(hoopPosition.x - ballPosition.x, 0f, hoopPosition.z - ballPosition.z);
            float horizontalDistance = horizontalDisplacement.magnitude;
            float heightDifference = hoopPosition.y - ballPosition.y;

            float launchAngle = 55f * Mathf.Deg2Rad;
            float gravity = Mathf.Abs(Physics.gravity.y);

            float idealSpeedSquared = (gravity * horizontalDistance * horizontalDistance) /
                                      (2 * (heightDifference - Mathf.Tan(launchAngle) * horizontalDistance) * Mathf.Pow(Mathf.Cos(launchAngle), 2));

            float idealSpeed = idealSpeedSquared > 0f ? Mathf.Sqrt(idealSpeedSquared) : baseVelocity.magnitude;

            Vector3 direction = horizontalDisplacement.normalized * Mathf.Cos(launchAngle) + Vector3.up * Mathf.Sin(launchAngle);
            Vector3 physicsBasedVelocity = direction * idealSpeed;

            finalVelocity = Vector3.Lerp(baseVelocity, physicsBasedVelocity, 0.6f);
            finalVelocity *= 1.23f; // boost final velocity slightly


            Vector3 directionToHoop = (hoopTarget.position - heldBall.transform.position).normalized;
            Vector3 correctedDirection = Vector3.Lerp(new Vector3(finalVelocity.x, 0f, finalVelocity.z).normalized,
                                                      new Vector3(directionToHoop.x, 0f, directionToHoop.z).normalized,
                                                      0.7f);
            float horizontalSpeed = new Vector3(finalVelocity.x, 0f, finalVelocity.z).magnitude;
            finalVelocity = new Vector3(correctedDirection.x, 0f, correctedDirection.z) * horizontalSpeed + Vector3.up * finalVelocity.y;
        }

        Vector3 finalAngularVelocity = (dominantHandAngularVelocity + offHandAngularVelocity) * 0.5f;

        ballRigidbody.isKinematic = false;
        ballRigidbody.velocity = finalVelocity;
        ballRigidbody.angularVelocity = finalAngularVelocity;

        if (ballTrail != null) ballTrail.enabled = true;

        if (isChallenge && ballGrab != null) {
            ballGrab.enabled = false;
            ChallengeModeManager.balls.Remove(heldBall);
            Debug.Log(ChallengeModeManager.balls.Count);
            if (ChallengeModeManager.balls.Count == 0) {
                timer.EndTimer();
            }
        }

        heldBall = null;
        ballRigidbody = null;

        OnShoot.Invoke();
    }

    private Transform FindBasketTarget() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("BasketTarget");
        Transform closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (GameObject target in targets) {
            float distance = Vector3.Distance(heldBall.transform.position, target.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestTarget = target.transform;
            }
        }
        return closestTarget;
    }
}
