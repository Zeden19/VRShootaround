using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleport object in front of the player
/// </summary>
public class TeleportToPlayer : MonoBehaviour {

    [Tooltip("Current player's transform. Use camera")]
    public Transform player; 
    
    [Tooltip("Distance in front of the player")]
    public float offsetDistance = 1.5f;

    [Tooltip("Force y to be a specific value")]
    public float yDistance = -1;

    private Rigidbody rb;
    private bool hasRigidbody;

    public void Start() {
        rb = GetComponent<Rigidbody>();
        hasRigidbody = rb != null; // Check if the object has a Rigidbody
    }

    public void Teleport() {
        if (player != null)  {
            // Calculate the target position slightly in front of the player
            Vector3 targetPosition = player.position + player.forward * offsetDistance;
            Quaternion targetRotation = player.rotation; // Match player's rotation
            if (yDistance != -1) {
                targetPosition.y = yDistance;
            }

            if (hasRigidbody) {
                // Disable Velocity
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                
                // Teleport using Rigidbody
                rb.MovePosition(targetPosition);
            } else {
                // Directly set the Transform position
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }
        }
        else {
            Debug.LogWarning("Player Transform is not assigned!");
        }
    }
}