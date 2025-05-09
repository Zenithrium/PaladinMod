﻿using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using UnityEngine.Networking;
using RoR2.Projectile;

namespace PaladinMod.States
{
    public class AirSlam : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.spinSlashDamageCoefficient;
        public static float leapDuration = 0.6f;
        public static float dropVelocity = 18f;

        private float duration;
        private bool hasFired;
        private bool hasLanded;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private PaladinSwordController swordController;
        private float previousAirControl;
        private bool hasHit;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = AirSlam.leapDuration / (0.75f + (0.25f * this.attackSpeedStat));
            this.hasFired = false;
            this.hasLanded = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            base.characterMotor.jumpCount = base.characterBody.maxJumpCount;

            this.previousAirControl = base.characterMotor.airControl;
            base.characterMotor.airControl = EntityStates.Croco.Leap.airControl;

            Vector3 direction = base.GetAimRay().direction;

            if (PaladinPlugin.IsLocalVRPlayer(characterBody)) {
                direction = Camera.main.transform.forward;
            }

            if (base.isAuthority)
            {
                base.characterBody.isSprinting = true;

                direction.y = Mathf.Max(direction.y, 1.25f * EntityStates.Croco.Leap.minimumY);
                Vector3 a = direction.normalized * (0.75f * EntityStates.Croco.Leap.aimVelocity) * this.moveSpeedStat;
                Vector3 b = Vector3.up * 0.95f * EntityStates.Croco.Leap.upwardVelocity;
                Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * (1.5f * EntityStates.Croco.Leap.forwardVelocity);

                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.velocity = a + b + b2;

                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BoostJumpEffect"), new EffectData
                {
                    origin = base.characterBody.footPosition,
                    rotation = Util.QuaternionSafeLookRotation(base.characterMotor.velocity)
                }, true);
            }

            base.characterDirection.moveVector = direction;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            string hitboxString = "LeapStrike";

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxString);
            }

            this.swordController.airSlamStacks = 1;

            base.PlayAnimation("FullBody, Override", "LeapSlam", "Whirlwind.playbackRate", this.duration * 1.5f);
            Util.PlaySound(Modules.Sounds.Lunge, base.gameObject);
            Util.PlaySound(Modules.Sounds.Cloth2, base.gameObject);

            float dmg = AirSlam.damageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Stun1s;
            this.attack.damageType.damageSource = DamageSource.Secondary;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = this.swordController.hitEffect;
            this.attack.forceVector = -Vector3.up * 6000f;
            this.attack.pushAwayForce = 500f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.impactSound = Modules.Asset.swordHitSoundEventL.index;
            if (this.swordController.isBlunt) this.attack.impactSound = Modules.Asset.batHitSoundEventL.index;
        }

        public override void OnExit()
        {
            base.OnExit();

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            base.characterMotor.airControl = this.previousAirControl;
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                this.swordController.PlaySwingSound();

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * GroundSweep.attackRecoil, -2f * GroundSweep.attackRecoil, -0.5f * GroundSweep.attackRecoil, 0.5f * GroundSweep.attackRecoil);
                    //EffectManager.SimpleMuzzleFlash(this.swordController.swingEffect, base.gameObject, "SwingDown", true);
                    UnityEngine.Object.Instantiate(this.swordController.swingEffect, FindModelChild("SwingDown"));

                    base.characterMotor.velocity *= 0.25f;
                    base.characterMotor.velocity += Vector3.up * -AirSlam.dropVelocity;
                }
            }

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                if (this.attack.Fire())
                {
                    if (!this.inHitPause)
                    {
                        this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Whirlwind.playbackRate");
                        this.hitPauseTimer = (4f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                        this.inHitPause = true;
                    }

                    if (!this.hasHit)
                    {
                        this.hasHit = true;
                        if (base.skillLocator.utility.skillDef.skillNameToken == "PALADIN_UTILITY_DASH_NAME") base.skillLocator.utility.RunRecharge(1f);
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.StartAimMode(0.5f, false);
            this.stopwatch += Time.deltaTime;

            if (this.stopwatch >= this.duration)
            {
                this.FireAttack();

                if (base.isAuthority && base.inputBank.skill2.down)
                {
                    if (base.skillLocator.secondary.stock > 0)
                    {
                        EntityState nextState = new AirSlamAlt();
                        this.outer.SetNextState(nextState);
                        return;
                    }
                }
            }

            if (this.stopwatch >= this.duration && base.isAuthority && base.characterMotor.isGrounded)
            {
                this.GroundImpact();
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void FireShockwave()
        {
            Vector3 shockwavePosition = base.characterBody.footPosition;
            Vector3 forward = base.characterDirection.forward;

            ProjectileManager.instance.FireProjectile(
                Modules.Projectiles.shockwave, 
                shockwavePosition, 
                Util.QuaternionSafeLookRotation(forward),
                base.gameObject,
                base.characterBody.damage * StaticValues.beamDamageCoefficient,
                EntityStates.BrotherMonster.WeaponSlam.waveProjectileForce, 
                base.RollCrit(),
                DamageColorIndex.Default,
                null,
                -1f,
                DamageTypeCombo.GenericSecondary);
        }

        private void GroundImpact()
        {
            if (!this.hasLanded)
            {
                this.hasLanded = true;

                if (this.swordController && this.swordController.swordActive)
                {
                    this.FireShockwave();

                    EffectManager.SpawnEffect(Modules.Asset.airSlamBoostedFX, new EffectData
                    {
                        origin = this.characterBody.footPosition + (this.characterDirection.forward * 2.5f),
                        scale = 1f
                    }, true);
                }
                else
                {
                    EffectManager.SpawnEffect(Modules.Asset.airSlamFX, new EffectData
                    {
                        origin = this.characterBody.footPosition + (this.characterDirection.forward * 2.5f),
                        scale = 1f
                    }, true);
                }

                Util.PlaySound(Modules.Sounds.GroundImpact, base.gameObject);
                Util.PlaySound(Modules.Sounds.LeapSlam, base.gameObject);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}