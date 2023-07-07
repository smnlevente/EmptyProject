using System;
using System.Text;
using System.Text.RegularExpressions;
using MVC;
using UnityEngine;

public class LocalizationController : Controller<LocalizationController>, IObserver
{
    public string LanguageFile
    {
        get
        {
            return "Localization";
        }
    }

    public string LocalizeText(string textToLocalize)
    {
        Regex re = new Regex(@"\$\$([A-Za-z0-9\-\_]+)");
        string[] substrings = re.Split(textToLocalize);
        StringBuilder sb = new StringBuilder();

        foreach (string match in substrings)
        {
            if (match == string.Empty)
            {
                continue;
            }

            KeyStringPair item = null;

            if (item == null)
            {
                item = LocalizationRepository.Instance.GetByID(match);
            }

            if (item == null)
            {
                Debug.LogWarning("Missing localization key: " + match);
                sb.Append(match);
            }
            else
            {
                sb.Append(item.Value);
            }
        }

        return sb.ToString();
    }

    public bool TextKeyExists(string textKey)
    {
        KeyStringPair item = LocalizationRepository.Instance.GetByID(textKey);
        return item != null;
    }

    public string LocalizeTime(int seconds)
    {
        return this.LocalizeTime(TimeSpan.FromSeconds(seconds));
    }

    public string LocalizeTime(TimeSpan duration)
    {
        if (duration.Days > 0)
        {
            return string.Format("{0:D1}D {1:D2}h", duration.Days, duration.Hours);
        }

        if (duration.Hours > 0)
        {
            return string.Format("{0:D1}h {1:D2}m", duration.Hours, duration.Minutes);
        }

        if ((duration.Hours < 1) && (duration.Minutes > 0))
        {
            return string.Format("{0:D1}m {1:D2}s", duration.Minutes, duration.Seconds);
        }

        if ((duration.Hours < 1) && (duration.Minutes < 1))
        {
            int seconds = Mathf.FloorToInt((float)duration.TotalSeconds);
            return string.Format("{0:D2}s", seconds < 1 ? 1 : seconds);
        }

        return duration.TotalSeconds.ToString();
    }

    public string LocalizeHourTime(TimeSpan duration)
    {
        if (duration.Hours > 0)
        {
            if (duration.Minutes > 0)
            {
                return string.Format("{0:D1}h {1:D2}m", duration.Hours, duration.Minutes);
            }
            else
            {
                return string.Format("{0:D1}h", duration.Hours);
            }
        }

        return duration.TotalSeconds.ToString();
    }

    public string LocalizeSecondsToOneDecimal(float floatSeconds)
    {
        TimeSpan timeToLocalize = TimeSpan.FromSeconds(floatSeconds);
        int seconds = timeToLocalize.Seconds;
        int deciseconds = timeToLocalize.Milliseconds / 100;
        return string.Format("{0:D1}.{1:D1}s", seconds, deciseconds);
    }
}