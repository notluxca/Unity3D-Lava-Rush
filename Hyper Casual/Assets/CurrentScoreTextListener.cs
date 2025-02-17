using UnityEngine;
using TMPro;

public class CurrentScoreTextListener : MonoBehaviour
{

    TMP_Text _text;
    
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        GameEvents.OnScoreChanged += UpdateScoreText;
    }

    private void OnDisable() {
        GameEvents.OnScoreChanged -= UpdateScoreText;
    }

    void UpdateScoreText(int currentScore)
    {
        _text.text = currentScore.ToString();
    }
}
