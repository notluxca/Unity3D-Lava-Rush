using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] TMP_Text highScoreTextMenu;
    [SerializeField] TMP_Text highScoreInGame;
    [SerializeField] TMP_Text currentScore;


    private void Start() {

    }

    public void StartHighScore(int score){
        highScoreTextMenu.text = "High Score: " + score;
        highScoreInGame.text = "High Score: " + score;
    }
    public void UpdateHighScore(int score){
        highScoreTextMenu.text = "High Score: " + score;
        highScoreInGame.text = "High Score: " + score;
    }

    

    
}
