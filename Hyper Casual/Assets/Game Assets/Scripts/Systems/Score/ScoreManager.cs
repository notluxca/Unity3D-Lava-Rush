using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Referências para UI (opcional)
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text MenuhighScoreText;

    private static int currentScore = 0;  // Pontuação atual
    private static int highScore = 0;    // Melhor pontuação salva

    private const string HIGH_SCORE_KEY = "HighScore"; // Chave para o PlayerPrefs

    private void OnEnable()
    {
        // Carrega o highscore salvo
        LoadHighScore();

        // Atualiza o texto da UI (se houver)
        GameEvents.HighScoreChanged(highScore);
        Platform.OnPlatformJump += AddScoreNoArgument;
        
    }

    private void OnDisable() {
        Platform.OnPlatformJump -= AddScoreNoArgument;
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
        
    }


    // Add score up whenever a plataform jumped event is fired
    private void AddScoreNoArgument()
    {
        currentScore += 1;

        // Verifica se a pontuação atual supera o highscore
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            
        }
        GameEvents.ScoreChanged(currentScore);
        // Atualiza o texto da UI (se houver)
        
    }

    
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save();
        GameEvents.HighScoreChanged(highScore); 
    }



    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);


    }

    public static int GetCurrentScore()
    {
        return currentScore;
    }


    public static int GetHighScore()
    {
        return highScore;
    }
}
