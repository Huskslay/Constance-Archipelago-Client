using TMPro;
using UnityEngine;

namespace RandomizerCore.Classes.Handlers.RecentItems;

public class TrackerText(TextMeshProUGUI text, bool permanent)
{
    private readonly TextMeshProUGUI text = text;
    private readonly bool permanent = permanent;

    private static readonly float fadeScale = 0.5f;

    private float timeUntilFade = 15f;
    private readonly float bumpHeight = 40f;

    public void Begin(string displayText, Color color)
    {
        text.text = displayText;
        text.color = color;
        text.gameObject.SetActive(true);
    }

    public void Bump()
    {
        text.transform.localPosition += new Vector3(0, bumpHeight, 0);
    }

    public bool Update()
    {
        if (permanent) return false;

        if (timeUntilFade > 0)
        {
            timeUntilFade -= Time.deltaTime;
            return false;
        }

        text.color = new(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * fadeScale);
        if (text.color.a <= 0)
        {
            Plugin.Destroy(text.gameObject);
            return true;
        }
        return false;
    }
}