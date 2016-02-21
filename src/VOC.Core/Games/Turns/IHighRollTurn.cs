namespace VOC.Core.Games.Turns
{
    public interface IHighRollTurn : ITurn
    {
        int Result { get; }
    }
}