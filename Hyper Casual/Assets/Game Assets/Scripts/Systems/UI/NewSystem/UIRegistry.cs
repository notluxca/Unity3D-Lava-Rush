using System;
using UnityEngine;

[Serializable]
public class UIScreen
{
    public GameUIs type;
    public GameObject gameObject;
}

[Serializable]
public class UIPopUp
{
    public UIPopUps type;
    public GameObject gameObject;
}

public class UIRegistry : MonoBehaviour
{
    public UIScreen[] uiScreens;
    public UIPopUp[] popUps;
}
