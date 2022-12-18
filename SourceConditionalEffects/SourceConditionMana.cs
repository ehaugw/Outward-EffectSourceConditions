using System;
using UnityEngine;

namespace EffectSourceConditions
{
    public class SourceConditionMana : SourceCondition
    {
        public float ManaCost = 0;

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
            return (ManaCost <= 0 || (character.Mana >= character.Stats.GetFinalManaConsumption(null, ManaCost))) ^ Inverted;
        }
    }
}
