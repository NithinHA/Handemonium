using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPSLS
{
    public static class Utility
    {
        public static void ImageFadeEffect(Image image, float startAmount = 0, float endAmount = 1, float duration = 1, Ease easeMode = Ease.Linear, Action onComplete = null)
        {
            Color color = image.color;
            color.a = startAmount;
            image.color = color;

            // Tween opacity from <startAmount> to <endAmount> in <duration> second
            image.DOFade(endAmount, duration).SetEase(easeMode).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public static int GetWeightedRandom(float[] weights, float totalWeight)
        {
            float randomVal = UnityEngine.Random.Range(0, totalWeight);
            for (int i = 0; i < weights.Length; i++)
            {
                if(randomVal <= weights[i])
                    return i;
                randomVal -= weights[i];
            }
            return weights.Length - 1;
        }
    }
}