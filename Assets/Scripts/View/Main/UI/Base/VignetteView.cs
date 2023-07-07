using System;
using System.Collections;
using MVC;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class VignetteView : UIView
{
    private CanvasGroup canvasGroup;
    private bool visible = false;
    private Coroutine fadeout;
    private Coroutine fadein;

    public CanvasGroup CanvasGroup
    {
        get
        {
            if (this.canvasGroup == null)
            {
                this.canvasGroup = this.GetComponent<CanvasGroup>();
            }

            return this.canvasGroup;
        }
    }

    public float CanvasAlpha
    {
        get
        {
            return this.CanvasGroup.alpha;
        }

        set
        {
            this.CanvasGroup.alpha = value;
        }
    }

    public void FadeIn(float time)
    {
        this.Show();
        if (this.fadeout != null)
        {
            this.StopCoroutine(this.fadeout);
        }

        if (this.fadein != null)
        {
            return;
        }

        this.fadein = this.StartCoroutine(this.Fade(time, 1f, () => this.fadein = null));
    }

    public void FadeOut(float time)
    {
        if (!this.visible)
        {
            return;
        }

        if (this.fadein != null)
        {
            this.StopCoroutine(this.fadein);
        }

        if (this.fadeout != null)
        {
            return;
        }

        this.StopAllCoroutines();
        this.fadeout = this.StartCoroutine(this.Fade(
            time,
            0f,
            () =>
            {
                this.Hide();
                this.fadeout = null;
            }));
    }

    public override void Hide()
    {
        this.visible = false;
        base.Hide();
    }

    public override void Show()
    {
        this.visible = true;
        base.Show();
    }

    private IEnumerator Fade(float duration, float target, Action callback)
    {
        float time = 0f;
        float start = this.CanvasAlpha;
        float diff = start - target;
        while (time < duration)
        {
            this.CanvasAlpha = start - (Mathf.Clamp01(time / duration) * diff);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        if (callback != null)
        {
            callback();
        }
    }
}
