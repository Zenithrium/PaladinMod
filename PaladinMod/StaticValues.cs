﻿namespace PaladinMod
{
    class StaticValues
    {
        //
        public const string characterName = "Paladin";
        public const string characterSubtitle = "Acolyte of Providence";
        public const string characterOutro = "..and so he left, faith in his doctrine shaken.";
        public const string characterOutroFailure = "..and so he vanished, his prayers unheard.";
        public const string characterLore =
            "\"You will go to Petrichor V.  This planet has been selected for your pilgrimage, the punishment for the sins committed towards your people<s>, K        </s>.\"\n\n"
            + "I stared ahead, stripped of my armor and my sword.  My crown, shattered.  My obsidian-stone carapace, damaged and torn.  A small, starry gemstone embedded in my right palm, a remnant of my past as king.  They keep me bound and push me towards a ship, end of the line, my sacrifice for the good of my brothers and sisters.\n\n"
            + "I did it to save my people, even if they do not accept such niceties.  I would not fall into their habits of never-ending war and suffering.  I tried to change them for the betterment of themselves, but I learned the hard way that traditionalism is something that one alone cannot change.\n\n"
            + "So be it.  Perhaps my message will live on in the minds of those who tried to defend me.  Maybe my actions will mean something, maybe they will result in nothing.  I do not care for my own fate, I only hope that my people will learn from their pasts and improve themselves.\n\n"
            + "For the sake of my people, for the sake of our history, for the sake of longevity...\n\n"
            + "To the stars I go... to this planet of hushed rumors.";

        public const string oldLore = "\nAlpha.  Gaze ahead.  He stands tall.  Similar face.  Blade like the sky, but not violent.  Peaceful yet powerful.  Protector.  Bulwark.  <color=#97d362>Friend.</color>\n\n"
            + "Me.  Frail.  Not worthy.  Yet he gave me opportunity.  Saw potential.  Imbued me with something.  Strength.  Power.  Understanding.   <color=#390b6d>M o n s t e r .</color>\n\n"
            + "Right hand.  Protectors alongside.  Other monsters.  None resemble me, or the Alpha.  Weapons were bestowed.  Hammer.  Axe.  Sword.  And more.  There was one other, however.  They looked off.  Red cape.  Bird-like.  Enemy.  Enemy?  She was the 13th Guardian.  The Alpha did not trust her, but the Omega insisted.  The Omega.  Brother.  Uncomfortable.  I do not trust him at all.\n\n"
            + "Loud.  A rumbling in the sky.  Massive flying metal beast.  A ship.  Off-world beings.  Landing violently.  They are similar in shape to some of us, yet different to others.  Mixture of flesh and machine.  Full flesh were some.  Full machine were few.\n\n"
            + "Them.  Non-hostile, but confused.  The Alpha explains they are confused.  Acting out of instinct to survive.  Something is wrong when I look at the Alpha.  He looks like he’s guilty of something.  Did he cause the crash?  Why were they nearby the planet?\n\n"
            + "A truth.  The Alpha had left his throne.  His blade trailed behind him.  We follow.  I ask about the ship.  He explains his doing.  It was influenced by the Omega.  Told to attack to protect us.  The Alpha looked saddened to have to hold his blade.  Told us to leave him and protect the others.\n\n"
            + "Destruction.  All over.  The dead are many.  I cannot bring them back.  Why?  I have been able to heal.  Why can’t I.  Frustration.  Eight of the Protectors are dead.  Four remain.  One is missing.  I do not know where they are.  I am afraid, but I will stay strong.\n\n"
            + "Running.  The Alpha is aboard the remains of the vessel.  His new throne.  I want to be there.  We are his guardians, why would he go alone?  Was he told by the Omega?  Hatred.  The Omega is the cause of our faults.  He wants control.  Will not let him have.  Why did the Omega know so much?  Was it the Thirteenth?   <color=#390b6d>H e r e t i c .</color>\n\n"
            + "A battle.  I failed.  I could not get to the Alpha in time.  The Orange one made it first, covered like a magnet of power.  Lightning flairs from him.  Strange devices stabbed into him.  He does not slow.  He moves faster than anything I’ve seen.  Devices in his hands sound like explosions.  They fire fast.  The Alpha falls.  The Worms were obliterated.  The ship was going to explode.  He is gone.  As if removed from his spot and taken to safety in a flash.  I rush in.  Protect the Alpha.  Heal him.  I have to.  I will.\n\n"
            + "Fallen.  The Alpha does not stir.  Power is useless.  Sword is shattered.  The ship is going to explode.  Countdown.  Final moments.  Why is this happening?  Why did this have to happen?  Was it planned?  Omega.  Omega.  His fault.  His fault.  Anger.  Rage.  Hate.   <color=#390b6d>R e v e n g e .</color>\n\n"
            + "Silence.  Bubble.  A shield.  The Alpha is alive but barely.  The ship had detonated.  The Alpha saved me with the last of his power.  No words.  Nothing more after.  Kept me alive.  Why?  Could he not heal himself?  Was this him bestowing his last remnants to me?  No answers.  New goal.  I picked up the snapped blade of the Alpha.  I began scouring the remains of the ship and took his cloak.  I will not falter.  I will move on.  I will revive him through having his will act through me.  The Orange One and his cohorts of sinners are burned into my mind.  They shall fall by my hand too.\n\n"
            + "I am his last.  His final message.  His wrath.  And yet, I feel guilty.  I feel wrong.  Is this what the Alpha would have wanted?  Or is this  <color=#390b6d>i n s t i n c t ?</color>  Must repress.  Hold back.  We are to not forget ourselves.  Continue with the mission.  The Omega must fall.  The Omega will fall.  The Moon is a harbinger of death and lies.\n\n\n"
            + "I am coming for you, <color=#dc0000>Mithrix.</color>";

        //Misc
        public const float maxSwordGlow = 3f;
        public const float swordGlowSpeed = 8f;

        //Base stats
        public const float baseDamage = 12f;
        public const float baseDamagePerLevel = baseDamage * 0.2f;
        public const float armorPerLevel = 1f;
        public const float regenAmount = 3f;

        //Divine Blade
        public const float slashDamageCoefficient = 3f;

        public const float beamDamageCoefficient = 3f;
        public const float beamSpeed = 160f;

        //Accursed Blade
        public const float cursedBladeDamageCoefficient = 3.8f;
        public const float cursedBladeHealCoefficient = 0.05f;

        //Spinning Slash
        public const float spinSlashDamageCoefficient = 7.5f;

        //Sunlight Spear
        public const float lightningSpearMinDamageCoefficient = 3f;
        public const float lightningSpearMaxDamageCoefficient = 7f;
        public const float lightningSpearChargeTime = 1.25f;

        //Lunar Shards
        public const float lunarShardDamageCoefficient = 0.75f;
        public const int lunarShardMaxStock = 12;

        //Quickstep
        public const float dashBarrierAmount = 0.15f;

        //Replenish
        public const float healRadius = 16f;
        public const float healAmount = 0.15f;
        public const float healBarrier = 0.15f;

        //Sacred Sunlight
        public const float healZoneChannelDuration = 1.5f;
        public const float healZoneRadius = 16f;
        public const float healZoneAmount = 0.01f;
        public const float healZoneBarrier = 0.01f;
        public const float healZoneDuration = 12f;

        //Hallowed Sunlight
        public const float scepterHealZoneChannelDuration = 1.5f;
        public const float scepterHealZoneRadius = 16f;
        public const float scepterHealZoneAmount = 0.02f;
        public const float scepterHealZoneBarrier = 0.02f;
        public const float scepterHealZoneDuration = 12f;

        //Vow of Silence
        public const float torporChannelDuration = 2f;
        public const float torporRadius = 24f;
        public const float torporSlowAmount = 0.6f;
        public const float torporDuration = 10;

        //Oath of Silence
        public const float scepterTorporChannelDuration = 2f;
        public const float scepterTorporRadius = 32f;
        public const float scepterTorporSlowAmount = 0.8f;
        public const float scepterTorporDuration = 10;

        //Sacred Oath
        public const float warcryChannelDuration = 2f;
        public const float warcryRadius = 20f;
        public const float warcryDamageMultiplier = 0.25f;
        public const float warcryAttackSpeedBuff = 1;
        public const float warcryDuration = 8;

        //Sacred Oath(Scepter)
        public const float scepterWarcryChannelDuration = 1f;
        public const float scepterWarcryRadius = 20f;
        public const float scepterWarcryDamageMultiplier = 0.5f;
        public const float scepterWarcryAttackSpeedBuff = 2f;
        public const float scepterWarcryDuration = 8;

        //Cruel Sun
        public const float cruelSunChannelDurationOld = 5f;
        public const float cruelSunChannelDuration = 2.0f;
        public const float cruelSunDuration = 12.5f;
        public static float cruelSunVfxSize = 0.37f;
        public static float cruelSunVfxCenterSize = 9.5f;
        public static float cruelSunVfxLightIntensity = 0.225f;
        public static float cruelSunAoE = 70f;
        public const float cruelSunCycleInterval = 0.5f;
        public const float cruelSunOverheatDuration = 1f;
        public const int cruelSunMinimumStacksBeforeApplyingBurns = 2;
        public const int cruelSunMaximumAllyStacks = 2;
        public const float cruelSunBurnDamageCoefficient = 1.6f;
        public static float cruelSunAllyDamageMultiplier = 0.15f;

        //Pride Flare(Cruel Sun Scepter)
        public static float prideFlareExplosionRadius = cruelSunAoE;
        public static float prideFlareMaxIndicatorRange = cruelSunAoE * 2f;
        public const float prideFlareDamageCoefficient = 0.01f; //multiplied by config value 9001
        public const float prideFlareProcCoefficient = 1f;
        public const float prideFlareForce = 400f;
        public const float prideFlareSelfForce = 10f;
        public static float prideFlareSpeed = cruelSunAoE * 0.4f;
        public const float prideFlareAimTimeMax = 3f;
    }
}