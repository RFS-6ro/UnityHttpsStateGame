namespace Core.Model
{
    public interface IStateAffectible
    {
        GameLoopState CurrentState { get; }

        void SetState(GameLoopState state);
    }
}
