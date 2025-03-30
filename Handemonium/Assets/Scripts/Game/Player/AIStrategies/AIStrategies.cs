using System.Collections.Generic;

namespace RPSLS.AIStrategies
{
    public class AIStrategyPureRandom : IAIStrategy
    {
        public Gesture ChooseGesture(List<Gesture> gestures)
        {
            return gestures[UnityEngine.Random.Range(0, gestures.Count)];
        }
    }
    
    public class AIStrategyWeightedRandom : IAIStrategy
    {
        private static Dictionary<GestureType, float> STATIC_WEIGHTS = new Dictionary<GestureType, float>()
        {
            { GestureType.Rock, .3f },
            { GestureType.Paper, .15f },
            { GestureType.Scissor, .25f },
            { GestureType.Lizard, .2f },
            { GestureType.Spock, .25f },
        };

        public Gesture ChooseGesture(List<Gesture> gestures)
        {
            float[] weights = new float[gestures.Count];
            float sum = 0;
            for (int i = 0; i < gestures.Count; i++)
            {
                float weight = STATIC_WEIGHTS[gestures[i].GestureType];
                weights[i] = weight;
                sum += weight;
            }

            int index = Utility.GetWeightedRandom(weights, sum);
            return gestures[index];
        }
    }
}