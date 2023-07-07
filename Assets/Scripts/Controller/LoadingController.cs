using System;
using MVC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : Controller<LoadingController>, IObserver
{
    private Action sceneLoaded;
    private Action activeSceneChanged;
    private Action sceneUnloaded;

    private bool shutter = false;

    public bool Shutter
    {
        get
        {
            return this.shutter;
        }
    }

    public void OpenedShutter()
    {
        SceneManager.UnloadSceneAsync("Loading");
    }

    public void ClosedShutter()
    {
        if (this.activeSceneChanged == null)
        {
            return;
        }

        this.activeSceneChanged();
    }

    public void UnloadLoading()
    {
        if (SceneManager.GetSceneByName("Loading").isLoaded)
        {
            this.NotifyAll("UnloadLoading");
        }

        this.activeSceneChanged = () => this.NotifyAll("UnloadLoading");
    }

    public void LoadLoading()
    {
        this.sceneLoaded = null;
        this.activeSceneChanged = null;
        this.sceneUnloaded = null;
        this.shutter = true;
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
    }

    public void ReturnToMain()
    {
        this.GotoScene(this.GetMainScene());
        this.sceneLoaded = () =>
        {
            TaskController.Instance.WaitForNextFrame()
              .Then(() => SceneManager.SetActiveScene(SceneManager.GetSceneByName("Loading")));
            this.sceneLoaded = null;
        };

        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Restart game will call StartGame from LoadingView
    /// </summary>
    public void RestartGame()
    {
        this.shutter = false;
        this.sceneLoaded = null;
        this.activeSceneChanged = () => TaskController.Instance.WaitForNextFrame().Then(this.StartMain);
        this.sceneUnloaded = null;
        this.ResetSetup();
        this.NotifyAll("RestartGame");
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);
    }

    public void LoadingEnded()
    {
        if (this.sceneLoaded != null)
        {
            this.sceneLoaded();
        }
    }

    private void ResetSetup()
    {
        TaskController.Instance.Stop();
        CameraController.Instance.RestoreRaycastLayers();
        this.NotifyAll("ReloadScene");
    }

    /// <summary>
    /// Here we setup actions from every stage of loading
    /// View and real loading have to finish for Main scene to become active 
    /// sceneLoaded when called first time will set it self to set Main scene as active when called second time
    /// That is needed because two conditions have to be met for loading to be done
    /// </summary>
    private void StartMain()
    {
        this.activeSceneChanged = null;
        this.sceneUnloaded = null;
        this.sceneLoaded = () => this.sceneLoaded = () =>
        {
            this.NotifyAll("PreMainSceneLoaded");
            this.NotifyAll("MainSceneLoaded");
            this.activeSceneChanged = () => this.NotifyAll("MainSceneActive");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(this.GetMainScene()));
        };

        AsyncOperation mainLoading = SceneManager.LoadSceneAsync(this.GetMainScene(), LoadSceneMode.Additive);
        this.NotifyAll("MainSceneLoading");
        TaskController.Instance.WaitUntil(time =>
        {
            this.NotifyAll("SceneProgress", mainLoading.progress);
            return mainLoading.isDone;
        });
    }

    /// <summary>
    /// This function is called when we start the game from Main scene
    /// It simulates phases of the real loading
    /// </summary>
    private void StartGameFromMain()
    {
        TaskController.Instance.WaitForNextFrame()
            .Then(() => this.NotifyAll("MainSceneLoading"))
            .WaitForNextFrame()
            .Then(() =>
            {
                this.NotifyAll("PreMainSceneLoaded");
                this.NotifyAll("MainSceneLoaded");
            })
            .WaitForNextFrame()
            .Then(() => this.NotifyAll("MainSceneActive"));
    }

    /// <summary>
    /// Called when we want to visit friend, neighbour or return to our farm
    /// It calls all notifications needed for scene transition
    /// Unloads current active scene
    /// </summary>
    /// <param name="scene">Name of the scene to be loaded</param>
    private void GotoScene(string scene)
    {
        string notificationName = scene.Contains("Main") ? "main" : scene.ToLower();
        string currentScene = SceneManager.GetActiveScene().name;

        this.shutter = true;
        this.NotifyAll("PreChangingScene", currentScene);
        this.NotifyAll("ChangingScene", currentScene);
        this.ResetSetup();

        this.sceneLoaded = null;
        this.activeSceneChanged = null;
        this.sceneUnloaded = null;

        this.activeSceneChanged = () => 
        {
            this.activeSceneChanged = () =>
            {
                SceneManager.UnloadSceneAsync(currentScene);
                this.NotifyAll(string.Format("{0}SceneLoading", notificationName));
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                this.activeSceneChanged = null;
                this.sceneUnloaded = null;

                this.sceneLoaded = () =>
                {
                    this.activeSceneChanged = () =>
                    TaskController.Instance.WaitForNextFrame()
                        .Then(() => this.NotifyAll(string.Format("{0}SceneActive", notificationName)));
                    TaskController.Instance.WaitForNextFrame()
                        .Then(() =>
                        {
                            this.NotifyAll(string.Format("Pre{0}SceneLoaded", notificationName));
                            this.NotifyAll(string.Format("{0}SceneLoaded", notificationName));
                            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
                        });
                };
            };
        };
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.sceneLoaded != null)
        {
            this.sceneLoaded();
        }

        this.NotifyAll("SceneLoaded", scene);
    }

    private void SceneUnloaded(Scene scene)
    {
        if (this.sceneUnloaded != null)
        {
            this.sceneUnloaded();
        }

        this.NotifyAll("SceneUnloaded", scene);
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene != scene)
        {
            this.NotifyAll("SceneActive", activeScene);
        }
    }

    private void ActiveSceneChanged(Scene newScene, Scene oldScene)
    {
        if (this.activeSceneChanged != null)
        {
            this.activeSceneChanged();
        }
    }

    private string GetMainScene()
    {
        return "Main";
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Main"))
        {
            this.StartGameFromMain();
        }
        else
        {
            TaskController.Instance.WaitForNextFrame().Then(this.StartMain);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += this.SceneLoaded;
        SceneManager.sceneUnloaded += this.SceneUnloaded;
        SceneManager.activeSceneChanged += this.ActiveSceneChanged;
    }
}
