using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public static int CurrentGems { get; private set; }
    public static int CurrentSessiomGems { get; private set; }



    private void Awake()
    {
        // Garante que só existe uma instância
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // DontDestroyOnLoad(gameObject); // Opcional, se quiser manter entre cenas
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        CurrentSessiomGems = 0;
    }

    private void Start()
    {
        CurrentGems = PlayerPrefs.GetInt("Gems", 0);
        GameEvents.GemCountChanged(CurrentGems);

        //! To Remove in Final Version
        SETcoins(600);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEvents.OnGemCollected += AddCoin;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.OnGemCollected -= AddCoin;
    }

    void AddCoin(int value)
    {
        CurrentGems += value;
        CurrentSessiomGems += value;

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
        Debug.Log("Trying to spend gems");
        if (!Instance.HasEnoughGems(value)) return false;

        Instance.SpendGems(value); // subtrai as gemas aqui
        return true;
    }

    private void SETcoins(int value)
    {
        CurrentGems = value;
    }
}
