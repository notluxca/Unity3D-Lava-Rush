using TMPro;
using UnityEngine;

public class DeathScreenUi : MonoBehaviour
{
    // find current score and set it
    // find how many gems were captured on that round 
    // display that

    [Header("Death Screen Elements")]
    [SerializeField] private TextMeshProUGUI scoreText, gemsText, highScoreText;
    private int currentSessionObtainedGems, currentSessionScore, highscore;



    private void OnEnable()
    {
        currentSessionObtainedGems = CoinManager.CurrentSessiomGems;
        currentSessionScore = ScoreManager.GetCurrentScore();
        highscore = ScoreManager.GetHighScore();

        UpdateAllUIElements();
    }

    private void UpdateAllUIElements()
    {
        scoreText.text = currentSessionScore.ToString();
        gemsText.text = currentSessionObtainedGems.ToString();
        highScoreText.text = highscore.ToString();
    }
}
