using System.Collections.Generic;

namespace RPSLS.AIStrategies
{
    public interface IAIStrategy
    {
        Gesture ChooseGesture(List<Gesture> gestures);
    }
}