﻿using System;
using UnityEngine;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using SkinChangeResponse = RoR2.CharacterSelectSurvivorPreviewDisplayController.SkinChangeResponse;

namespace PaladinMod.Modules
{
    public static class Skins
    {
        static List<GameObject> allGameObjectActivations = new List<GameObject>();

        private static CharacterSelectSurvivorPreviewDisplayController paladinCSSPreviewController;
        private static SkinChangeResponse[] defaultResponses;

        private static void AddCSSSkinChangeResponse(SkinDef def, PaladinCSSEffect cssEffect) {

            //duplicating a skinchangeresponse from defaults that I set up in editor
            //gotta do this song and dance instead of simply adding our own custom skinchangeresponses because for some reason adding to unityevents in code doesn't work
            //or at least didn't work last time i tried
            SkinChangeResponse newSkinResponse = defaultResponses[(int)cssEffect];
            newSkinResponse.triggerSkin = def;

            HG.ArrayUtils.ArrayAppend(ref paladinCSSPreviewController.skinChangeResponses, newSkinResponse);
        }

        #region tools
        /// <summary>
        /// create an array of all gameobjects that are activated/deactivated by skins, then for each skin pass in the specific objects that will be active
        /// </summary>
        /// <param name="activatedObjects">specific objects that will be active. add these objects to the allGameObjectActivations list</param>
        /// <returns></returns>
        private static SkinDef.GameObjectActivation[] getActivations(params GameObject[] activatedObjects) {

            List<SkinDef.GameObjectActivation> GameObjectActivations = new List<SkinDef.GameObjectActivation>();

            for (int i = 0; i < allGameObjectActivations.Count; i++) {

                bool activate = activatedObjects.Contains(allGameObjectActivations[i]);
                //activate |= Config.spookyArms.Value && (i == 1 || i == 2);

                GameObjectActivations.Add(new SkinDef.GameObjectActivation {
                    gameObject = allGameObjectActivations[i],
                    shouldActivate = activate
                });
            }

            return GameObjectActivations.ToArray();
        }

        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root)
        {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, null);
        }
        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef)
        {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, unlockableDef, null);
        }
        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef, Mesh skinMesh)
        {
            SkinDefInfo skinDefInfo = new SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[]
                {
                    new SkinDef.MeshReplacement
                    {
                        renderer = mainRenderer,
                        mesh = skinMesh
                    }
                },
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = rendererInfos,
                RootObject = root,
                UnlockableDef = unlockableDef 
            };

            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<RoR2.SkinDef>();
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            skinDef.name = skinDefInfo.Name;

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, RoR2.SkinDef self)
        {
        }

        internal struct SkinDefInfo
        {
            internal SkinDef[] BaseSkins;
            internal Sprite Icon;
            internal string NameToken;
            internal UnlockableDef UnlockableDef;
            internal GameObject RootObject;
            internal CharacterModel.RendererInfo[] RendererInfos;
            internal SkinDef.MeshReplacement[] MeshReplacements;
            internal SkinDef.GameObjectActivation[] GameObjectActivations;
            internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            internal string Name;
        }

        public static Material CreateMaterial(string materialName)
        {
            return CreateMaterial(materialName, 0);
        }

        public static Material CreateMaterial(string materialName, float emission)
        {
            return CreateMaterial(materialName, emission, Color.black);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
        {
            return CreateMaterial(materialName, emission, emissionColor, 0);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!PaladinPlugin.commandoMat) PaladinPlugin.commandoMat = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;
            
            Material mat = UnityEngine.Object.Instantiate<Material>(PaladinPlugin.commandoMat);
            Material tempMat = Asset.mainAssetBundle.LoadAsset<Material>(materialName);
            if (!tempMat)
            {
                Debug.LogError("failed to load material from bundle: " + materialName);
                return PaladinPlugin.commandoMat;
            }

            mat.name = materialName;
            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);
            //mat.SetInt("_Cull", 0);
            return mat;
        }
        #endregion


        #region GET SKIN
        public static Dictionary<uint, string> SkinIdices = new Dictionary<uint, string>();
        public static List<SkinDef> skinDefs = new List<SkinDef>();

        public enum PaladinSkin
        {
            NONE = -1,
            DEFAULT,
            MASTERY,
            GRANDMASTERY,
            POISON,
            CLAY,
            SPECTER,
            DRIP,
            MINECRAFT,
            LUNARKNIGHT,
            TYPHOONLEGACY,
            POISONLEGACY
        }

        public static bool isPaladinCurrentSkin(CharacterBody characterbody, string skin)
        {
            return characterbody.baseNameToken == "PALADIN_NAME" && SkinIdices.ContainsKey(characterbody.skinIndex) && SkinIdices[characterbody.skinIndex] == skin;
        }

        public static bool isPaladinCurrentSkin(CharacterBody characterbody, PaladinSkin skin)
        {
            return isPaladinCurrentSkin(characterbody, GetFuckingSkinID(skin));
        }

        public static string GetFuckingSkinID(PaladinSkin skin)
        {
            switch (skin)
            {
                default:
                case PaladinSkin.DEFAULT:
                    return "PALADINBODY_DEFAULT_SKIN_NAME";
                case PaladinSkin.MASTERY:
                    return "PALADINBODY_LUNAR_SKIN_NAME";
                case PaladinSkin.GRANDMASTERY:
                    return "PALADINBODY_TYPHOON_SKIN_NAME";
                case PaladinSkin.POISON:
                    return "PALADINBODY_POISON_SKIN_NAME";
                case PaladinSkin.CLAY:
                    return "PALADINBODY_CLAY_SKIN_NAME";
                case PaladinSkin.SPECTER:
                    return "PALADINBODY_SPECTER_SKIN_NAME";
                case PaladinSkin.DRIP:
                    return "PALADINBODY_DRIP_SKIN_NAME";
                case PaladinSkin.MINECRAFT:
                    return "PALADINBODY_MINECRAFT_SKIN_NAME";
                case PaladinSkin.LUNARKNIGHT:
                    return "PALADINBODY_LUNARKNIGHT_SKIN_NAME";
                case PaladinSkin.TYPHOONLEGACY:
                    return "PALADINBODY_TYPHOONLEGACY_SKIN_NAME";
                case PaladinSkin.POISONLEGACY:
                    return "PALADINBODY_POISONLEGACY_SKIN_NAME";
            }
        }
        #endregion

        public static void RegisterSkins() 
        {
            GameObject bodyPrefab = Prefabs.paladinPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();
             
            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            GameObject displayPrefab = Prefabs.paladinDisplayPrefab;
            ChildLocator displayChildLocator = displayPrefab.GetComponent<ChildLocator>(); 
            paladinCSSPreviewController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            paladinCSSPreviewController.bodyPrefab = bodyPrefab;
            defaultResponses = paladinCSSPreviewController.skinChangeResponses; 

            skinDefs = new List<SkinDef>();

            GameObject cape = childLocator.FindChild("CapeModel").gameObject;
            GameObject armLeft = childLocator.FindChild("CreepyArmsLeft").gameObject;
            GameObject armRight = childLocator.FindChild("CreepyArmsRight").gameObject;
            GameObject fuckingCrystalCrown = childLocator.FindChild("FuckMe").gameObject;

            allGameObjectActivations.Add(cape);
            allGameObjectActivations.Add(armLeft);
            allGameObjectActivations.Add(armRight);
            allGameObjectActivations.Add(fuckingCrystalCrown);

            SkinDef.GameObjectActivation[] defaultActivations = getActivations();
            SkinDef.GameObjectActivation[] capeActivations = getActivations(cape);
            SkinDef.GameObjectActivation[] nkuhanaActivations = getActivations(armLeft, armRight);
            SkinDef.GameObjectActivation[] GMActivations = getActivations(cape, fuckingCrystalCrown);
            
            #region DefaultSkin
            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;
            int lastRend = defaultRenderers.Length - 1;
            SkinDef defaultSkin = CreateSkinDef("PALADINBODY_DEFAULT_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"), defaultRenderers, mainRenderer, model);
            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.defaultMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.defaultSwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.defaultCapeMesh,
                    renderer = defaultRenderers[1].renderer
                },
            }; 

            if (Modules.Config.cape.Value)
            {
                defaultSkin.gameObjectActivations = capeActivations;
                childLocator.FindChild("CapeModel").gameObject.SetActive(true);
            }
            else defaultSkin.gameObjectActivations = defaultActivations;

            AddCSSSkinChangeResponse(defaultSkin, PaladinCSSEffect.DEFAULT);

            skinDefs.Add(defaultSkin);
            #endregion

            #region MasterySkin(lunar)
            CharacterModel.RendererInfo[] masteryRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(masteryRendererInfos, 0);

            // add the passive effect
            #region clone mithrix effect
            GameObject lunarPassiveEffect = GameObject.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<ChildLocator>().FindChild("Phase3HammerFX").gameObject);
            lunarPassiveEffect.transform.parent = childLocator.FindChild("SwordActiveEffectLunar");
            lunarPassiveEffect.transform.localScale = Vector3.one * 0.0002f;
            lunarPassiveEffect.transform.rotation = Quaternion.Euler(new Vector3(45, 90, 0));
            lunarPassiveEffect.transform.localPosition = new Vector3(0, 0, -0.003f);
            lunarPassiveEffect.gameObject.SetActive(true);

            lunarPassiveEffect.transform.Find("Amb_Fire_Ps, Left").localScale = Vector3.one * 0.6f;
            lunarPassiveEffect.transform.Find("Amb_Fire_Ps, Right").localScale = Vector3.one * 0.6f;
            lunarPassiveEffect.transform.Find("Core, Light").localScale = Vector3.one * 0.1f;
            lunarPassiveEffect.transform.Find("Blocks, Spinny").localScale = Vector3.one * 0.4f;
            lunarPassiveEffect.transform.Find("Sparks").localScale = Vector3.one * 0.4f;

            lunarPassiveEffect = GameObject.Instantiate(lunarPassiveEffect);
            lunarPassiveEffect.transform.parent = displayChildLocator.FindChild("SwordActiveEffectLunar");
            lunarPassiveEffect.transform.localScale = Vector3.one * 0.0002f;
            lunarPassiveEffect.transform.rotation = Quaternion.Euler(new Vector3(45, 90, 0));
            lunarPassiveEffect.transform.localPosition = new Vector3(0, 0, -0.003f);
            lunarPassiveEffect.gameObject.SetActive(true);
            #endregion

            Material lunarSwordMat = CreateMaterial("matLunarSword", 0, Color.black, 1f);
            lunarSwordMat.EnableKeyword("FORCE_SPEC");
            lunarSwordMat.EnableKeyword("FRESNEL_EMISSION");
            lunarSwordMat.SetFloat("_SpecularStrength", 1f);
            masteryRendererInfos[0].defaultMaterial = lunarSwordMat;
            //masteryRendererInfos[1].defaultMaterial = CreateMaterial("matLunarCape");
            masteryRendererInfos[lastRend].defaultMaterial = CreateMaterial("matLunarPaladin", 5, Color.white, 1f);

            SkinDef masterySkin = CreateSkinDef("PALADINBODY_LUNAR_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"), masteryRendererInfos, mainRenderer, model, Modules.Unlockables.paladinMasterySkinDef);
            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.lunarMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.lunarSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            masterySkin.gameObjectActivations = defaultActivations;

            masterySkin.projectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[] {
                new SkinDef.ProjectileGhostReplacement{
                    projectilePrefab = Projectiles.swordBeamProjectile,
                    projectileGhostReplacementPrefab = Projectiles.CloneAndColorSwordBeam(new Color(0.3f, 0.1f, 1f), 1.2f)//slightly darker blue than normal
                }
            };

            AddCSSSkinChangeResponse(masterySkin, PaladinCSSEffect.LUNAR);

            skinDefs.Add(masterySkin);
            #endregion

            #region GrandMasterySkin
            CharacterModel.RendererInfo[] grandMasteryRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(grandMasteryRendererInfos, 0);

            grandMasteryRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinGMSword", StaticValues.maxSwordGlow, Color.white);
            grandMasteryRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinGM", 10, Color.white);
            grandMasteryRendererInfos[lastRend].defaultMaterial = CreateMaterial("matPaladinGM", 10, Color.white);

            SkinDef grandMasterySkin = CreateSkinDef("PALADINBODY_TYPHOON_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texGrandMasteryAchievement"), grandMasteryRendererInfos, mainRenderer, model, Modules.Unlockables.paladinGrandMasterySkinDef);
            grandMasterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.gmMesh,
                    renderer = defaultRenderers[lastRend].renderer 
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.gmSwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.gmCapeMesh,
                    renderer = defaultRenderers[1].renderer
                }
            };

            grandMasterySkin.gameObjectActivations = GMActivations;

            AddCSSSkinChangeResponse(grandMasterySkin, PaladinCSSEffect.BEEFY);

            skinDefs.Add(grandMasterySkin);

            #endregion

            #region PoisonSkin
            CharacterModel.RendererInfo[] poisonRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(poisonRendererInfos, 0);

            poisonRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinNkuhanaScythe", StaticValues.maxSwordGlow, Color.white);
            poisonRendererInfos[lastRend].defaultMaterial = CreateMaterial("matPaladinNkuhana", 3, Color.white);

            SkinDef poisonSkin = CreateSkinDef("PALADINBODY_POISON_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texPoisonAchievement"), poisonRendererInfos, mainRenderer, model, Modules.Unlockables.paladinPoisonSkinDef);
            poisonSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.poisonMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.poisonSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
                //arms are only in this skin they don't need to be mesh replacements, just gameobjectactivations 
            };

            poisonSkin.gameObjectActivations = nkuhanaActivations;

            poisonSkin.projectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[] {
                new SkinDef.ProjectileGhostReplacement{
                    projectilePrefab = Projectiles.swordBeamProjectile,
                    projectileGhostReplacementPrefab = Projectiles.CloneAndColorSwordBeam(Color.green, 0.6f)
                }
            };

            AddCSSSkinChangeResponse(poisonSkin, PaladinCSSEffect.GREENSCYTHE);

            skinDefs.Add(poisonSkin);
            #endregion

            #region ClaySkin
            CharacterModel.RendererInfo[] clayRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(clayRendererInfos, 0);

            clayRendererInfos[0].defaultMaterial = CreateMaterial("matClayPaladin", StaticValues.maxSwordGlow, Color.white);
            clayRendererInfos[lastRend].defaultMaterial = CreateMaterial("matClayPaladin", 10, Color.white);

            SkinDef claySkin = CreateSkinDef("PALADINBODY_CLAY_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texClayAchievement"), clayRendererInfos, mainRenderer, model, Modules.Unlockables.paladinClaySkinDef);
            claySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.clayMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.claySwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            claySkin.gameObjectActivations = defaultActivations;

            claySkin.projectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[] {
                new SkinDef.ProjectileGhostReplacement{
                    projectilePrefab = Projectiles.swordBeamProjectile,
                    projectileGhostReplacementPrefab = Projectiles.CloneAndColorSwordBeam(new Color(1f,0.35f,0), 0.65f) //yello smello
                }
            };

            AddCSSSkinChangeResponse(claySkin, PaladinCSSEffect.TAR);

            skinDefs.Add(claySkin);
            #endregion

            #region SpecterSkin
            CharacterModel.RendererInfo[] specterRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(specterRendererInfos, 0);

            specterRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinSpecterScythe", StaticValues.maxSwordGlow, Color.white);
            specterRendererInfos[lastRend].defaultMaterial = CreateMaterial("matPaladinSpecter");

            SkinDef specterSkin = CreateSkinDef("PALADINBODY_SPECTER_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texSpecterSkin"), specterRendererInfos, mainRenderer, model);
            specterSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.specterMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.specterSwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.defaultCapeMesh,
                    renderer = defaultRenderers[1].renderer
                }
            };

            specterSkin.gameObjectActivations = capeActivations;

            specterSkin.projectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[] {
                new SkinDef.ProjectileGhostReplacement{
                    projectilePrefab = Projectiles.swordBeamProjectile,
                    projectileGhostReplacementPrefab = Projectiles.CloneAndColorSwordBeam(Color.red, 0.669f)
                }
            };

            AddCSSSkinChangeResponse(specterSkin, PaladinCSSEffect.REDSCYTHE);

            if (Modules.Config.cursed.Value) skinDefs.Add(specterSkin);

            #endregion            
            #region DripSkin
            CharacterModel.RendererInfo[] dripRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(dripRendererInfos, 0);

            dripRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinDrip", StaticValues.maxSwordGlow, Color.white);
            dripRendererInfos[lastRend].defaultMaterial = CreateMaterial("matPaladinDrip", 3, Color.white);

            SkinDef dripSkin = CreateSkinDef("PALADINBODY_DRIP_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texDripAchievement"), dripRendererInfos, mainRenderer, model);
            dripSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.dripMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.batMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            dripSkin.gameObjectActivations = defaultActivations;

            AddCSSSkinChangeResponse(dripSkin, PaladinCSSEffect.DEFAULT);

            if (Modules.Config.cursed.Value) skinDefs.Add(dripSkin);
            #endregion
            #region MinecraftSkin
            CharacterModel.RendererInfo[] minecraftRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(minecraftRendererInfos, 0);

            minecraftRendererInfos[0].defaultMaterial = CreateMaterial("matMinecraftSword", StaticValues.maxSwordGlow, Color.white);
            minecraftRendererInfos[lastRend].defaultMaterial = CreateMaterial("matMinecraftPaladin", 3, Color.white);

            SkinDef minecraftSkin = CreateSkinDef("PALADINBODY_MINECRAFT_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texMinecraftSkin"), minecraftRendererInfos, mainRenderer, model);
            minecraftSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.minecraftMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.minecraftSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            minecraftSkin.gameObjectActivations = defaultActivations;

            AddCSSSkinChangeResponse(minecraftSkin, PaladinCSSEffect.BEEFY);

            if (Modules.Config.cursed.Value) skinDefs.Add(minecraftSkin);
            #endregion

            #region LunarKnightSkin(lunar)
            CharacterModel.RendererInfo[] lunarKnightRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(lunarKnightRendererInfos, 0);

            lunarKnightRendererInfos[0].defaultMaterial = CreateMaterial("matLunarKnight", StaticValues.maxSwordGlow, Color.white);
            lunarKnightRendererInfos[1].defaultMaterial = lunarKnightRendererInfos[0].defaultMaterial;
            lunarKnightRendererInfos[lastRend].defaultMaterial = CreateMaterial("matLunarKnight", 5, Color.white, 1f);

            SkinDef lunarKnightSkin = CreateSkinDef("PALADINBODY_LUNARKNIGHT_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievementLegacy"), lunarKnightRendererInfos, mainRenderer, model, Modules.Unlockables.paladinMasterySkinDef);
            lunarKnightSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.lunarKnightMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.lunarKnightSwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.lunarKnightCapeMesh,
                    renderer = defaultRenderers[1].renderer
                }
            };

            lunarKnightSkin.gameObjectActivations = capeActivations;

            lunarKnightSkin.projectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[] {
                new SkinDef.ProjectileGhostReplacement{
                    projectilePrefab = Projectiles.swordBeamProjectile,
                    projectileGhostReplacementPrefab = Projectiles.CloneAndColorSwordBeam(new Color(0.3f, 0.1f, 1f), 1.2f)//mastery blue
                }
            };

            AddCSSSkinChangeResponse(lunarKnightSkin, PaladinCSSEffect.DEFAULT);

            if (Config.legacySkins.Value) 
                skinDefs.Add(lunarKnightSkin);
            #endregion
            #region GrandMasterySkinLegacy
            CharacterModel.RendererInfo[] grandMasteryLegacyRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(grandMasteryLegacyRendererInfos, 0);

            grandMasteryLegacyRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinGMSwordOld", StaticValues.maxSwordGlow, Color.white);
            grandMasteryLegacyRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinGM", 10, Color.white);
            grandMasteryLegacyRendererInfos[lastRend].defaultMaterial = CreateMaterial("matPaladinGMOld", 6.9f, Color.white);

            SkinDef grandMasteryLegacySkin = CreateSkinDef("PALADINBODY_TYPHOONLEGACY_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texGrandMasteryAchievementLegacy"), grandMasteryLegacyRendererInfos, mainRenderer, model, Modules.Unlockables.paladinGrandMasterySkinDef);
            grandMasteryLegacySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.gmLegacyMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.gmLegacySwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.gmCapeMesh,
                    renderer = defaultRenderers[1].renderer
                }
            };

            grandMasteryLegacySkin.gameObjectActivations = capeActivations;

            AddCSSSkinChangeResponse(grandMasteryLegacySkin, PaladinCSSEffect.BEEFY);

            if (Config.legacySkins.Value)
                skinDefs.Add(grandMasteryLegacySkin);

            #endregion
            #region PoisonSkinLegacy
            CharacterModel.RendererInfo[] poisonLegacyRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(poisonLegacyRendererInfos, 0);

            poisonLegacyRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinNkuhanaLegacy", StaticValues.maxSwordGlow, Color.white);
            poisonLegacyRendererInfos[1].defaultMaterial = poisonLegacyRendererInfos[0].defaultMaterial;
            poisonLegacyRendererInfos[lastRend].defaultMaterial = CreateMaterial("matPaladinNkuhanaLegacy", 3, Color.white);

            SkinDef poisonLegacySkin = CreateSkinDef("PALADINBODY_POISONLEGACY_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texPoisonAchievementLegacy"), poisonLegacyRendererInfos, mainRenderer, model, Modules.Unlockables.paladinPoisonSkinDef);
            poisonLegacySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.poisonLegacyMesh,
                    renderer = defaultRenderers[lastRend].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.poisonLegacySwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Asset.poisonLegacyCapeMesh, //no fucking clue what's going on with the nkuhana legacy cape mesh just use lunar edited
                    renderer = defaultRenderers[1].renderer
                }
            };

            poisonLegacySkin.gameObjectActivations = capeActivations;

            poisonLegacySkin.projectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[] {
                new SkinDef.ProjectileGhostReplacement{
                    projectilePrefab = Projectiles.swordBeamProjectile,
                    projectileGhostReplacementPrefab = Projectiles.CloneAndColorSwordBeam(Color.green, 0.6f)
                }
            };

            AddCSSSkinChangeResponse(poisonLegacySkin, PaladinCSSEffect.GREEN);

            if (Config.legacySkins.Value) 
                skinDefs.Add(poisonLegacySkin);
            #endregion
            

            skinController.skins = skinDefs.ToArray();

            for (int i = 0; i < skinDefs.Count; i++)
            {
                SkinIdices[(uint)i] = skinDefs[i].name;
            }

            On.RoR2.ProjectileGhostReplacementManager.Init += ProjectileGhostReplacementManager_Init;

            //InitializeNemSkins();
        }

        private static void ProjectileGhostReplacementManager_Init(On.RoR2.ProjectileGhostReplacementManager.orig_Init orig) {

            ModelSkinController skinController = Prefabs.paladinPrefab.GetComponentInChildren<ModelSkinController>();

            for (int i = 1; i < skinController.skins.Length; i++) {
                SkinDef skin = skinController.skins[i];

                Effects.PaladinSkinInfo skinInfo = Effects.GetSkinInfo(skin.nameToken);

                FinalizeSkinChangeResponse(skin, skinInfo);

                //if there is no custom skinInfo it will default to default skinInfo
                if (skinInfo.skinNameToken == "PALADINBODY_DEFAULT_SKIN_NAME")
                    continue;

                FinalizeSkinProjectileGhost(skin, skinInfo);
            }

            orig();
        }

        private static void FinalizeSkinChangeResponse(SkinDef skin, Effects.PaladinSkinInfo skinInfo)
        {
            for (int i = 0; i < paladinCSSPreviewController.skinChangeResponses.Length; i++)
            {
                if (paladinCSSPreviewController.skinChangeResponses[i].triggerSkin == skin)
                    return;
            }
            AddCSSSkinChangeResponse(skin, skinInfo.CSSEffect);
        }

        private static void FinalizeSkinProjectileGhost(SkinDef skin, Effects.PaladinSkinInfo skinInfo)
        {
            if (skin.projectileGhostReplacements.Length > 0)
                return;

            GameObject newGhost = skinInfo.swordBeamProjectileGhost;

            if (newGhost == null)
                return;

            HG.ArrayUtils.ArrayAppend(ref skin.projectileGhostReplacements, new SkinDef.ProjectileGhostReplacement
            {
                projectilePrefab = Projectiles.swordBeamProjectile,
                projectileGhostReplacementPrefab = newGhost,
            });
        }

        private static void InitializeNemSkins()
        {
            if (Prefabs.nemPaladinPrefab == null) return;

            GameObject bodyPrefab = Prefabs.nemPaladinPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            List<SkinDef> skinDefs = new List<SkinDef>();

            #region DefaultSkin
            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;
            SkinDef defaultSkin = CreateSkinDef("PALADINBODY_DEFAULT_SKIN_NAME", Asset.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"), defaultRenderers, mainRenderer, model);
            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[0];

            skinDefs.Add(defaultSkin);
            #endregion

            skinController.skins = skinDefs.ToArray();
        }

        /// <summary>
        /// pass in strings for mesh assets in your bundle. pass the same amount and order based on your rendererinfos, filling with null as needed
        /// <code>
        /// myskindef.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
        ///    "meshHenrySword",
        ///    null,
        ///    "meshHenry");
        /// </code>
        /// </summary>
        /// <param name="assetBundle">your skindef's rendererinfos to access the renderers</param>
        /// <param name="defaultRendererInfos">your skindef's rendererinfos to access the renderers</param>
        /// <param name="meshes">name of the mesh assets in your project</param>
        /// <returns></returns>
        internal static SkinDef.MeshReplacement[] GetMeshReplacements(this AssetBundle assetBundle, CharacterModel.RendererInfo[] defaultRendererInfos, params string[] meshes)
        {

            List<SkinDef.MeshReplacement> meshReplacements = new List<SkinDef.MeshReplacement>();

            for (int i = 0; i < defaultRendererInfos.Length; i++)
            {
                if (string.IsNullOrEmpty(meshes[i]))
                    continue;

                meshReplacements.Add(
                new SkinDef.MeshReplacement
                {
                    renderer = defaultRendererInfos[i].renderer,
                    mesh = assetBundle.LoadAsset<Mesh>(meshes[i])
                });
            }

            return meshReplacements.ToArray();
        }
    }
}
