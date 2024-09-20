using System.Linq;

namespace EffectSourceConditions
{
    public class SourceConditionItemInInventory : SourceCondition
    {
        /// <summary>
        /// The item ID that a character must have equipped for CharacterHasRequirement to return true
        /// </summary>
        public int RequiredItemID;

        /// <summary>
        /// The enchant ID that a character must have equipped for CharacterHasRequirement to return true
        /// </summary>
        public int Amount = 1;
        
        /// <summary>
        /// Set to true to require the character to not have the relevant skill
        /// </summary>
        public bool Inverted = false;

        /// <summary>
        /// Returns true if RequiredSkillID <= 0 or if the character knows the Skill with ItemID = RequiredSkillID
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public override bool CharacterHasRequirement(Character character)
        {
            return ((RequiredItemID == 0) || (((character?.Inventory?.ItemCount(RequiredItemID) ?? 0) > 0) ^ Inverted));
        }
    }
}
