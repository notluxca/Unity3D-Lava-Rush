using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text gemsText; //! outside responsibility
    private UIManager uiManager;

    private Coroutine deathCoroutine; // <-- guarda a corrotina
    public int currentSessionGems;
    public int currentSessionScore;
    public bool hasRevived = false;

    private void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        PlayerEvents.onPlayerDied += onPlayerDied;
        PlayerEvents.OnPlayerFirstMove += OnGameStart;
        SceneManager.sceneLoaded += OnSceneLoaded;
        ClearCurrentSessionStats();
    }

    private void OnDisable()
    {
        PlayerEvents.onPlayerDied -= onPlayerDied;
        PlayerEvents.OnPlayerFirstMove -= OnGameStart;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ClearCurrentSessionStats()
    {
        Debug.Log("Limpando dados da current session");
        currentSessionGems = 0;
        currentSessionScore = 0;
        hasRevived = false;
    }

    void OnGameStart(Vector3 playerStartMovePosition)
    {
        uiManager.OpenUI(GameUIs.GameplayUI);
    }

    //! Chama DERROTA depois de 2 segundos
    public void onPlayerDied()
    {
        Debug.Log("Player Died");

        if (deathCoroutine != null)
            StopCoroutine(deathCoroutine); // Se já estiver rodando, para ela antes de começar outra.

        deathCoroutine = StartCoroutine(StartDeathProcedure());
    }

    public void RevivePlayer()
    {
        if (deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine); // Para a coroutine se o player reviver
            deathCoroutine = null;
        }

        uiManager.ClosePopUp(UIPopUps.DeathRevive);
        playerController.playerMovement.RevivePlayer();

    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameEvents.SceneLoaded();
    }

    private IEnumerator StartDeathProcedure()
    {
        yield return new WaitForSeconds(2);

        if (hasRevived) // Confirma se o player já reviveu nessa sessão
            uiManager.OpenPopUp(UIPopUps.Score);
        else
            uiManager.OpenPopUp(UIPopUps.DeathRevive);

        deathCoroutine = null; // Limpa a referência depois que terminar
    }
}
