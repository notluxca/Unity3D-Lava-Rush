using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{

    public int CurrentGems;
    [SerializeField] TMP_Text gemsText;

    private void Start() {
        CurrentGems = PlayerPrefs.GetInt("Gemstest", 0);
        UpdateUi();
    }

    private void OnEnable() {
        GemCollectable.gemCollected += AddCoin;
    }

    private void OnDisable() {
        GemCollectable.gemCollected -= AddCoin;
    }

    void AddCoin(){
        CurrentGems += 1;
        UpdateUi();
        PlayerPrefs.SetInt("Gems", CurrentGems);
    }

    void UpdateUi(){
        // gemsText.text = CurrentGems.ToString(); //! update this 
    }
}
