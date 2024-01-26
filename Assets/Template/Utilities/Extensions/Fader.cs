using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//TODO: Update this to use DoTween
public static class Fader
{
    private static readonly List<CancellationTokenSource> _cancellationTokensSources = new();

    public static void StopAllFades()
    {
        foreach (CancellationTokenSource cancellationTokenSource in _cancellationTokensSources)
        {
            cancellationTokenSource.Cancel();
        }

        _cancellationTokensSources.Clear();
    }

    public static void FadeIn(this CanvasGroup canvasGroup)
    {
        canvasGroup.Fade(1).Forget();
    }

    public static void FadeOut(this CanvasGroup canvasGroup)
    {
        canvasGroup.Fade(0).Forget();
    }

    public static async UniTask Fade(this CanvasGroup canvasGroup, float targetAlphaValue)
    {
        CancellationTokenSource cancellationTokenSource = new();
        _cancellationTokensSources.Add(cancellationTokenSource);

        if (targetAlphaValue > 0)
        {
            canvasGroup.gameObject.SetActive(true);
        }

        float currentTime = 0;
        float startingAlpha = canvasGroup.alpha;

        while (currentTime < Durations.UIFadeDuration)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            canvasGroup.alpha = Mathf.Lerp(startingAlpha, targetAlphaValue, currentTime / Durations.UIFadeDuration);
            currentTime += Time.unscaledDeltaTime;
            await UniTask.Yield();
        }

        canvasGroup.alpha = targetAlphaValue;
        UpdateGameObjectActiveState(canvasGroup.gameObject, canvasGroup.alpha);
    }

    private static void UpdateGameObjectActiveState(GameObject objectToUpdate, float objectAlpha)
    {
        objectToUpdate.SetActive(objectAlpha != 0);
    }

    public static void FadeIn(this Image image)
    {
        image.Fade(1).Forget();
    }

    public static void FadeOut(this Image image)
    {
        image.Fade(0).Forget();
    }

    public static async UniTask Fade(this Image image, float targetAlphaValue)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokensSources.Add(cancellationTokenSource);

        if (targetAlphaValue > 0)
        {
            image.gameObject.SetActive(true);
        }

        float currentTime = 0;
        float startingAlpha = image.color.a;

        while (currentTime < Durations.UIFadeDuration)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            float newAlphaValue = Mathf.Lerp(startingAlpha, targetAlphaValue, currentTime / Durations.UIFadeDuration);
            image.color = image.color.GetWithNewAlpha(newAlphaValue);
            currentTime += Time.unscaledDeltaTime;
            await UniTask.Yield();
        }

        image.color = image.color.GetWithNewAlpha(targetAlphaValue);
        UpdateGameObjectActiveState(image.gameObject, image.color.a);
    }
}