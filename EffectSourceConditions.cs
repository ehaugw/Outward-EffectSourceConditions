using HarmonyLib;

namespace EffectSourceConditions
{
    using System.Collections.Generic;
    using UnityEngine;
    using BepInEx;
    using System;
    using System.Linq;
    using System.IO;

    [BepInPlugin(GUID, NAME, VERSION)]
    public class EffectSourceConditions : BaseUnityPlugin
    {
        public const string GUID = "com.ehaugw.effectsourceconditions";
        public const string VERSION = "1.1.0";
        public const string NAME = "Effect Source Conditions";

        internal void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        public const string EFFECTS_CONTAINER = "Effects";
        public const string SOURCE_CONDITION_CONTAINER = "SourceConditions";
        public const string EFFECTS_MANUAL_CONTAINER = "ManualContainer";

        public static bool HasSourceConditions(Effect __instance)
        {
            foreach (SourceCondition req in __instance?.transform?.Find(SOURCE_CONDITION_CONTAINER)?.gameObject?.GetComponents<SourceCondition>() ?? new SourceCondition[] { })
            {
                if (!req.CharacterHasRequirement(__instance.SourceCharacter))
                {
                    return false; //don't call original function
                }
            }
            return true;//call the original function
        }

        [HarmonyPatch(typeof(WeaponDamage), "BuildDamage", new Type[] { typeof(Character), typeof(DamageList), typeof(float) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Ref })]
        public class WeaponDamage_BuildDamage
        {
            [HarmonyPrefix]
            public static bool Prefix(Effect __instance, Character _targetCharacter, ref DamageList _list, ref float _knockback)
            {
                return HasSourceConditions(__instance);
            }
        }

        [HarmonyPatch(typeof(PunctualDamage), "BuildDamage", new Type[] { typeof(Character), typeof(DamageList), typeof(float) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Ref })]
        public class PunctualDamage_BuildDamage
        {
            [HarmonyPrefix]
            public static bool Prefix(Effect __instance, Character _targetCharacter, ref DamageList _list, ref float _knockback)
            {
                return HasSourceConditions(__instance);
            }
        }

        [HarmonyPatch(typeof(Effect), "TryTriggerConditions", new Type[] { typeof(Character), typeof(bool) })]
        public class Effect_TryTriggerConditions
        {
            [HarmonyPrefix]
            public static bool Prefix(Effect __instance, Character _affectedCharacter, bool _skipPriority)
            {
                return HasSourceConditions(__instance);
            }
        }

    }
}