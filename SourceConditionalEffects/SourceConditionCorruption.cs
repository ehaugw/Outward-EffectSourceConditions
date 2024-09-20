using System;
using UnityEngine;

namespace EffectSourceConditions
{
    public class SourceConditionCorruption : SourceCondition
    {
        public float Corruption = 0;

        /// <summary>
        /// Set to true to require the character to not have the relevant skill
        /// </summary>
        public bool Inverted = false;

        /// <summary>
        /// Returns true if ManaCost <= 0 or if the character has equal to or more mana than required.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public override bool CharacterHasRequirement(Character character)
        {
            return ((character?.PlayerStats?.Corruption?? 0) >= Corruption) ^ Inverted;
        }
    }
}
