using UnityEngine;
using TMPro;

public class HighscoreTextListener : MonoBehaviour
{
    private TMP_Text _text;
    
    void OnEnable()
    {
        _text = GetComponent<TMP_Text>();    
        GameEvents.OnHighScoreChanged += UpdateHighscoreText;
        _text.text = ScoreManager.GetHighScore().ToString();
    }

    private void OnDisable() {
        GameEvents.OnHighScoreChanged -= UpdateHighscoreText;
    }

    public void UpdateHighscoreText(int currentScore){
        // Debug.Log("Atualizando gemas");
        _text.text = currentScore.ToString();
    }
}
