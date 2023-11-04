using System.Linq;
using TinyHelper;

namespace EffectSourceConditions
{
    public class SourceConditionQuest : SourceCondition
    {
        /// <summary>
        /// The skill IDs that a character must have learned for CharacterHasRequirement to return true
        /// </summary>
        public int[] Quests;
        
        /// <summary>
        /// Set to true to require the character to not have the relevant skill
        /// </summary>
        public bool Inverted = false;

        public LogicType Logic = LogicType.Any;

        /// <summary>
        /// Returns true the character knows the Skill with ItemID = RequiredSkillID
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public override bool CharacterHasRequirement(Character character)
        {
            return QuestRequirements.HasQuestKnowledge(character, Quests, Logic, Inverted);
        }
    }
}
