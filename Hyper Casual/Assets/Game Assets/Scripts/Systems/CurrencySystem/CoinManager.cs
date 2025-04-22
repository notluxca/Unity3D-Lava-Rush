using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public static int CurrentGems { get; private set; }

    private void Awake()
    {
        // Garante que só existe uma instância
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional, se quiser manter entre cenas
    }

    private void Start()
    {
        CurrentGems = PlayerPrefs.GetInt("Gems", 0);
        GameEvents.GemCountChanged(CurrentGems);
    }

    private void OnEnable()
    {
        GameEvents.OnGemCollected += AddCoin;
    }

    private void OnDisable()
    {
        GameEvents.OnGemCollected -= AddCoin;
    }

    void AddCoin(int value)
    {
        CurrentGems += value;
        PlayerPrefs.SetInt("Gems", CurrentGems);
        GameEvents.GemCountChanged(CurrentGems);
    }

    public bool HasEnoughGems(int value) => CurrentGems >= value;

    public void SpendGems(int value)
    {
        if (HasEnoughGems(value))
        {
            CurrentGems -= value;
            PlayerPrefs.SetInt("Gems", CurrentGems);
            GameEvents.GemCountChanged(CurrentGems);
        }
    }

    public static bool TrySpendGems(int value)
    {
        if (!Instance.HasEnoughGems(value)) return false;

        Instance.SpendGems(value); // subtrai as gemas aqui
        return true;
    }
}
