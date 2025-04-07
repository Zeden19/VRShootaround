using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class ScoreCounter : MonoBehaviour {
    public TextMeshProUGUI text;
    public static int score = 0;
    void Start()
    {
        text.text = score.ToString();
    }

    public void IncrementScore() {
        score += 1;
        text.text = score.ToString();
    }

    public void ResetScore() {score = 0; text.text = score.ToString();}
}
