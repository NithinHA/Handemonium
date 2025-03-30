using RPSLS.Framework;

namespace RPSLS.Game
{
    public struct RoundResultDesc
    {
        public Gesture Winner;
        public Gesture Loser;
        public RoundResultState Result;

        public RoundResultDesc(Gesture winner = null, Gesture loser = null, RoundResultState result = RoundResultState.None)
        {
            Winner = winner == null ? InGameController.Instance.GameRules.EmptyHandGesture : winner;
            Loser = loser == null ? InGameController.Instance.GameRules.EmptyHandGesture : loser;
            Result = result;
        }
    }
    
    public enum RoundResultState
    {
        None, Win, Draw, Lose
    }
}