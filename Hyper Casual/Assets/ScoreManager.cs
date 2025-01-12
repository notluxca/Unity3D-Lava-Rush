using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Referências para UI (opcional)
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    private int currentScore = 0;  // Pontuação atual
    private int highScore = 0;    // Melhor pontuação salva

    private const string HIGH_SCORE_KEY = "HighScore"; // Chave para o PlayerPrefs

    private void Start()
    {
        // Carrega o highscore salvo
        LoadHighScore();

        // Atualiza o texto da UI (se houver)
        UpdateUI();
    }

  
    public void AddScore(int points)
    {
        currentScore += points;

        // Verifica se a pontuação atual supera o highscore
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
        }

        // Atualiza o texto da UI (se houver)
        UpdateUI();
    }

    
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save();
    }


    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore}";
        }

        if (highScoreText != null)
        {
            highScoreText.text = $"HIGH SCORE: {highScore}";
        }
    }


    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);

        // Atualiza a UI (se houver)
        UpdateUI();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }


    public int GetHighScore()
    {
        return highScore;
    }
}
