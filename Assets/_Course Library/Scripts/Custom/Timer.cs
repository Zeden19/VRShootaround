using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
    private bool startTimer = false;
    private float timer;

    [Tooltip("The time the timer starts from")]
    public TMPro.TextMeshProUGUI text;
    [Tooltip("Events to run when timer reaches starts")]
    public UnityEvent onTimerStart;
    [Tooltip("Events to run when timer reaches 0")]
    public UnityEvent onTimerEnd;

    void Start() {
        text.text = "0.00";
    }

    // Update is called once per frame
    void Update() {
        if (startTimer) {timer-= Time.deltaTime;}
        if (timer <= 0.0f && startTimer) {
            EndChallenge();
        }
        text.text = timer.ToString("0.00");
    }

    public void StartTimer(float targetTime) {
        onTimerStart.Invoke();
        timer = targetTime;
        startTimer = true;
    }

    public void EndTimer() {
        EndChallenge();
    }

    private void EndChallenge() {
        timer = 0.0f;
        text.text = timer.ToString("0.00");
        startTimer = false;
        onTimerEnd.Invoke();
    }

    public float GetTime() {
        return timer;
    }
}
