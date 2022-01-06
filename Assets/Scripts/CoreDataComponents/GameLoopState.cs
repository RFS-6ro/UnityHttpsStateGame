namespace Core.Model
{
    public enum GameLoopState
    {
        SimulationReceiving,
        Visualisation,
        CollectingInput,
        SendingInput,

        Disabled
    }
}
