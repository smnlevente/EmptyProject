using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField]
    private string localizationKey;
    private Text textComponent;

    public Text TextComponent
    {
        get
        {
            return this.textComponent = this.AssertComponent(this.textComponent);
        }
    }

    public string LocalizationKey
    {
        get
        {
            return this.localizationKey;
        }
    }

    private void Start()
    {
        this.TextComponent.SetLocalize(this.LocalizationKey);
    }
}
