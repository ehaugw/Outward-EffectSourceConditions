﻿using HarmonyLib;
using TinyHelper;
using UnityEngine;
using static MapMagic.ObjectPool;

namespace EffectSourceConditions
{
    public class DynamicSkillStat : UnityEngine.MonoBehaviour
    {
        public float ManaCost = 0;
        public float StaminaCost = 0;
        public float HealthCost = 0;
        public float Cooldown = 2;
        public float DurabilityCost = 0;

        public Character.SpellCastType CastType = Character.SpellCastType.NONE;
        public Character.SpellCastModifier CastModifier = Character.SpellCastModifier.NONE;
        public float MobileCastMovementMult = -1;
        public int CastSheatheRequired = 0;
        public bool CastLocomotionEnabled = false;

        public delegate void ManaCostModifier(Skill skill, float original, ref float result);
        public static ManaCostModifier ManaCostModifiers = delegate (Skill skill, float original, ref float result) { };

        public void SetSkillStats(Skill skill) {

            float effectiveManaCost = ManaCost;
            ManaCostModifiers(skill, ManaCost, ref effectiveManaCost);

            skill.ManaCost = Mathf.Round(effectiveManaCost);
            skill.StaminaCost= StaminaCost;
            skill.HealthCost= HealthCost;
            skill.DurabilityCost= DurabilityCost;

            skill.CastSheathRequired = CastSheatheRequired;
            skill.MobileCastMovementMult = MobileCastMovementMult;
            skill.CastModifier = CastModifier;
            skill.CastLocomotionEnabled = CastLocomotionEnabled;
            if (skill.ActivateEffectAnimType != CastType)
                At.SetValue(CastType, typeof(Item), skill, "m_activateEffectAnimType");
            
            if (!skill.InCooldown())
               skill.Cooldown= Cooldown;
        }

        public static void UnsetSkillStats(Skill skill)
        {
            skill.ManaCost= 0;
            skill.StaminaCost= 0;
            skill.HealthCost= 0;
            skill.DurabilityCost= 0;

            skill.CastSheathRequired = 0;
            skill.MobileCastMovementMult = -1;
            skill.CastModifier = Character.SpellCastModifier.Immobilized;
            skill.CastLocomotionEnabled = false;
            if (skill.ActivateEffectAnimType != Character.SpellCastType.NONE)
                At.SetValue(Character.SpellCastType.NONE, typeof(Item), skill, "m_activateEffectAnimType");
            
            if (!skill.InCooldown())
                skill.Cooldown = 0;
        }
        
        public static void TryUpdateSkillStats(Skill skill)
        {
            if (skill.gameObject.GetComponentsInChildren<DynamicSkillStat>() is DynamicSkillStat[] dynamicStats && dynamicStats.Length > 0)
            {
                bool updated = false;

                foreach (var dynamicStat in dynamicStats)
                {
                    if (skill.OwnerCharacter is Character character && dynamicStat.gameObject.GetComponents<SourceCondition>() is SourceCondition[] sourceConditions)
                    {
                        bool result = true;
                        foreach (var sourceCondition in sourceConditions)
                        {
                            if (!sourceCondition.CharacterHasRequirement(character))
                            {
                                result = false;
                            }
                        }
                        if (result && sourceConditions.Length > 0)
                        {
                            dynamicStat.SetSkillStats(skill);
                            updated = true;
                        }
                    }
                }
                if (!updated)
                {
                    DynamicSkillStat.UnsetSkillStats(skill);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Item), "Description", MethodType.Getter)]
    //[HarmonyAfter(new string[] {"com.sinai.sideloader"})]
    public class Item_Description
    {
        [HarmonyPrefix]
        public static void Prefix(Item __instance)
        {
            if (__instance is Skill skill)
            {
                DynamicSkillStat.TryUpdateSkillStats(skill);
            }
        }
    }

    [HarmonyPatch(typeof(Skill), "HasBaseRequirements")]
    public class Skill_HasBaseRequirements
    {
        [HarmonyPrefix]
        public static void Prefix(Skill __instance)
        {
            DynamicSkillStat.TryUpdateSkillStats(__instance);
        }
    }
}
