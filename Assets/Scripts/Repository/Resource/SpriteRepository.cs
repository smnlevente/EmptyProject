using System.Linq;
using UnityEngine;

public class SpriteRepository : EditorJsonRepository<SpriteRepository>
{
    public Sprite GetSpriteByID(string name)
    {
        KeyStringPair pair = this.GetByID(name);
        if (pair == null)
        {
            return null;
        }

        int lastWord = pair.Value.LastIndexOf("/") + 1;
        string spriteID = pair.Value.Substring(lastWord, pair.Value.Length - lastWord);
        string textureID = pair.Value.Substring(0, lastWord - 1);

        return Resources.LoadAll<Sprite>(textureID).FirstOrDefault(sprite => sprite.name == spriteID);
    }

    protected override string JsonFile()
    {
        return "Editor/sprite.json";
    }
}
