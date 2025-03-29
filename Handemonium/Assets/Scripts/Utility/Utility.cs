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
        public static void ImageFadeEffect(Image image, float startAmount = 0, float endAmount = 1, float duration = 3, Action onComplete = null)
        {
            Color color = image.color;
            color.a = startAmount;
            image.color = color;

            // Tween opacity from <startAmount> to <endAmount> in <duration> second
            image.DOFade(endAmount, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}