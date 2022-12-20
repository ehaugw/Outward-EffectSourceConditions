using HarmonyLib;
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

        //public int ?ItemID = null;

        public void SetSkillStats(Skill skill) {
            skill.ManaCost= ManaCost;
            skill.StaminaCost= StaminaCost;
            skill.HealthCost= HealthCost;
            skill.DurabilityCost= DurabilityCost;
            if (!skill.InCooldown())
               skill.Cooldown= Cooldown;
        }

        public static void UnsetSkillStats(Skill skill)
        {
            skill.ManaCost= 0;
            skill.StaminaCost= 0;
            skill.HealthCost= 0;
            skill.DurabilityCost= 0;

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
                        foreach (var sourceCondition in sourceConditions)
                        {
                            if (sourceCondition.CharacterHasRequirement(character))
                            {
                                dynamicStat.SetSkillStats(skill);
                                updated = true;
                            }
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
