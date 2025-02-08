using UnityEngine;
using TMPro;

public class GemsListenerText : MonoBehaviour
{
    private TMP_Text gemsText;
    void OnEnable()
    {
        gemsText = GetComponent<TMP_Text>();    
        GameEvents.OnGemCountChanged += UpdateGemsText;
    }

    private void OnDisable() {
        GameEvents.OnGemCountChanged -= UpdateGemsText;
    }

    public void UpdateGemsText(int currentGems){
        Debug.Log("Atualizando gemas");
        gemsText.text = currentGems.ToString();
    }
}
