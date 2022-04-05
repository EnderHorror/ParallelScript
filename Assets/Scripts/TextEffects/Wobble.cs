
using TMPro;
using UnityEngine;

public class Wobble : ITextEffect
{
    /// <summary>
    /// 文字飘动特效
    /// </summary>
    /// <param name="info"></param>
    /// <param name="src"></param>
    public void HandleEffect(TextAnimInfo info, TMP_Text tMP_Text, Mesh src)
    {
        var vertices = src.vertices;

        for (int i = info.startIndex; i < info.endIndex; i++)
        {
            TMP_CharacterInfo c = tMP_Text.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            Vector3 offset = WobbleGenerate(Time.time + i)*2;
            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }
        src.vertices = vertices;
    }
    
    static Vector2 WobbleGenerate(float time) 
    {
        return new Vector2(Mathf.Sin(time*15f), Mathf.Cos(time*12f));
    }


}
