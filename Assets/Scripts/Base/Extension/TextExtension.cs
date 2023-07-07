using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public static class TextExtension
{
    private static List<UILineInfo> lines = new List<UILineInfo>();
    private static Dictionary<Text, int> startFonts = new Dictionary<Text, int>();

    public static void SetLocalize(this Text component, string format, params object[] arguments)
    {
        component.Set(LocalizationController.Instance.LocalizeText(string.Format(format, arguments)));
    }

    public static void SetLocalize(this Text component, string key)
    {
        component.Set(LocalizationController.Instance.LocalizeText(key));
    }

    public static void Set(this Text component, string format, params object[] arguments)
    {
        component.Set(string.Format(format, arguments));
    }

    public static void Set(this Text component, string format, object argument)
    {
        component.Set(string.Format(format, argument));
    }

    public static void Set(this Text component, string text)
    {
        component.text = text;
        component.SetSize();
    }

    public static void SetSize(this Text component)
    {
        if (component == null || component.text == null || component.text.Length < 1)
        {
            return;
        }

        if (!startFonts.ContainsKey(component))
        {
            startFonts.Add(component, component.fontSize);
        }
        else
        {
            component.fontSize = startFonts[component];
        }

        float pixelsPerUnit = component.font.fontSize / (float)component.fontSize;
        string regularText = component.text;
        if (component.supportRichText)
        {
            regularText = Regex.Replace(component.text, "<.*?>", string.Empty);
        }
        
        TextGenerationSettings settings = component.GetGenerationSettings(component.rectTransform.rect.size);
        float height = component.cachedTextGeneratorForLayout.GetPreferredHeight(regularText, settings) / pixelsPerUnit;
        float width = component.cachedTextGeneratorForLayout.GetPreferredWidth(GetLine(component, regularText), settings) / pixelsPerUnit;
        float currentWidth = component.rectTransform.rect.width;
        float currentHeight = component.rectTransform.rect.height;
        while (width > currentWidth || height > currentHeight)
        {
            if (component.fontSize <= 7)
            {
                Debug.LogWarningFormat("Font too small on: {0}; text {1}", component.gameObject.name, regularText);
                return;
            }

            component.fontSize = Mathf.Max(7, component.fontSize - 2);
            pixelsPerUnit = component.font.fontSize / (float)component.fontSize;
            settings = component.GetGenerationSettings(component.rectTransform.rect.size);
            height = component.cachedTextGeneratorForLayout.GetPreferredHeight(regularText, settings) / pixelsPerUnit;
            width = component.cachedTextGeneratorForLayout.GetPreferredWidth(GetLine(component, regularText), settings) / pixelsPerUnit;
        }
    }

    private static string GetLine(this Text component, string regularText)
    {
        component.cachedTextGeneratorForLayout.GetLines(lines);
        string text = regularText;
        if (lines != null && lines.Count > 1)
        {
            UILineInfo currentLine = lines.OrderBy(line => line.ToString().Length).First();
            int preIndex = currentLine.startCharIdx;
            int index = lines.IndexOf(currentLine);
            int nextIndex = (index + 1) < lines.Count ? lines[index + 1].startCharIdx : text.Length;
            text = text.Substring(preIndex, nextIndex - preIndex);
        }

        return text;
    }
}
