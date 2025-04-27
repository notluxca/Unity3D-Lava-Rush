using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    private UIManager uiManager;

    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
        OpenInitialUI();
    }

    public void OpenInitialUI() { uiManager.OpenUI(GameUIs.InitialUI); }

    public void OpenInventoryUI() { uiManager.OpenUI(GameUIs.Inventory); }

    public void OpenShopUI() { uiManager.OpenUI(GameUIs.ShopUI); }

    public void OpenSettingsUI() { uiManager.OpenUI(GameUIs.SettingsUI); }

    public void OpenCreditsPopUp() { uiManager.OpenPopUp(UIPopUps.Credits); }

    public void OpenDeathRevivePopUp() { uiManager.OpenPopUp(UIPopUps.DeathRevive); }

    public void OpenScorePopUp() { uiManager.OpenPopUp(UIPopUps.Score); }

    public void OpenUrl(string url) { Application.OpenURL(url); }









}

