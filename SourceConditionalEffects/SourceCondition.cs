namespace EffectSourceConditions
{
    public abstract class SourceCondition : UnityEngine.MonoBehaviour
    {
        public abstract bool CharacterHasRequirement(Character character);
    }
}
