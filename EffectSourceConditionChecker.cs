using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyHelper;
using UnityEngine;

namespace EffectSourceConditions
{
    public class EffectSourceConditionChecker : EffectCondition
    {
        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            if (transform?.parent?.parent?.FindAllInAllChildren("ActivationEffects") is Transform[] activationEffectTransforms)
            {
                foreach (var transform in activationEffectTransforms)
                {
                    bool result = true;
                    var components = transform.GetComponentsInChildren<SourceCondition>();
                    foreach (var sourceCondition in components)
                    {
                        if (!sourceCondition.CharacterHasRequirement(_affectedCharacter))
                        {
                            result = false;
                        }
                    }
                    if (result && components.Count() > 0)
                    {
                        return !Invert;
                    }
                }
            }
            return Invert;
        }

        public static Skill.ActivationCondition AddToSkill(Skill skill)
        {
            var activationConditions = TinyGameObjectManager.GetOrMake(skill.transform, "AdditionalActivationConditions", true, true);
            var gameObj = TinyGameObjectManager.GetOrMake(activationConditions, "EffectSourceConditionChecker", true, true).gameObject;

            var condition = new Skill.ActivationCondition();
            var myCondition = gameObj.AddComponent<EffectSourceConditionChecker>();
            //gameObj.SetActive(false);

            condition.Condition = myCondition;
            
            At.SetValue("This would have no effect.", typeof(Skill.ActivationCondition), condition, "m_defaultMessage");

            List<Skill.ActivationCondition> conditions = ((At.GetValue(typeof(Skill), skill, "m_additionalConditions") as Skill.ActivationCondition[])?.ToList()) ?? new List<Skill.ActivationCondition>();
            conditions.Add(condition);
            At.SetValue(conditions.ToArray(), typeof(Skill), skill, "m_additionalConditions");
            return condition;
        }
    }
}
