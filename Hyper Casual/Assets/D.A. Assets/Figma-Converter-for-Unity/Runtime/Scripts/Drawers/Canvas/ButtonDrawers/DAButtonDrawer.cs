﻿#if DABUTTON_EXISTS
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DA_Assets.DAB;
using System.Text.RegularExpressions;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    [Serializable]
    public class DAButtonDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
    {
        public void SetupDAButton(SyncData btnSyncData)
        {
            DAButton daButton = btnSyncData.GameObject.GetComponent<DAButton>();

            SyncHelper[] btnChilds = btnSyncData.GameObject
                .GetComponentsInChildren<SyncHelper>(true)
                .Where(x => x != null && x.ContainsCustomButtonTags())
                .ToArray();

            daButton.TargetGraphics.Clear();

            bool allSprites = btnChilds.Where(x => x.ContainsTag(FcuTag.Image)).Count(x => x.Data.IsSprite()) > 1;

            var groups = btnChilds.GroupBy(helper => ExtractGroupKey(helper.gameObject.name));

            foreach (var group in groups)
            {
                SyncHelper mainObject = group.FirstOrDefault(helper => helper.ContainsTag(FcuTag.BtnDefault));

                if (mainObject == null)
                {
                    Debug.LogError($"Object with tag '{nameof(FcuTag.BtnDefault)}' was not found for {btnSyncData.GameObject.name}.");
                    continue;
                }

                if (mainObject.TryGetComponentSafe(out Graphic mainGraphic))
                {
                    if (!daButton.TargetGraphics.Contains(mainGraphic))
                        daButton.TargetGraphics.Add(mainGraphic);
                }

                List<SyncHelper> stateHelpers = group.Where(h => h != mainObject).ToList();

                if (allSprites)
                {
                    mainObject.gameObject.TryAddComponent(out SpriteAnimator spriteAnimator);

                    AnimatedProperty<Sprite> spriteProps = new AnimatedProperty<Sprite>();

                    foreach (var helper in stateHelpers)
                    {
                        if (helper.TryGetComponentSafe(out Image image))
                        {
                            if (helper.ContainsTag(FcuTag.BtnPressed))
                                spriteProps.Pressed = image.sprite;
                            if (helper.ContainsTag(FcuTag.BtnHover))
                                spriteProps.Highlighted = image.sprite;
                            if (helper.ContainsTag(FcuTag.BtnDisabled))
                                spriteProps.Disabled = image.sprite;
                        }
                        helper.gameObject.Destroy();
                    }

                    spriteAnimator.Properties = spriteProps;
                    spriteAnimator.Animations = monoBeh.Settings.ButtonSettings.DAB_Settings.SpriteAnimations;
                }
                else
                {
                    mainObject.gameObject.TryAddComponent(out ColorAnimator colorAnimator);

                    AnimatedProperty<Color> colorProps = new AnimatedProperty<Color>();

                    colorProps.Looped = DabConfig.Instance.DefaultColorProps.Looped;

                    foreach (var helper in stateHelpers)
                    {
                        if (helper.TryGetComponentSafe(out Graphic g))
                        {
                            if (helper.ContainsTag(FcuTag.BtnPressed))
                                colorProps.Pressed = g.color;
                            if (helper.ContainsTag(FcuTag.BtnHover))
                                colorProps.Highlighted = g.color;
                            if (helper.ContainsTag(FcuTag.BtnDisabled))
                                colorProps.Disabled = g.color;
                        }
                        helper.gameObject.Destroy();
                    }

                    colorAnimator.Properties = colorProps;
                    colorAnimator.Animations = monoBeh.Settings.ButtonSettings.DAB_Settings.ColorAnimations;
                }

                if (monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Pressed.Enabled ||
                    monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Highlighted.Enabled ||
                    monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Disabled.Enabled ||
                    monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations.Looped.Enabled)
                {
                    mainObject.gameObject.TryAddComponent(out ScaleAnimator scaleAnimator);
                    scaleAnimator.Animations = monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleAnimations;
                    scaleAnimator.Properties = monoBeh.Settings.ButtonSettings.DAB_Settings.ScaleProperties;
                }
            }
        }

        private string ExtractGroupKey(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "no-digit";

            // Беремо все, що до першого дефіса
            string firstPart = fullName.Split('-')[0].Trim();

            // Шукаємо першу послідовність цифр
            var match = System.Text.RegularExpressions.Regex.Match(firstPart, @"\d+");
            if (match.Success)
            {
                // Якщо знайдено цифри, повертаємо їх як ключ групи
                return match.Value;
            }
            else
            {
                // Якщо немає цифр, групуємо все сюди
                return "no-digit";
            }
        }

    }
}
#endif