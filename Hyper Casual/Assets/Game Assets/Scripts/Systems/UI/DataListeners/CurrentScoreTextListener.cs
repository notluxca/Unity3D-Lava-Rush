using UnityEngine;
using TMPro;

public class CurrentScoreTextListener : MonoBehaviour
{

    TMP_Text _text;
    
    void Start()
    {
        PlayerEvents.OnPlayerFirstMove += ResetScoreText;
    }

    private void OnEnable() {
        _text = GetComponent<TMP_Text>();
        _text.text = "0";
        GameEvents.OnScoreChanged += UpdateScoreText;
        Debug.Log(ScoreManager.GetCurrentScore());
        
    }

    private void OnDisable() {
        GameEvents.OnScoreChanged -= UpdateScoreText;
        PlayerEvents.OnPlayerFirstMove -= ResetScoreText;
    }

    void UpdateScoreText(int currentScore)
    {
        
        // Debug.Log("Updating score text");
        _text.text = currentScore.ToString();
        //! must have a way to reset
    }

    void ResetScoreText(Vector3 pos)
    {
        Debug.Log("Resetting score text");
        _text.text = "0";
    }
}
