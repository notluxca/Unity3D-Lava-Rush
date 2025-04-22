using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // [SerializeField] UIManager uIManager; // will know uiController
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text gemsText; //! outside responsability
    int currentGems;

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        PlayerEvents.onPlayerDied += onPlayerDied;
        PlayerEvents.OnPlayerFirstMove += OnGameStart;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        PlayerEvents.onPlayerDied -= onPlayerDied;
        PlayerEvents.OnPlayerFirstMove -= OnGameStart;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnGemCollected()
    {
        currentGems += 1;
    }

    void OnGameStart(Vector3 playerStartMovePosition)
    {
        UIManager.Instance.OpenUI(GameUIs.GameplayUI);
        // Debug.Log("Tried starting game by event");
    }

    //! Chama DERROTA depois de 2 segundos
    public void onPlayerDied()
    {
        StartCoroutine(StartDeathProceedure());
    }


    public void RevivePlayer()
    {
        UIManager.Instance.ClosePopUp(UIPopUps.DeathRevive);

        playerController.playerMovement.RevivePlayer();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameEvents.SceneLoaded();
    }

    // transform in controllable coroutine
    IEnumerator StartDeathProceedure()
    {
        //todo: call death ui
        //todo: wait for choice 
        //todo: if revive call revive procedure

        yield return new WaitForSeconds(2);
        UIManager.Instance.OpenPopUp(UIPopUps.DeathRevive);
    }


}
