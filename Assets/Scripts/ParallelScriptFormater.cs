
using System;
using System.Text;
using TMPro;
using UnityEngine;

public class ParallelScriptFormater
{
    /// <summary>
    /// 移除标签
    /// </summary>
    public static StringBuilder RemoveTags(string Rawtext,TMP_Text tmpText)
    {
        tmpText.maxVisibleCharacters = 0;
        tmpText.text = Rawtext;
        tmpText.ForceMeshUpdate();
        StringBuilder unTagText = new StringBuilder();
        for (int i = 0; i < tmpText.textInfo.characterCount; i++)
        {
            unTagText.Append(tmpText.textInfo.characterInfo[i].character);
        }

        return unTagText;
    }
    public static TextAnimationType GetTextAnimationType(string stringVal)
    {
        TextAnimationType result;
        try
        {
            result = (TextAnimationType)Enum.Parse(typeof(TextAnimationType), stringVal, true);
        }
        catch (ArgumentException)
        {
            Debug.LogError("Invalid Text Animation Type: " + stringVal);
            result = TextAnimationType.None;
        }
        return result;
    }
}
