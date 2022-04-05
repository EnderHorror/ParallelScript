using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

/// <summary>
/// 文字特效函数接口
/// </summary>
public interface ITextEffect
{
    /// <summary>
    /// 处理字符网格
    /// </summary>
    /// <param name="info">link标签信息</param>
    /// <param name="src">网格</param>
    void HandleEffect(TextAnimInfo animInfo,TMP_Text tMP_Text, Mesh src);
}

/// <summary>
/// TMP文字自定义特效
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TMPEffect : MonoBehaviour
{
    private TMP_Text tmpText;
    private Dictionary<TextAnimationType,ITextEffect> effectsDir = new Dictionary<TextAnimationType, ITextEffect>();
    private TextAnimInfo[] textAnimInfos = new TextAnimInfo[] {};
    
    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        GenerateEffects();
    }
    
    /// <summary>
    /// 读取实现ITextEffect接口是类并实例化添加到effectsDir里
    /// </summary>
    private void GenerateEffects()
    {
        var effects = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t =>t.GetInterfaces().Contains(typeof(ITextEffect))).ToArray();
        foreach (var effect in effects)
        {
            ConstructorInfo ctor = effect.GetConstructor(new Type[0]);
            var textInterface = ctor.Invoke(null) as ITextEffect;
            effectsDir.Add((TextAnimationType)Enum.Parse(typeof(TextAnimationType),effect.Name),textInterface);
        }
    }


    public void Animate(List<DialogueCommand> commands)
    {
        textAnimInfos = SeparateOutTextAnimInfo(commands);
    }
    private void Update()
    {
        tmpText.ForceMeshUpdate();
        Mesh mesh = tmpText.mesh;
        
        foreach (var item in textAnimInfos)
        {
            ApllyEffect(item,mesh,effectsDir[item.type]);
        }
        tmpText.canvasRenderer.SetMesh(mesh);
    }
    
    /// <summary>
    /// 应用效果器
    /// </summary>
    /// <param name="id">link的值</param>
    /// <param name="mesh">TMP的网格</param>
    /// <param name="effectFunc">效果类的接口</param>
    void ApllyEffect(TextAnimInfo animInfom ,Mesh mesh,ITextEffect effectInterface)
    {
        effectInterface.HandleEffect(animInfom, tmpText, mesh);
    }
    /// <summary>
    /// 把动画commands 开始和结束部分连接起来
    /// </summary>
    private TextAnimInfo[] SeparateOutTextAnimInfo(List<DialogueCommand> commands)
    {
        List<TextAnimInfo> tempResult = new List<TextAnimInfo>();
        List<DialogueCommand> animStartCommands = new List<DialogueCommand>();
        List<DialogueCommand> animEndCommands = new List<DialogueCommand>();
        for (int i = 0; i < commands.Count; i++)
        {
            DialogueCommand command = commands[i];
            if (command.type == DialogueCommandType.AnimStart)
            {
                animStartCommands.Add(command);
                commands.RemoveAt(i);
                i--;
            }
            else if (command.type == DialogueCommandType.AnimEnd)
            {
                animEndCommands.Add(command);
                commands.RemoveAt(i);
                i--;
            }
        }
        if (animStartCommands.Count != animEndCommands.Count)
        {
            Debug.LogError("Unequal number of start and end animation commands. Start Commands: " + animStartCommands.Count + " End Commands: " + animEndCommands.Count);
        }
        else
        {
            for (int i = 0; i < animStartCommands.Count; i++)
            {
                DialogueCommand startCommand = animStartCommands[i];
                DialogueCommand endCommand = animEndCommands[i];
                tempResult.Add(new TextAnimInfo
                {
                    startIndex = startCommand.position,
                    endIndex = endCommand.position,
                    type = startCommand.textAnimValue
                });
            }
        }
        return tempResult.ToArray();
    }
}
public struct TextAnimInfo
{
    public int startIndex;
    public int endIndex;
    public TextAnimationType type;

    public override string ToString()
    {
        return $"Start:{startIndex}\n" +
            $"End:{endIndex}\n" +
            $"type:{type}";
    }
}
public enum TextAnimationType
{
    None,
    Rainbow,
    Wobble
}
