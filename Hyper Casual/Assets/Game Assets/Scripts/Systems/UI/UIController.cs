using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public void OpenInitialUI() { UIManager.Instance.OpenUI(GameUIs.InitialUI); }  

    public void OpenInventoryUI() { UIManager.Instance.OpenUI(GameUIs.Inventory); }

    public void OpenShopUI() { UIManager.Instance.OpenUI(GameUIs.ShopUI); }

    public void OpenSettingsUI() { UIManager.Instance.OpenUI(GameUIs.SettingsUI); }

    public void OpenCreditsPopUp() { UIManager.Instance.OpenPopUp(UIPopUps.Credits); }

    public void OpenDeathRevivePopUp() { UIManager.Instance.OpenPopUp(UIPopUps.DeathRevive); }

    public void OpenScorePopUp() { UIManager.Instance.OpenPopUp(UIPopUps.Score); }

    public void OpenUrl(string url) { Application.OpenURL(url); }

    
    






}

