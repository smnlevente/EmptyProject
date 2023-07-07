using ObjectMapper;

public class LocalizationRepository : Repository<LocalizationRepository, KeyStringPair>
{
    public override void Reload()
    {
        base.Reload();
        this.RemoveAll();
        this.Deserialize();
    }

    protected override ISerializer<RepositoryDto<KeyStringPair>> Serializer()
    {
        string localization = FileReaders.Get.ReadFile(string.Format("Localization/{0}.loc", LocalizationController.Instance.LanguageFile));
        return new LocalizationSerializer(localization);
    }
}
