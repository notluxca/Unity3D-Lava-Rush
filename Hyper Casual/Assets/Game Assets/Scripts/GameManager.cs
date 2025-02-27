using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    // [SerializeField] UIManager uIManager; // will know uiController

    [SerializeField] TMP_Text gemsText; //! outside responsability
    int currentGems;

    private void Update() {
        
    }

    
    private void OnEnable() {
        Application.targetFrameRate = 60;
        // GemCollectable.gemCollected += OnGemCollected;
        PlayerEvents.onPlayerDied += onPlayerDied;
        PlayerEvents.OnPlayerFirstMove += OnGameStart;
    }

    private void OnDisable() {
        PlayerEvents.onPlayerDied -= onPlayerDied;
        PlayerEvents.OnPlayerFirstMove += OnGameStart;
    }

    private void OnGemCollected()
    {
        currentGems += 1;
        // gemsText.text = currentGems.ToString();
    }

    void OnGameStart(Vector3 playerStartMovePosition){
        UIManager.Instance.OpenUI(GameUIs.GameplayUI);
    }

    //! Chama DERROTA depois de 2 segundos
    public void onPlayerDied(){
        StartCoroutine(StartDeathProceedure());    
    }

    public void RevivePlayer(){
        // takes current position
        // find a safe landing spot in plataforms in front
        // move player to that position
        // enable player movement
        Debug.Log("Revive called on gamemanager, warn player");
    }   

    // transform in controllable coroutine
    IEnumerator StartDeathProceedure(){
        yield return new WaitForSeconds(2);
        //todo: call death ui
        //todo: wait for choice 
        //todo: if revive call revive procedure
        UIManager.Instance.OpenPopUp(UIPopUps.DeathRevive);
        // uIManager.OpenDeathPopUp();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
