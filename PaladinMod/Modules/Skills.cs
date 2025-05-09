﻿using EntityStates;
using PaladinMod.Misc;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Skills
    {
        internal static SkillDef berserkOutSkillDef;
        //internal static SkillDef berserkSlashSkillDef;
        //internal static SkillDef berserkSpinSlashSkillDef;
        internal static SkillDef berserkDashSkillDef;

        internal static SkillDef cancelSunSkillDef;

        private static SkillLocator skillLocator;

        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();

        public static void SetupSkills(GameObject bodyPrefab)
        {
            foreach (GenericSkill obj in bodyPrefab.GetComponentsInChildren<GenericSkill>())
            {
                PaladinPlugin.DestroyImmediate(obj);
            }

            skillLocator = bodyPrefab.GetComponent<SkillLocator>();

            PassiveSetup();
            PrimarySetup(bodyPrefab);
            SecondarySetup(bodyPrefab);
            UtilitySetup(bodyPrefab);
            SpecialSetup(bodyPrefab);
        }

        private static void PassiveSetup()
        {
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = "PALADIN_PASSIVE_NAME";
            skillLocator.passiveSkill.skillDescriptionToken = "PALADIN_PASSIVE_DESCRIPTION";
            skillLocator.passiveSkill.icon = Asset.iconP;
        }

        private static void PrimarySetup(GameObject bodyPrefab)
        {
            SteppedSkillDef mySkillDef = ScriptableObject.CreateInstance<SteppedSkillDef>();
            mySkillDef.stepCount = 4;
            mySkillDef.stepGraceDuration = 0.3f;
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Slash));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Asset.icon1;
            mySkillDef.skillDescriptionToken = "PALADIN_PRIMARY_SLASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_PRIMARY_SLASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_PRIMARY_SLASH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_SWORDBEAM"
            };

            if (bodyPrefab.name == "LunarKnightBody")
            {
                mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.LunarKnight.MaceSlam));
                mySkillDef.activationStateMachineName = "Body";
            }

            if (bodyPrefab.name == "NemesisPaladinBody")
            {
                mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Nemesis.PunchCombo));
            }

            skillDefs.Add(mySkillDef);

            skillLocator.primary = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (newFamily as ScriptableObject).name = "rob_Paladin_Primary";
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.primary._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void SecondarySetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.SpinSlashEntry));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Asset.icon2;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_SPINSLASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_SPINSLASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_SPINSLASH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING",
            };

            skillDefs.Add(mySkillDef);

            skillLocator.secondary = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (newFamily as ScriptableObject).name = "rob_Paladin_Secondary";
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.secondary._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.ChargeLightningSpear));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 8f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Asset.icon2b;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_LIGHTNING_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_LIGHTNING_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_LIGHTNING_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_SHOCKING",
                "KEYWORD_AGILE"
            };

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.LunarShards));
            mySkillDef.activationStateMachineName = "Slide";
            mySkillDef.baseMaxStock = StaticValues.lunarShardMaxStock;
            mySkillDef.baseRechargeInterval = 1.5f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.resetCooldownTimerOnUse = true;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.rechargeStock = 99;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Asset.icon2c;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_LUNARSHARD_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_LUNARSHARD_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_AGILE"
            };

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinLunarShardSkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void UtilitySetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Quickstep.QuickstepSimple));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 2;
            mySkillDef.baseRechargeInterval = 10f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.forceSprintDuringState = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Asset.icon3;
            mySkillDef.skillDescriptionToken = "PALADIN_UTILITY_DASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_UTILITY_DASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_UTILITY_DASH_NAME";

            skillDefs.Add(mySkillDef);

            skillLocator.utility = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (newFamily as ScriptableObject).name = "rob_Paladin_Utility";
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.utility._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelSmallHeal));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 8;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Asset.icon3b;
            mySkillDef.skillDescriptionToken = "PALADIN_UTILITY_HEAL_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_UTILITY_HEAL_NAME";
            mySkillDef.skillNameToken = "PALADIN_UTILITY_HEAL_NAME";

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinHealSkillDefDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void SpecialSetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelHealZone));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 0;
            mySkillDef.icon = Asset.icon4;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_HEALZONE_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_HEALZONE_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_HEALZONE_NAME";

            skillDefs.Add(mySkillDef);

            skillLocator.special = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (newFamily as ScriptableObject).name = "rob_Paladin_Special";
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.special._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.special.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelTorpor));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 0;
            mySkillDef.icon = Asset.icon4b;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_TORPOR_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_TORPOR_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_TORPOR_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_TORPOR"
            };

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinTorporSkillDefDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelWarcry));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 0;
            mySkillDef.icon = Asset.icon4c;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_WARCRY_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_WARCRY_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_WARCRY_NAME";

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelCruelSun));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 12f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = false;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 0;
            mySkillDef.icon = Asset.icon4d;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_SUN_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_SUN_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_SUN_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_OVERHEAT"
            };

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinCruelSunSkillDefDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            cancelSunSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            cancelSunSkillDef.activationState = new SerializableEntityStateType(typeof(EntityStates.Idle));
            cancelSunSkillDef.activationStateMachineName = "Weapon";
            cancelSunSkillDef.baseMaxStock = 0;
            cancelSunSkillDef.baseRechargeInterval = 0f;
            cancelSunSkillDef.fullRestockOnAssign = false;
            cancelSunSkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            cancelSunSkillDef.resetCooldownTimerOnUse = false;
            cancelSunSkillDef.isCombatSkill = false;
            cancelSunSkillDef.mustKeyPress = true;
            cancelSunSkillDef.rechargeStock = 0;
            cancelSunSkillDef.requiredStock = 0;
            cancelSunSkillDef.stockToConsume = 0;
            cancelSunSkillDef.icon = Asset.mainAssetBundle.LoadAsset<Sprite>("CruelSunIconCancel");
            cancelSunSkillDef.skillDescriptionToken = "PALADIN_SPECIAL_SUN_CANCEL_DESCRIPTION";
            cancelSunSkillDef.skillName = "PALADIN_SPECIAL_SUN_CANCEL_NAME";
            cancelSunSkillDef.skillNameToken = "PALADIN_SPECIAL_SUN_CANCEL_NAME";


            if (Config.legacyCruelSun.Value) {
                mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
                mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelCruelSunOld));
                mySkillDef.activationStateMachineName = "Weapon";
                mySkillDef.baseMaxStock = 1;
                mySkillDef.baseRechargeInterval = 40f;
                mySkillDef.beginSkillCooldownOnSkillEnd = true;
                mySkillDef.canceledFromSprinting = false;
                mySkillDef.fullRestockOnAssign = false;
                mySkillDef.interruptPriority = InterruptPriority.Skill;
                mySkillDef.resetCooldownTimerOnUse = false;
                mySkillDef.isCombatSkill = true;
                mySkillDef.mustKeyPress = true;
                mySkillDef.cancelSprintingOnActivation = true;
                mySkillDef.rechargeStock = 1;
                mySkillDef.requiredStock = 1;
                mySkillDef.stockToConsume = 1;
                mySkillDef.icon = Asset.icon4d;
                mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_SUN_LEGACY_DESCRIPTION";
                mySkillDef.skillName = "PALADIN_SPECIAL_SUN_LEGACY_NAME";
                mySkillDef.skillNameToken = "PALADIN_SPECIAL_SUN_LEGACY_NAME";
                mySkillDef.keywordTokens = null;

                skillDefs.Add(mySkillDef);

                Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
                skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant {
                    skillDef = mySkillDef,
                    unlockableDef = Modules.Unlockables.paladinCruelSunSkillDefDef,
                    viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
                };
            }
            SkillDef berserkSkillDef = CreateRageSkillDef(new SkillDefInfo
            {
                skillName = "PALADIN_SPECIAL_BERSERK_NAME",
                skillNameToken = "PALADIN_SPECIAL_BERSERK_NAME",
                skillDescriptionToken = "PALADIN_SPECIAL_BERSERK_DESCRIPTION",
                skillIcon = Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("BerserkIcon"),
                activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Rage.RageEnter)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 100,
                requiredStock = 0,
                stockToConsume = 0
            });

            berserkOutSkillDef = CreateSkillDef(new SkillDefInfo
            {
                skillName = "PALADIN_SPECIAL_BERSERK_NAME",
                skillNameToken = "PALADIN_SPECIAL_BERSERK_NAME",
                skillDescriptionToken = "PALADIN_SPECIAL_BERSERK_DESCRIPTION",
                skillIcon = Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("BerserkIcon"),
                activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Rage.RageExit)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            berserkDashSkillDef = CreateSkillDef(new SkillDefInfo
            {
                skillName = "PALADIN_UTILITY_BLINK_NAME",
                skillNameToken = "PALADIN_UTILITY_BLINK_NAME",
                skillDescriptionToken = "PALADIN_UTILITY_BLINK_DESCRIPTION",
                skillIcon = Asset.icon3,
                activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Quickstep.FlashStep)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            //if (Modules.Config.cursed.Value) AddSpecialSkill(bodyPrefab, berserkSkillDef);
        }

        internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
        {
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            skillDef.skillName = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static SkillDef CreateRageSkillDef(SkillDefInfo skillDefInfo)
        {
            PaladinRageSkillDef skillDef = ScriptableObject.CreateInstance<PaladinRageSkillDef>();

            skillDef.skillName = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSecondarySkill(targetPrefab, i);
            }
        }

        internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddUtilitySkill(targetPrefab, i);
            }
        }

        internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.special.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSpecialSkill(targetPrefab, i);
            }
        }
    }
}

internal class SkillDefInfo
{
    public string skillName;
    public string skillNameToken;
    public string skillDescriptionToken;
    public Sprite skillIcon;

    public SerializableEntityStateType activationState;
    public string activationStateMachineName;
    public int baseMaxStock;
    public float baseRechargeInterval;
    public bool beginSkillCooldownOnSkillEnd;
    public bool canceledFromSprinting;
    public bool forceSprintDuringState;
    public bool fullRestockOnAssign;
    public InterruptPriority interruptPriority;
    public bool resetCooldownTimerOnUse;
    public bool isCombatSkill;
    public bool mustKeyPress;
    public bool cancelSprintingOnActivation;
    public int rechargeStock;
    public int requiredStock;
    public int stockToConsume;

    public string[] keywordTokens;
}