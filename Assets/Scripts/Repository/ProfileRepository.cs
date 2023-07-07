using System;
using System.Collections.Generic;
using System.IO;
using ObjectMapper;
using UnityEngine;

public class ProfileRepository : Repository<ProfileRepository, Profile>, IObserver
{
    private static readonly string SaveFile = "profile.sav";
    private float serializationTimeout = 60f;
    private float currentTime = 0f;
    private bool blockSave = false;

    public string DirPath
    {
        get
        {
            return FileReaders.Get.GetPersistentDataPath();
        }
    }

    protected override ISerializer<RepositoryDto<Profile>> Serializer()
    {
        string persistentPath = FileReaders.Get.GetPersistentDataPath();
        string path = Path.Combine(persistentPath, SaveFile);
        return new JsonSerializer<RepositoryDto<Profile>>.Builder(path).Build();
    }

    protected override List<Profile> Initialize()
    {
        return new List<Profile>() { new Profile() };
    }

    protected override void Serialize()
    {
        if (this.blockSave)
        {
            return;
        }

        this.GetFirst().SetLastSave();
        base.Serialize();
    }

    protected override void Deserialize()
    {
        this.RemoveAll();
        base.Deserialize();
    }

    private void OnReloadScene()
    {
        this.Deserialize();
    }

    private void OnMainSceneActive()
    {
        this.GetFirst().Initialize();
    }

    private void OnChangingScene(string sceneName)
    {
        this.Serialize();
    }

    private void OnServerSaveChosen()
    {
        this.blockSave = true;
    }

    private void Update()
    {
        this.currentTime += Time.deltaTime;
        if (this.currentTime >= this.serializationTimeout)
        {
            this.currentTime = 0f;
            this.Serialize();
        }
    }
}
