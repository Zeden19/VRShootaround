using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChallenge : MonoBehaviour {
    public void changeChallenge(string challenge) {
        ChallengeModeManager.SetSelectedChallenge(challenge);
    }
}
