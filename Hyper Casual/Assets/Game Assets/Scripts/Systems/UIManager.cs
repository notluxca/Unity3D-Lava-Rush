using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{    
    private UIGroup currentOpenedUi;
    private UIGroup lastOpenedUi;
    private UIController uiController;
    public CoinManager coinManager;
    public GameManager gameManager;
    public TransitionScreen transitionScreen;

    [SerializeField] private GameObject deathRevivePopUp;
    [SerializeField] private GameObject deathPopUp;
    [SerializeField] private GameObject buyPopUp;
    [SerializeField] private GameObject pausePopUp;

    private void Awake() {
        uiController = GetComponent<UIController>();
    }

    public void CloseButton(){
        uiController.Switch_UI_group_On(0);
    }

    public void OpenShop(){
        uiController.Switch_UI_group_On(4);
    }

    public void OpenInventory(){
        uiController.Switch_UI_group_On(5);
    }

    public void OpenCredits(){
        uiController.Switch_UI_group_On(8);
    }
    public void OpenConfig(){
        uiController.Switch_UI_group_On(7);
    }
    public void OpenPause(){
        uiController.Switch_UI_group_On(6);
    }

    public void OpenDeathPopUp(){
        if(coinManager.HasEnoughGems(100)){
            deathRevivePopUp.SetActive(true);
        } else OpenDeathScreen();
    }
    public void OpenDeathScreen(){
        deathRevivePopUp.SetActive(false);
        deathPopUp.SetActive(true);
    }


    public void ReviveButton(){
        if(coinManager.HasEnoughGems(100)) {
            coinManager.SpendGems(100);
            gameManager.RevivePlayer();
        } else {
            Debug.Log("Not enough gems");
            OpenDeathScreen();
        }
    }

    public void RestartGameButton(){
        // call gamemanager
        transitionScreen.FadeIn();
        Invoke("RestartGame", 1.5f);
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

enum UiGroups 
{
  InitialView,
  GameView,
  Shop,
  Inventory,
}
