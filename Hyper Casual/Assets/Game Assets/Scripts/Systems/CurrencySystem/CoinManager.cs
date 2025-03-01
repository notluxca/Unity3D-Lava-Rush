using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static int CurrentGems { get; private set; }
    [SerializeField] TMP_Text gemsText;

    private void Start() {
        CurrentGems = PlayerPrefs.GetInt("Gems", 0);
        GameEvents.GemCountChanged(CurrentGems);
    }

    private void OnEnable() {
        GameEvents.OnGemCollected += AddCoin;
    }

    private void OnDisable() {
        GameEvents.OnGemCollected -= AddCoin;
    }

    void AddCoin(int value){
        CurrentGems += value;
        PlayerPrefs.SetInt("Gems", CurrentGems);
        GameEvents.GemCountChanged(CurrentGems);
    }

    public bool HasEnoughGems(int value) => CurrentGems >= value;
    
    public static int GetCurrentGems() => CurrentGems;
    
    public void SpendGems(int value){
        if(HasEnoughGems(value)){
            CurrentGems -= value;
            PlayerPrefs.SetInt("Gems", CurrentGems);
            GameEvents.GemCountChanged(CurrentGems);
        }
    }
}
