﻿using PaladinMod.Misc;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ScepterChannelCruelSunOld : BaseChannelSpellState
    {
        private GameObject chargeEffect;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = Modules.Sounds.ChannelTorpor;
            this.maxSpellRadius = ScepterCastCruelSunOld.sunPrefabDiameter * 0.5f;
            this.baseDuration = StaticValues.cruelSunChannelDurationOld;
            this.swordController = base.gameObject.GetComponent<PaladinSwordController>();
            this.overrideAreaIndicatorMat = Modules.Asset.areaIndicatorMat;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("ScepterCruelSunChannelEffect").gameObject;
                this.chargeEffect.SetActive(false);
                this.chargeEffect.SetActive(true);
            }
        }

        protected override void PlayChannelAnimation()
        {
            base.PlayCrossfade("Gesture, Override", "ChannelSun", "Spell.playbackRate", 2.5f, 0.1f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.swordController) this.swordController.sunPosition = this.areaIndicatorInstance.transform.position;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.chargeEffect)
            {
                this.chargeEffect.GetComponentInChildren<ParticleSystem>().Stop();
            }
        }

        protected override BaseCastChanneledSpellState GetNextState()
        {
            return new ScepterCastCruelSunOld();
        }
    }
}