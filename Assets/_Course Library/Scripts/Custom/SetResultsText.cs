using TMPro;
using UnityEngine;

public class SetResultsText : MonoBehaviour {
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI scoreText;

    public void SetTitleText() {
        titleText.text = ChallengeModeManager.selectedChallenge.Replace("_", " ") + " Results:";
    }

    public void SetScoreText() {
        scoreText.text = ScoreCounter.score.ToString();
    }
}
