using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{

    public int CurrentGems;
    [SerializeField] TMP_Text gemsText;

    private void Start() {
        CurrentGems = PlayerPrefs.GetInt("Gems", 0);
        GameEvents.GemCountChanged(CurrentGems);
        
    }

    private void OnEnable() {
        GameEvents.OnGemCollected += AddCoin;
        //GemCollectable.gemCollected += AddCoin;
    }

    private void OnDisable() {
        GameEvents.OnGemCollected -= AddCoin;
        // GemCollectable.gemCollected -= AddCoin;
    }

    void AddCoin(int value){
        CurrentGems += value;
        PlayerPrefs.SetInt("Gems", CurrentGems);
        GameEvents.GemCountChanged(CurrentGems);
    }

    
}
