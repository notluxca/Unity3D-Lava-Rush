using UnityEngine;
using TMPro;

public class HighscoreTextListener : MonoBehaviour
{
    private TMP_Text _text;
    
    void OnEnable()
    {
        _text = GetComponent<TMP_Text>();    
        GameEvents.OnHighScoreChanged += UpdateHighscoreText;
    }

    private void OnDisable() {
        GameEvents.OnHighScoreChanged -= UpdateHighscoreText;
    }

    public void UpdateHighscoreText(int currentGems){
        // Debug.Log("Atualizando gemas");
        _text.text = currentGems.ToString();
    }
}
