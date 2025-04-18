﻿using R2API;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PaladinMod.Modules
{
    public static class Projectiles
    {
        public static GameObject swordBeamProjectile;
        //public static GameObject beamGhostRed;
        //public static GameObject beamGhostGreen;
        //public static GameObject beamGhostYellow;
        //public static GameObject beamGhostBlue;
        //public static GameObject beamGhostPurple;

        public static GameObject shockwave; 
        public static GameObject lightningSpear;
        public static GameObject lunarShard;

        public static GameObject heal;
        public static GameObject healZone;
        public static GameObject torpor;
        public static GameObject warcry;

        public static GameObject scepterHealZone;
        public static GameObject scepterTorpor;
        public static GameObject scepterWarcry;
        public static GameObject scepterCruelSun;
        public static GameObject scepterCruelSunGhost;

        public static void LateSetup()
        {
            //fuck man
            var overlapAttack = swordBeamProjectile.GetComponent<ProjectileOverlapAttack>();
            if (overlapAttack) overlapAttack.damageCoefficient = 1f;
        }

        public static void DontPool(this GameObject gob)
        {
            VFXAttributes vfx = gob.GetComponent<VFXAttributes>();
            if (vfx == null)
                vfx = gob.AddComponent<VFXAttributes>();
            vfx.DoNotPool = true;
        }

        public static void RegisterProjectiles()
        {
            //would like to simplify this all eventually....
            #region SpinningSlashShockwave
            shockwave = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/BrotherSunderWave"), "PaladinShockwave", true);
            shockwave.transform.GetChild(0).transform.localScale = new Vector3(10, 1.5f, 1);
            shockwave.GetComponent<ProjectileCharacterController>().lifetime = 0.5f;
            shockwave.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            GameObject shockwaveGhost = PrefabAPI.InstantiateClone(shockwave.GetComponent<ProjectileController>().ghostPrefab, "PaladinShockwaveGhost", false);
            shockwaveGhost.transform.GetChild(0).transform.localScale = new Vector3(10, 1, 1);
            shockwaveGhost.transform.GetChild(1).transform.localScale = new Vector3(10, 1.5f, 1);
            PaladinPlugin.Destroy(shockwaveGhost.transform.GetChild(0).Find("Infection, World").gameObject);
            PaladinPlugin.Destroy(shockwaveGhost.transform.GetChild(0).Find("Water").gameObject);
            PaladinPlugin.Destroy(shockwaveGhost.transform.GetChild(0).Find("Debris").gameObject);

            //Material shockwaveMat = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarWispTrackingBomb").GetComponent<ProjectileController>().ghostPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
            shockwaveGhost.transform.GetChild(1).GetComponent<MeshRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/matTeamAreaIndicatorIntersectionPlayer.mat").WaitForCompletion();
            shockwaveGhost.transform.GetChild(0).Find("Dust").gameObject.GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/DLC1/LunarSun/matLunarSunProjectileBackdrop.mat").WaitForCompletion();
            shockwaveGhost.transform.GetChild(0).gameObject.AddComponent<PaladinMod.Misc.StupidFuckingBullshit>();

            shockwaveGhost.DontPool();
            shockwave.GetComponent<ProjectileController>().ghostPrefab = shockwaveGhost;
            #endregion

            #region LunarShard
            lunarShard = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LunarShardProjectile"), "PaladinLunarShard", true);
            PaladinPlugin.Destroy(lunarShard.GetComponent<ProjectileSteerTowardTarget>());
            lunarShard.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = 1f;
            #endregion

            #region SunlightSpear
            lightningSpear = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile"), "LightningSpear", true);
            lightningSpear.AddComponent<Misc.ProjectileOverchargeOnImpact>();

            GameObject spearGhost = Asset.lightningSpear.InstantiateClone("LightningSpearGhost", false);
            spearGhost.AddComponent<ProjectileGhostController>();

            //vfx
            foreach (ParticleSystemRenderer i in spearGhost.GetComponentsInChildren<ParticleSystemRenderer>()) {
                if (i) i.trailMaterial = Modules.Asset.matYellowLightningLong;
            }
            Light light = spearGhost.GetComponentInChildren<Light>();
            light.range = 16f;
            light.intensity = 32f;
            spearGhost.GetComponentInChildren<TrailRenderer>().material = Modules.Asset.matYellowLightningLong;

            lightningSpear.transform.localScale *= 2f;

            lightningSpear.GetComponent<ProjectileController>().ghostPrefab = spearGhost;
            //lightningSpear.GetComponent<ProjectileOverlapAttack>().impactEffect = Assets.lightningImpactFX;
            lightningSpear.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;
            lightningSpear.GetComponent<ProjectileImpactExplosion>().impactEffect = Asset.altLightningImpactFX;
            lightningSpear.GetComponent<Rigidbody>().useGravity = false;

            PaladinPlugin.Destroy(lightningSpear.GetComponent<AntiGravityForce>());
            PaladinPlugin.Destroy(lightningSpear.GetComponent<ProjectileProximityBeamController>());
            #endregion

            #region SwordBeam
            swordBeamProjectile = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/FMJ"), "PaladinSwordBeam", true);
            swordBeamProjectile.transform.localScale = new Vector3(6, 3, 2);
            //GameObject beamGhost = Assets.swordBeam.InstantiateClone("SwordBeamGhost", false);
            //beamGhost.AddComponent<ProjectileGhostController>();

            //swordBeam.GetComponent<ProjectileController>().ghostPrefab = Assets.swordBeamGhost; 

            //GameObject MercEvisProjectileGhost = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab;
            swordBeamProjectile.GetComponent<ProjectileController>().ghostPrefab = Asset.swordBeamGhost;//MercEvisProjectileGhost;
            swordBeamProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            PaladinPlugin.Destroy(swordBeamProjectile.transform.Find("SweetSpotBehavior").gameObject);

            //run this in case moffein's phase round lightning is installed
            if (swordBeamProjectile.GetComponent<ProjectileProximityBeamController>()) PaladinPlugin.Destroy(swordBeamProjectile.GetComponent<ProjectileProximityBeamController>());

            swordBeamProjectile.AddComponent<DestroyOnTimer>().duration = 0.3f;

            #endregion

            #region Replenish
            heal = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinHeal", true);
            heal.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(heal.GetComponent<ProjectileDotZone>());

            heal.AddComponent<DestroyOnTimer>().duration = 5f;

            Misc.PaladinHealController healController = heal.AddComponent<Misc.PaladinHealController>();

            healController.radius = StaticValues.healRadius;
            healController.healAmount = StaticValues.healAmount;
            healController.barrierAmount = StaticValues.healBarrier;

            PaladinMod.PaladinPlugin.Destroy(heal.transform.GetChild(0).gameObject);
            GameObject healFX = Asset.healEffectPrefab.InstantiateClone("HealEffect", false);
            healFX.transform.parent = heal.transform;
            healFX.transform.localPosition = Vector3.zero;

            healFX.transform.localScale = Vector3.one * StaticValues.healRadius * 2f;
            #endregion

            #region SacredSunlight
            healZone = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinHealZone", true);
            healZone.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(healZone.GetComponent<ProjectileDotZone>());

            healZone.AddComponent<DestroyOnTimer>().duration = StaticValues.healZoneDuration + 2f;

            Misc.PaladinHealZoneController healZoneController = healZone.AddComponent<Misc.PaladinHealZoneController>();

            healZoneController.radius = StaticValues.healZoneRadius;
            healZoneController.interval = 0.25f;
            healZoneController.rangeIndicator = null;
            healZoneController.buffDuration = 0f;
            healZoneController.floorWard = false;
            healZoneController.expires = true;
            healZoneController.invertTeamFilter = false;
            healZoneController.expireDuration = StaticValues.healZoneDuration;
            healZoneController.animateRadius = false;
            healZoneController.healAmount = StaticValues.healZoneAmount;
            healZoneController.barrierAmount = StaticValues.healZoneBarrier;
            healZoneController.freezeProjectiles = false;

            PaladinMod.PaladinPlugin.Destroy(healZone.transform.GetChild(0).gameObject);
            GameObject healZoneFX = Asset.healZoneEffectPrefab.InstantiateClone("HealZoneEffect", false);
            healZoneFX.transform.parent = healZone.transform;
            healZoneFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(healZoneFX, StaticValues.healZoneRadius, StaticValues.healZoneDuration);
            #endregion

            #region ScepterSacredSunlight
            scepterHealZone = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinScepterHealZone", true);
            scepterHealZone.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(scepterHealZone.GetComponent<ProjectileDotZone>());

            scepterHealZone.AddComponent<DestroyOnTimer>().duration = StaticValues.scepterHealZoneDuration + 2f;

            Misc.PaladinHealZoneController scepterHealZoneController = scepterHealZone.AddComponent<Misc.PaladinHealZoneController>();

            scepterHealZoneController.radius = StaticValues.scepterHealZoneRadius;
            scepterHealZoneController.interval = 0.25f;
            scepterHealZoneController.rangeIndicator = null;
            scepterHealZoneController.buffDuration = 0f;
            scepterHealZoneController.floorWard = false;
            scepterHealZoneController.expires = true;
            scepterHealZoneController.invertTeamFilter = false;
            scepterHealZoneController.expireDuration = StaticValues.scepterHealZoneDuration;
            scepterHealZoneController.animateRadius = false;
            scepterHealZoneController.healAmount = StaticValues.scepterHealZoneAmount;
            scepterHealZoneController.barrierAmount = StaticValues.scepterHealZoneBarrier;
            scepterHealZoneController.freezeProjectiles = false;
            scepterHealZoneController.cleanseDebuffs = true;

            PaladinMod.PaladinPlugin.Destroy(scepterHealZone.transform.GetChild(0).gameObject);
            GameObject scepterHealZoneFX = Asset.healZoneEffectPrefab.InstantiateClone("ScepterHealZoneEffect", false);
            scepterHealZoneFX.transform.parent = scepterHealZone.transform;
            scepterHealZoneFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(scepterHealZoneFX, StaticValues.scepterHealZoneRadius, StaticValues.scepterHealZoneDuration);
            #endregion

            #region VowOfSilence
            torpor = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinTorpor", true);
            torpor.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(torpor.GetComponent<ProjectileDotZone>());

            torpor.AddComponent<DestroyOnTimer>().duration = StaticValues.torporDuration + 2f;

            Misc.PaladinHealZoneController torporController = torpor.AddComponent<Misc.PaladinHealZoneController>();

            torporController.radius = StaticValues.torporRadius;
            torporController.interval = 1f;
            torporController.rangeIndicator = null;
            torporController.buffDef = Buffs.torporDebuff;
            torporController.buffDuration = 1f;
            torporController.floorWard = false;
            torporController.expires = true;
            torporController.invertTeamFilter = true;
            torporController.expireDuration = StaticValues.torporDuration;
            torporController.animateRadius = false;
            torporController.healAmount = 0f;
            torporController.freezeProjectiles = false;
            torporController.grounding = true;

            PaladinMod.PaladinPlugin.Destroy(torpor.transform.GetChild(0).gameObject);
            GameObject torporFX = Asset.torporEffectPrefab.InstantiateClone("TorporEffect", false);
            torporFX.transform.parent = torpor.transform;
            torporFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(torporFX, StaticValues.torporRadius, StaticValues.torporDuration);
            #endregion

            #region ScepterVowOfSilence
            scepterTorpor = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinScepterTorpor", true);
            scepterTorpor.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(scepterTorpor.GetComponent<ProjectileDotZone>());

            scepterTorpor.AddComponent<DestroyOnTimer>().duration = StaticValues.scepterTorporDuration + 2f;

            Misc.PaladinHealZoneController scepterTorporController = scepterTorpor.AddComponent<Misc.PaladinHealZoneController>();

            scepterTorporController.radius = StaticValues.scepterTorporRadius;
            scepterTorporController.interval = 1f;
            scepterTorporController.rangeIndicator = null;
            scepterTorporController.buffDef = Buffs.scepterTorporDebuff;
            scepterTorporController.buffDuration = 1f;
            scepterTorporController.floorWard = false;
            scepterTorporController.expires = true;
            scepterTorporController.invertTeamFilter = true;
            scepterTorporController.expireDuration = StaticValues.scepterTorporDuration;
            scepterTorporController.animateRadius = false;
            scepterTorporController.healAmount = 0f;
            scepterTorporController.freezeProjectiles = true;
            scepterTorporController.grounding = true;

            PaladinMod.PaladinPlugin.Destroy(scepterTorpor.transform.GetChild(0).gameObject);
            GameObject scepterTorporFX = Asset.torporEffectPrefab.InstantiateClone("ScepterTorporEffect", false);
            scepterTorporFX.transform.parent = scepterTorpor.transform;
            scepterTorporFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(scepterTorporFX, StaticValues.scepterTorporRadius, StaticValues.scepterTorporDuration);
            #endregion

            #region SacredOath
            warcry = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinWarcry", true);
            warcry.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(warcry.GetComponent<ProjectileDotZone>());

            warcry.AddComponent<DestroyOnTimer>().duration = StaticValues.warcryDuration + 2f;

            BuffWard warcryController = warcry.AddComponent<BuffWard>();

            warcryController.radius = StaticValues.warcryRadius;
            warcryController.interval = 0.25f;
            warcryController.rangeIndicator = null;
            warcryController.buffDef = Buffs.warcryBuff;
            warcryController.buffDuration = 2f;
            warcryController.floorWard = false;
            warcryController.expires = true;
            warcryController.invertTeamFilter = false;
            warcryController.expireDuration = StaticValues.warcryDuration;
            warcryController.animateRadius = false;

            PaladinMod.PaladinPlugin.Destroy(warcry.transform.GetChild(0).gameObject);
            GameObject warcryFX = Asset.warcryEffectPrefab.InstantiateClone("WarcryEffect", false);
            warcryFX.transform.parent = warcry.transform;
            warcryFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(warcryFX, StaticValues.warcryRadius, StaticValues.warcryDuration);
            #endregion

            #region ScepterSacredOath
            scepterWarcry = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinScepterWarcry", true);
            scepterWarcry.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(scepterWarcry.GetComponent<ProjectileDotZone>());

            scepterWarcry.AddComponent<DestroyOnTimer>().duration = StaticValues.scepterWarcryDuration + 2f;

            BuffWard scepterWarcryController = scepterWarcry.AddComponent<BuffWard>();

            scepterWarcryController.radius = StaticValues.scepterWarcryRadius;
            scepterWarcryController.interval = 0.25f;
            scepterWarcryController.rangeIndicator = null;
            scepterWarcryController.buffDef = Buffs.scepterWarcryBuff;
            scepterWarcryController.buffDuration = 1f;
            scepterWarcryController.floorWard = false;
            scepterWarcryController.expires = true;
            scepterWarcryController.invertTeamFilter = false;
            scepterWarcryController.expireDuration = StaticValues.scepterWarcryDuration;
            scepterWarcryController.animateRadius = false;

            PaladinMod.PaladinPlugin.Destroy(scepterWarcry.transform.GetChild(0).gameObject);
            GameObject scepterWarcryFX = Asset.warcryEffectPrefab.InstantiateClone("ScepterWarcryEffect", false);
            scepterWarcryFX.transform.parent = scepterWarcry.transform;
            scepterWarcryFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(scepterWarcryFX, StaticValues.scepterWarcryRadius, StaticValues.scepterWarcryDuration);
            #endregion

            #region PrideFlare
            //Ghost
            scepterCruelSunGhost = PrefabAPI.InstantiateClone(Asset.paladinScepterSunPrefab, "PaladinScepterSunProjectileGhost");

            Object.DestroyImmediate(scepterCruelSunGhost.GetComponent<EntityStateMachine>());
            Object.DestroyImmediate(scepterCruelSunGhost.GetComponent<NetworkStateMachine>());
            Object.DestroyImmediate(scepterCruelSunGhost.GetComponent<PaladinSunController>());
            scepterCruelSunGhost.AddComponent<ProjectileGhostController>();
            VFXAttributes scsgVFXA = scepterCruelSunGhost.AddComponent<VFXAttributes>();
            scsgVFXA.vfxPriority = VFXAttributes.VFXPriority.Always;
            scsgVFXA.vfxIntensity = VFXAttributes.VFXIntensity.High;

            //scepterCruelSunGhost.AddComponent<DetachParticleOnDestroyAndEndEmission>().particleSystem = ;

            //Projectile
            scepterCruelSun = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion(), "PaladinScepterSunProjectile");

            Object.Destroy(scepterCruelSun.GetComponent<ProjectileProximityBeamController>());
            Object.Destroy(scepterCruelSun.GetComponent<AkEvent>());

            ProjectileController scsPC = scepterCruelSun.GetComponent<ProjectileController>();
            scsPC.ghostPrefab = scepterCruelSunGhost;
            scsPC.canImpactOnTrigger = false;
            scsPC.cannotBeDeleted = true;
            scsPC.procCoefficient = StaticValues.prideFlareProcCoefficient;

            scepterCruelSun.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            scepterCruelSun.GetComponent<ProjectileSimple>().desiredForwardSpeed = StaticValues.prideFlareSpeed;

            ProjectileImpactExplosion scsPE = scepterCruelSun.GetComponent<ProjectileImpactExplosion>();
            scsPE.blastAttackerFiltering = AttackerFiltering.AlwaysHit;
            scsPE.blastDamageCoefficient = StaticValues.prideFlareDamageCoefficient * Config.prideFlareMultiplier.Value;
            scsPE.blastProcCoefficient = StaticValues.prideFlareProcCoefficient;
            scsPE.blastRadius = StaticValues.prideFlareExplosionRadius;
            scsPE.falloffModel = BlastAttack.FalloffModel.Linear;
            scsPE.canRejectForce = false;
            scsPE.destroyOnEnemy = false;
            scsPE.impactEffect = Asset.paladinSunSpawnPrefab;

            scepterCruelSun.AddComponent<AlignToNormal>();
            scepterCruelSun.GetComponent<AntiGravityForce>().antiGravityCoefficient = 0.95f;

            //maybe: delete meshfilter?

            //Transfering over some data we need to keep the burn AoE going.
            PaladinSunController baseScript = Asset.paladinSunPrefab.GetComponent<PaladinSunController>();
            PaladinSunController newScript = scepterCruelSun.AddComponent<PaladinSunController>();
            newScript.buffApplyEffect = baseScript.buffApplyEffect;
            newScript.buffDef = baseScript.buffDef;
            newScript.activeLoopDef = baseScript.activeLoopDef;
            newScript.damageLoopDef = baseScript.damageLoopDef;
            newScript.stopSoundName = baseScript.stopSoundName;
            #endregion

            Modules.Prefabs.projectilePrefabs = new List<GameObject>
            {
                swordBeamProjectile,
                shockwave,
                lightningSpear,
                lunarShard,
                heal,
                healZone,
                scepterHealZone,
                torpor,
                scepterTorpor,
                warcry,
                scepterWarcry,
                scepterCruelSun
            };
        }
        /// <summary>
        /// To add a custom colored sword beam for your skin
        /// <para> create a projectileGhostReplacement in your skindef, and you can use this to easily get a new ProjectileGhost variation of Paladin's sword beam </para>
        /// </summary>
        /// <param name="beamColor"></param>
        /// <param name="lightBright"></param>
        /// <returns></returns>
        public static GameObject CloneAndColorSwordBeam(Color beamColor, float lightBright = 0.8f) {

            GameObject MercEvisProjectileGhost = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab;

            return CloneAndColorGhost(MercEvisProjectileGhost, beamColor, lightBright);
        }

        public static GameObject CloneAndColorGhost(GameObject projectileGhost, Color beamColor, float lightBright = 0.8f) {

            GameObject evisGhostNew = PrefabAPI.InstantiateClone(projectileGhost, "EvisProjectileClone", false);

            foreach (ParticleSystemRenderer i in evisGhostNew.GetComponentsInChildren<ParticleSystemRenderer>()) { 
                if (i) {
                    Material mat = UnityEngine.Object.Instantiate<Material>(i.material);
                    mat.SetColor("_TintColor", beamColor); 
                    i.material = mat;
                }
            }

            evisGhostNew.GetComponentInChildren<Light>().color = beamColor * lightBright;

            return evisGhostNew;
        }

        public static void InitSpellEffect(GameObject target, float radius, float duration)
        {
            target.transform.localScale = Vector3.one * radius * 2f;

            foreach(ParticleSystem i in target.GetComponentsInChildren<ParticleSystem>())
            {
                var particleSystem = i.main;
                particleSystem.startLifetime = duration;
            }
        }
    }
}
