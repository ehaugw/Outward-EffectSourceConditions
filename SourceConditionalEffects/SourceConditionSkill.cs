namespace EffectSourceConditions
{
    public class SourceConditionSkill : SourceCondition
    {
        /// <summary>
        /// The skill ID that a character must have learned for CharacterHasRequirement to return true
        /// </summary>
        public int RequiredSkillID;
        
        /// <summary>
        /// Set to true to require the character to not have the relevant skill
        /// </summary>
        public bool Inverted = false;

        /// <summary>
        /// Assigns the ID of the provided Skill to RequiredSkillID
        /// </summary>
        public Item RequiredSkill
        {
            get
            {
                return ResourcesPrefabManager.Instance.GetItemPrefab(RequiredSkillID);
            }
            set
            {
                RequiredSkillID = value.ItemID;
            }
        }

        /// <summary>
        /// Returns true if RequiredSkillID <= 0 or if the character knows the Skill with ItemID = RequiredSkillID
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public override bool CharacterHasRequirement(Character character)
        {
            return ((RequiredSkillID <= 0) || ((character?.Inventory?.SkillKnowledge?.IsItemLearned(RequiredSkillID) ?? false) ^ Inverted));
        }
    }
}
