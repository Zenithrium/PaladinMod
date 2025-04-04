﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaladinMod.Modules
{
    public enum PaladinCSSEffect
    {
        DEFAULT,
        LUNAR,
        BEEFY,
        TAR,
        YELLOW,
        GREEN,
        GREENSCYTHE,
        RED,
        REDSCYTHE,
        PURPLE,
        FLAME
    }

    public enum PassiveEffect
    {
        SwordActiveEffect,
        SwordActiveEffectLunar,
        SwordActiveEffectBeefy,
        SwordActiveEffectTar,
        SwordActiveEffectSun,
        SwordActiveEffectGreenScythe,
        SwordActiveEffectGreenOld,
        SwordActiveEffectRedScythe,
        SwordActiveEffectRed,
        SwordActiveEffectPurple,
        SwordActiveEffectFlame,
    }
    public enum HitEffect
    {
        Default,
        Green,
        Yellow,
        Red,
        Clay,
        Purple,
        Black,
        Blunt,
    }
    public enum SwingEffect
    {
        Default,
        Green,
        Yellow,
        Red,
        Clay,
        Purple,
        Black,
        Flame,
        White,
        Bat,
    }
    public enum SpinSlashEffect
    {
        Default,
        Green,
        Yellow,
        Red,
        Clay,
        Purple,
        Black,
        Flame,
    }
    public enum EmpwoeredSpinSlashEffect
    {
        Default,
        Green,
        Yellow,
        Red,
        Clay,
        Purple,
        Black,
        Flame
    }
    public static class Effects
    {
        private static List<PaladinSkinInfo> paladinSkinInfos;

        public delegate void RegisterEffectsEvent();
        public static event RegisterEffectsEvent OnRegisterEffects;

        [System.Obsolete("use Effects.AddSkinInfo to add your skininfo")]
        public static PaladinSkinInfo[] skinInfos = null;
        
        public struct PaladinSkinInfo
        {
            /// <summary>
            /// The NameToken of your skin
            /// </summary>
            public string skinNameToken;
            /// <summary>
            /// the sword glow when you're blessed (use passiveEffectType)
            /// </summary>
            public string passiveEffectName;
            /// <summary>
            /// the sword glow when you're blessed
            /// </summary>
            public PassiveEffect passiveEffectType;
            /// <summary>
            /// "PaladinSwing", "PaladinSwingBlunt", any sound you like, or leave empty to use default
            /// </summary>
            public string swingSoundString;
            /// <summary>
            /// changes impact sounds to blunt versions for all swing impacts. leave false to keep default slash impact sounds
            /// </summary>
            public bool isWeaponBlunt;
            /// <summary>
            /// leave null and use use the hitEffectType enum, or load in a completely custom effect
            /// </summary>
            public GameObject hitEffect;
            public HitEffect hitEffectType;
            /// <summary>
            /// leave null and use use the swingEffectType enum, or load in a completely custom effect
            /// </summary>
            public GameObject swingEffect;
            public SwingEffect swingEffectType;
            /// <summary>
            /// leave null and use use the spinSlashEffectType enum, or load in a completely custom effect
            /// </summary>
            public GameObject spinSlashEffect;
            public SpinSlashEffect spinSlashEffectType;
            /// <summary>
            /// leave null and use use the empoweredSpinSlashEffect enum, or load in a completely custom effect
            /// </summary>
            public GameObject empoweredSpinSlashEffect;
            public EmpwoeredSpinSlashEffect empoweredSpinSlashEffectType;

            public Color eyeTrailColor;
            /// <summary>
            /// leave null to use default. leave null and pass in a color in swordBeamProjectileColor to recolor
            /// </summary>
            public GameObject swordBeamProjectileGhost;
            /// <summary>
            /// leave null to use default. does nothing if there is a swordBeamProjectileGhost passed in
            /// </summary>
            public Color? swordBeamProjectileColor;

            public PaladinCSSEffect CSSEffect;

            public void FinalizeInfo()
            {
                if (string.IsNullOrEmpty(passiveEffectName))
                {
                    passiveEffectName = passiveEffectType.ToString();
                }

                if (string.IsNullOrEmpty(swingSoundString))
                {
                    swingSoundString = Sounds.Swing;
                }

                if (hitEffect == null)
                {
                    hitEffect = Asset.hitEffects[(int)hitEffectType];
                }
                if (swingEffect == null)
                {
                    swingEffect = Asset.swordSwingEffects[(int)swingEffectType];
                }
                if (spinSlashEffect == null)
                {
                    spinSlashEffect = Asset.spinSlashEffects[(int)spinSlashEffectType];
                }
                if (empoweredSpinSlashEffect == null)
                {
                    empoweredSpinSlashEffect = Asset.spinSlashEmpoweredEffects[(int)empoweredSpinSlashEffectType];
                }

                if (swordBeamProjectileGhost == null && swordBeamProjectileColor != null)
                {
                    swordBeamProjectileGhost = Projectiles.CloneAndColorSwordBeam(swordBeamProjectileColor.Value);
                }
            }
        }

        public static void RegisterEffects()
        {
            paladinSkinInfos = new List<PaladinSkinInfo>();
            
            #region Skins
            paladinSkinInfos.Add(new PaladinSkinInfo {
                skinNameToken = "PALADINBODY_DEFAULT_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwing,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX,
                eyeTrailColor = Color.white
            });

            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_LUNAR_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectLunar",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwing,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX, 
                eyeTrailColor = new Color(196f / 255f, 255f / 255f, 254f / 255f)
            });
            paladinSkinInfos.Add(new PaladinSkinInfo {
                skinNameToken = "PALADINBODY_LUNARKNIGHT_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = true,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwing,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX,
                eyeTrailColor = new Color(196f / 255f, 255f / 255f, 254f / 255f)
            });

            paladinSkinInfos.Add(new PaladinSkinInfo 
            {
                skinNameToken = "PALADINBODY_TYPHOON_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectGM", 
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwing,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX,
                eyeTrailColor = new Color(255f / 255f, 215f / 255f, 0),
            });
            paladinSkinInfos.Add(new PaladinSkinInfo {
                skinNameToken = "PALADINBODY_TYPHOONLEGACY_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectGM",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwing,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX,
                eyeTrailColor = new Color(255f / 255f, 215f / 255f, 0)
            });

            paladinSkinInfos.Add(new PaladinSkinInfo
            { 
                skinNameToken = "PALADINBODY_POISON_SKIN_NAME", 
                passiveEffectName = "SwordActiveEffectGreenScythe",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXGreen,
                swingEffect = Asset.swordSwingGreen,
                spinSlashEffect = Asset.spinningSlashFXGreen,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXGreen, 
                eyeTrailColor = new Color(133f / 255f, 255f / 255f, 147f / 255f)
            });
            paladinSkinInfos.Add(new PaladinSkinInfo {
                skinNameToken = "PALADINBODY_POISONLEGACY_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectGreenOld",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXGreen,
                swingEffect = Asset.swordSwingGreen,
                spinSlashEffect = Asset.spinningSlashFXGreen,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXGreen,
                eyeTrailColor = new Color(133f / 255f, 255f / 255f, 147f / 255f)
            });

            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_CLAY_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectTar",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXClay,
                swingEffect = Asset.swordSwingClay,
                spinSlashEffect = Asset.spinningSlashFXClay,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXClay,
                eyeTrailColor = new Color(255f / 255f, 64f / 255f, 64f / 255f)
            });

            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_SPECTER_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectRedScythe",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXRed,
                swingEffect = Asset.swordSwingRed,
                spinSlashEffect = Asset.spinningSlashFXRed,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXRed,
                //eyeTrailColor = new Color(248f / 255f, 23f / 255f, 83f / 255f)
            });

            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_DRIP_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.SwingBlunt,
                isWeaponBlunt = true,
                hitEffect = Asset.hitFXBlunt,
                swingEffect = Asset.swordSwingBat,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX,
                eyeTrailColor = Color.white
            });

            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_MINECRAFT_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectGM",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwing,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX,
                eyeTrailColor = Color.white
            });
            #endregion

            #region DarkSoulsSkins
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_ABYSSWATCHER_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectFlame",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXRed,
                swingEffect = Asset.swordSwingFlame,
                spinSlashEffect = Asset.spinningSlashFXFlame,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXFlame
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_ARTORIAS_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectPurple",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXPurple,
                swingEffect = Asset.swordSwingPurple,
                spinSlashEffect = Asset.spinningSlashFXPurple,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXPurple
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_BLACKKNIGHT_SKIN_NAME",
                passiveEffectName = "",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwingWhite,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_FARAAM_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFX,
                swingEffect = Asset.swordSwing,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_GIANTDAD_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectSun",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXYellow,
                swingEffect = Asset.swordSwingYellow,
                spinSlashEffect = Asset.spinningSlashFXYellow,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXYellow
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_GWYN_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectFlame",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXRed,
                swingEffect = Asset.swordSwingFlame,
                spinSlashEffect = Asset.spinningSlashFXFlame,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXFlame
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_HAVEL_SKIN_NAME",
                passiveEffectName = "",
                swingSoundString = Sounds.SwingBlunt,
                isWeaponBlunt = true,
                hitEffect = Asset.hitFXBlunt,
                swingEffect = Asset.swordSwingWhite,
                spinSlashEffect = Asset.spinningSlashFX,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFX
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_ORNSTEIN_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectSun",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXYellow,
                swingEffect = Asset.swordSwingYellow,
                spinSlashEffect = Asset.spinningSlashFXYellow,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXYellow
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_PURSUER_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectRed",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXRed,
                swingEffect = Asset.swordSwingRed,
                spinSlashEffect = Asset.spinningSlashFXRed,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXRed
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_RINGEDKNIGHT_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectFlame",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXRed,
                swingEffect = Asset.swordSwingFlame,
                spinSlashEffect = Asset.spinningSlashFXFlame,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXFlame
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_SOLAIRE_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectSun",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXYellow,
                swingEffect = Asset.swordSwingYellow,
                spinSlashEffect = Asset.spinningSlashFXYellow,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXYellow
            });
            paladinSkinInfos.Add(new PaladinSkinInfo
            {
                skinNameToken = "PALADINBODY_DARKWRAITH_SKIN_NAME",
                passiveEffectName = "",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Asset.hitFXBlack,
                swingEffect = Asset.swordSwingBlack,
                spinSlashEffect = Asset.spinningSlashFXBlack,
                empoweredSpinSlashEffect = Asset.spinningSlashEmpoweredFXBlack
            });
            #endregion

            OnRegisterEffects?.Invoke();
        }
        
        public static void AddSkinInfo(PaladinSkinInfo skinInfo)
        {
            skinInfo.FinalizeInfo();
            paladinSkinInfos.Add(skinInfo);
        }

        public static PaladinSkinInfo GetSkinInfo(string skinName)
        {
            for (int i = 0; i < paladinSkinInfos.Count; i++)
            {
                if (paladinSkinInfos[i].skinNameToken == skinName)
                {
                    return paladinSkinInfos[i];
                }
            }
            return paladinSkinInfos[0];
        }
    }
}