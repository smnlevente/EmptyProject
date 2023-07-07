using UnityEngine;

public class ShutterView : SingletonView<ShutterView>, IObserver
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject content;

    private bool opened = false;
    private bool closed = false;

    public bool Opened
    {
        get
        {
            return this.opened;
        }
    }

    public bool Closed
    {
        get
        {
            return this.closed;
        }
    }

    private void OpenEnded()
    {
        this.opened = true;
        LoadingController.Instance.OpenedShutter();
    }

    private void CloseEnded()
    {
        this.closed = true;
        LoadingController.Instance.ClosedShutter();
    }

    private void OnUnloadLoading()
    {
        this.opened = false;
        this.animator.Play("open");
    }

    private void OnMainSceneActive()
    {
        if (LoadingController.Instance.Shutter)
        {
            this.opened = false;
            TaskController.Instance.WaitFor(0.2f).Then(() => this.animator.Play("open"));
        }
        else
        {
            this.opened = false;
            this.animator.Play("open");
        }
    }

    private void Start()
    {
        if (LoadingController.Instance.Shutter)
        {
            this.closed = false;
            this.animator.Play("close");
        }

        LoadingController.Instance.Attach(this);
    }
}
