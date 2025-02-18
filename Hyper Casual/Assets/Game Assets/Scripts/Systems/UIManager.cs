using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{    
    private UIGroup currentOpenedUi;
    private UIGroup lastOpenedUi;
    private UIController uiController;

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
}

enum UiGroups 
{
  InitialView,
  GameView,
  Shop,
  Inventory,
}
