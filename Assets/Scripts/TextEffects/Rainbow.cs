using TMPro;
using UnityEngine;


public class Rainbow : ITextEffect
{
    public Gradient rainbow = new Gradient();
    //渐变色需要自己想办法设置
    public Rainbow()
    {
        rainbow = Resources.Load<EffectPreset>("EffectPreset").textGradinet;
    }
    
    public void HandleEffect(TextAnimInfo info,TMP_Text tMP_Text, Mesh src)
    {
        var colors = src.colors;
        var vertices = src.vertices;
        for (int i = info.startIndex; i <info.endIndex; i++)
        {
            TMP_CharacterInfo c = tMP_Text.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index].x*0.001f, 1f));
            colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x*0.001f, 1f));
            colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x*0.001f, 1f));
            colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x*0.001f, 1f));
        }
        src.colors = colors;
    }
}
