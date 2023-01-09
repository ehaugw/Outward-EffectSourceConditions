namespace EffectSourceConditions
{
    public class SourceConditionQuest : SourceCondition
    {
        /// <summary>
        /// The skill ID that a character must have learned for CharacterHasRequirement to return true
        /// </summary>
        public int RequiredQuestID;
        
        /// <summary>
        /// Set to true to require the character to not have the relevant skill
        /// </summary>
        public bool Inverted = false;

        /// <summary>
        /// Assigns the ID of the provided Skill to RequiredSkillID
        /// </summary>
        public Item RequiredQuest
        {
            get
            {
                return ResourcesPrefabManager.Instance.GetItemPrefab(RequiredQuestID);
            }
            set
            {
                RequiredQuestID = value.ItemID;
            }
        }

        /// <summary>
        /// Returns true if RequiredSkillID == 0 or if the character knows the Skill with ItemID = RequiredSkillID
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public override bool CharacterHasRequirement(Character character)
        {
            return ((RequiredQuestID == 0) || ((character?.Inventory?.QuestKnowledge?.IsItemLearned(RequiredQuestID) ?? false) ^ Inverted));
        }
    }
}
