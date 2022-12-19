using HarmonyLib;

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
    }

    [HarmonyPatch(typeof(Skill), "HasBaseRequirements")]
    public class Skill_HasBaseRequirements
    {
        [HarmonyPrefix]
        public static void Prefix(Skill __instance, bool _tryingToActivate/*, out bool __result*/)
        {

            if (__instance.gameObject.GetComponentsInChildren<DynamicSkillStat>() is DynamicSkillStat[] dynamicStats && dynamicStats.Length > 0)
            {
                bool updated = false;

                foreach (var dynamicStat in dynamicStats)
                {
                    if (__instance.OwnerCharacter is Character character && dynamicStat.gameObject.GetComponents<SourceCondition>() is SourceCondition[] sourceConditions)
                    {
                        foreach (var sourceCondition in sourceConditions)
                        {
                            if (sourceCondition.CharacterHasRequirement(character))
                            {
                                dynamicStat.SetSkillStats(__instance);
                                updated = true;
                            }
                        }
                    }
                }
                if (!updated)
                {
                    DynamicSkillStat.UnsetSkillStats(__instance);
                }
            }
        }
    }
}
