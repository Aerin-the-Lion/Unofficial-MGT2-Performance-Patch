using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;
using System.Diagnostics;
using UnofficialMGT2PerformancePatch.Config;

namespace UnofficialMGT2PerformancePatch
{
    public partial class Hooks
    {
        [HarmonyPatch(typeof(Menu_DevGame), "Init_GameplayFeatures")]
        public class OptimizeInitGameplayFeatures
        {
            static bool Prefix(Menu_DevGame __instance, gameplayFeatures ___gF_, string ___searchStringA,
                mainScript ___mS_, textScript ___tS_, sfxScript ___sfx_, GUI_Main ___guiMain_)
            {
                if(!ConfigManager.IsModEnabled.Value || !ConfigManager.IsGameDevMenuInitOptimizationEnabled.Value)
                {
                    return true;
                }
                //-------------------------------------------------------------------------------------------------

                Traverse.Create(__instance).Field("FindScripts").GetValue();
                if (__instance.g_GameGameplayFeatures.Length == 0)
                {
                    __instance.g_GameGameplayFeatures = new bool[___gF_.gameplayFeatures_LEVEL.Length];
                }
                for (int i = 0; i < __instance.uiObjects[120].transform.childCount; i++)
                {
                    UnityEngine.Object.Destroy(__instance.uiObjects[120].transform.GetChild(i).gameObject);
                }
                for (int j = 0; j < ___gF_.gameplayFeatures_LEVEL.Length; j++)
                {
                    if (___gF_.IsErforscht(j))
                    {
                        string text = ___gF_.GetName(j);
                        ___searchStringA = ___searchStringA.ToLower();
                        text = text.ToLower();
                        if (__instance.uiObjects[204].GetComponent<InputField>().text.Length <= 0 || text.Contains(___searchStringA))
                        {
                            Item_DevGame_GameplayFeature component =
                                UnityEngine.Object.Instantiate<GameObject>(__instance.uiPrefabs[0],
                                new Vector3(0f, 0f, 0f), Quaternion.identity, __instance.uiObjects[120].transform)
                                .GetComponent<Item_DevGame_GameplayFeature>();

                            component.myID = j;
                            component.mS_ = ___mS_;
                            component.tS_ = ___tS_;
                            component.sfx_ = ___sfx_;
                            component.guiMain_ = ___guiMain_;
                            component.gF_ = ___gF_;
                            component.menuDevGame_ = __instance;
                            BUTTON_Click_Custom(component);
                            BUTTON_Click_Custom(component);
                        }
                    }
                }
                __instance.DROPDOWN_SortGameplayFeatures();
                ___guiMain_.KeinEintrag(__instance.uiObjects[120], __instance.uiObjects[121]);
                //一回だけ計算させる
                Traverse.Create(__instance).Method("CalcDevCosts").GetValue();

                return false;
            }

            static void BUTTON_Click_Custom(Item_DevGame_GameplayFeature component)
            {
                Traverse traverse = Traverse.Create(component);
                traverse.Method("FindScripts").GetValue();
                traverse.Method("SetPlattformLock").GetValue();

                Button myButton = traverse.Field("myButton").GetValue<Button>();
                if (!myButton.interactable)
                {
                    component.GetComponent<Image>().color = Color.white;
                    //component.menuDevGame_.DisableGameplayFeature(component.myID);
                    DisableGameplayFeature_Custom(component.myID, component.menuDevGame_);
                    return;
                }
                component.sfx_.PlaySound(3, false);
                //if (component.menuDevGame_.SetGameplayFeature(component.myID))
                if (SetGameplayFeature_Custom(component.myID, component.menuDevGame_))
                {
                    component.GetComponent<Image>().color = component.guiMain_.colors[4];
                    return;
                }
                component.GetComponent<Image>().color = Color.white;
            }

            static public bool DisableGameplayFeature_Custom(int i, Menu_DevGame menu)
            {
                menu.g_GameGameplayFeatures[i] = false;
                //menu.CalcDevCosts();                  //今まで、何百回もやってた処理をなくす。
                menu.GetGesamtDevPoints();
                Traverse.Create(menu).Method("UpdateGesamtGameplayFeatures").GetValue();
                return menu.g_GameGameplayFeatures[i];
            }

            static public bool SetGameplayFeature_Custom(int i, Menu_DevGame menu)
            {
                gameplayFeatures gF_ = Traverse.Create(menu).Field("gF_").GetValue<gameplayFeatures>();
                menu.g_GameGameplayFeatures[i] = !menu.g_GameGameplayFeatures[i];
                if (menu.uiObjects[146].GetComponent<Dropdown>().value == 4 && gF_.gameplayFeatures_LOCKPLATFORM[i, 4])
                {
                    menu.g_GameGameplayFeatures[i] = false;
                }
                if (menu.uiObjects[146].GetComponent<Dropdown>().value == 5 && gF_.gameplayFeatures_LOCKPLATFORM[i, 3])
                {
                    menu.g_GameGameplayFeatures[i] = false;
                }
                //menu.CalcDevCosts();                  //今まで、何百回もやってた処理をなくす。
                menu.GetGesamtDevPoints();
                Traverse.Create(menu).Method("UpdateGesamtGameplayFeatures").GetValue();
                return menu.g_GameGameplayFeatures[i];
            }
        }
    }
}
