namespace EffectSourceConditions
{
    public class SourceConditionStatusEffect : SourceCondition
    {
        /// <summary>
        /// The skill ID that a character must have learned for CharacterHasRequirement to return true
        /// </summary>
        public string statusEffectName;
        
        /// <summary>
        /// Set to true to require the character to not have the relevant skill
        /// </summary>
        public bool Inverted = false;

        /// <summary>
        /// Assigns the ID of the provided Skill to RequiredSkillID
        /// </summary>
        public StatusEffect RequiredStatusEffect
        {
            get
            {
                return ResourcesPrefabManager.Instance.GetStatusEffectPrefab(statusEffectName);
            }
            set
            {
                statusEffectName = value.IdentifierName;
            }
        }

        /// <summary>
        /// Returns true if RequiredSkillID <= 0 or if the character knows the Skill with ItemID = RequiredSkillID
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public override bool CharacterHasRequirement(Character character)
        {
            return (statusEffectName != null && (character?.StatusEffectMngr?.HasStatusEffect(statusEffectName) ?? false)) ^ Inverted;
        }
    }
}
