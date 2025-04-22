﻿using DA_Assets.DAI;
using System;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
    [Serializable]
    public class ButtonSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
    {
        [SerializeField] ButtonComponent buttonComponent = ButtonComponent.UnityButton;
        [FcuInspectorProperty(ComponentType.EnumField, FcuLocKey.label_button_type, FcuLocKey.tooltip_button_type)]
        public ButtonComponent ButtonComponent
        {
            get => buttonComponent;
            set
            {
                SetValue(ref buttonComponent, value);
            }
        }

        [SerializeField] UnityButtonSettings unityButtonSettings;
        [SerializeProperty(nameof(unityButtonSettings))]
        public UnityButtonSettings UnityButtonSettings => monoBeh.Link(ref unityButtonSettings);

#if DABUTTON_EXISTS
        [SerializeField] DAB_Settings dabSettings;
        [SerializeProperty(nameof(dabSettings))]
        public DAB_Settings DAB_Settings => monoBeh.Link(ref dabSettings);
#endif
    }
}