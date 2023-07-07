using System;
using MVC;
using TaskManager;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : View, IObserver
{
    private float loadingTime = 2f;
    private float loadPercent = 0f;
    private float serverPercent = 0f;
    private Action startTimer;
    private Task serverIncremantor;
    private Task timerIncremantor;

    private void StartTimer()
    {
        this.loadPercent = Mathf.Max(this.loadPercent, this.serverPercent);
        this.timerIncremantor = TaskController.Instance.WaitUntil(time =>
        {
            return time >= this.loadingTime;
        }).Then(() =>
        {
            LoadingController.Instance.LoadingEnded();
        });
    }

    private void Setup()
    {
        this.loadPercent = 0f;
        this.serverPercent = 0f;
    }

    private void OnMainSceneLoading()
    {
        this.StartTimer();
    }

    private void OnSceneProgress(float progress)
    {
        this.loadPercent = Mathf.Max(this.loadPercent, progress);
    }

    private void OnServerResourceTimerRestart()
    {
        if (this.serverIncremantor != null)
        {
            this.serverIncremantor.Stop();
        }

        if (this.timerIncremantor != null)
        {
            this.timerIncremantor.Stop();
        }

        this.Setup();
    }

    private void Start()
    {
        if (LoadingController.Instance.Shutter)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.Setup();
        LoadingController.Instance.Attach(this);
    }

    private void OnDisable()
    {
        if (LoadingController.Instance != null)
        {
            LoadingController.Instance.Detach(this);
        }

        if (this.serverIncremantor != null)
        {
            this.serverIncremantor.Stop();
        }
    }
}
