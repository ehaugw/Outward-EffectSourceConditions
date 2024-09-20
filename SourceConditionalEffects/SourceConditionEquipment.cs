using System.Linq;

namespace EffectSourceConditions
{
    public class SourceConditionEquipment : SourceCondition
    {
        /// <summary>
        /// The item ID that a character must have equipped for CharacterHasRequirement to return true
        /// </summary>
        public int RequiredItemID;

        /// <summary>
        /// The enchant ID that a character must have equipped for CharacterHasRequirement to return true
        /// </summary>
        public int RequiredEnchantID;
        
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
            if (character?.Inventory?.Equipment is CharacterEquipment characterEquipment)
            {
                foreach (var slot in characterEquipment.EquipmentSlots.Where(s => s != null && s.EquippedItem != null))
                {
                    var matchingItem    = RequiredItemID    == 0 || slot.EquippedItem.ItemID == RequiredItemID;
                    var matchingEnchant = RequiredEnchantID == 0 || slot.EquippedItem.ActiveEnchantmentIDs.Contains(RequiredEnchantID);

                    if (matchingItem && matchingEnchant)
                    {
                        return !Inverted;
                    }
                }
            }

            return Inverted;
            //return ((RequiredItemID == 0) || (((character?.Inventory?.Equipment?.ItemEquippedCount(RequiredItemID) ?? 0) > 0) ^ Inverted));
        }
    }
}
