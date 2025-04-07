using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ChallengeModeManager : MonoBehaviour {

    private Scene scene;

    public static string selectedChallenge;

    public GameObject three_point_challenge;
    public GameObject mid_range_challenge;
    public GameObject all_shots_challenge;

    public TeleportPlayer teleportPlayer = null;

    public static List<GameObject> balls;


    private void Awake() {
        DontDestroyOnLoad(gameObject);
 
        // This makes sure it is added only once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Add the listener to be called when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Store the creating scene as the scene to trigger start
        scene = SceneManager.GetActiveScene();
    }
    public static void SetSelectedChallenge(string challenge) {
        selectedChallenge = challenge;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (!string.Equals(scene.path, this.scene.path)) return;
        if (SceneManager.GetActiveScene().name.Equals("Court_Challenge")) {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
                if (gameObject.name.Equals(selectedChallenge)) {
                    // Set the challenge gameObjects to be true
                    gameObject.SetActive(true);

                    // Teleport the player to the starting area of challenge
                    teleportPlayer.area = GameObject.Find("Start Pad").GetComponent<TeleportationArea>();
                    teleportPlayer.Teleport();

                    // get all balls -- used in ShootBall to remove
                    balls = GameObject.FindGameObjectsWithTag("Basketball").ToList();
                    
                    return;
                }
            }
        }
    }

    public string getChallenge() {
        return selectedChallenge;
    }
}
