using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.Events;
[RequireComponent(typeof(TMPEffect))]
public class Dialog : MonoBehaviour
{
    public bool Playing { get; private set; } = false;
    [Tooltip("播放一行文字的时间")]
    [SerializeField] private float textAnimateTime = 0.4f;
    public UnityEvent OnFinish;

    private TMP_Text tmpText;
    private TMPEffect tmpEffect;
    
    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        tmpEffect = GetComponent<TMPEffect>();
    }

    public void Play(string text)
    {
        if (!Playing)
        {
            StartCoroutine(PlayText(text));
        }
    }
    
    
    /// <summary>
    /// 显示文字的协程
    /// </summary>
    IEnumerator PlayText(string rawtext)
    {

        //去掉脚本<>里的内容
        var tokens = TokenAnalyze.ProcessInputString(rawtext, out var outputString);
        //需要显示的内容
        tmpText.SetText(outputString);
        tmpText.ForceMeshUpdate();  

        tmpEffect.Animate(tokens);//更新文字信息

        Playing = true;

        float waitTime = textAnimateTime/ (tmpText.textInfo.characterCount+1);//计算单个文字出现时间


        tmpText.maxVisibleCharacters = 0;

        for (int i = 0; i <= tmpText.textInfo.characterCount; i++)
        {
            LuaEventCenter.Instance.DoString($"w={waitTime}");//设置等待时间
            foreach (var job in tokens)
            {
                if(job.type==DialogueCommandType.Lua&&job.position==i) LuaEventCenter.Instance.DoString(job.stringValue);
            }
            yield return new WaitForSeconds((float)LuaEventCenter.Instance.GetNumber("w"));
            //+1是因为第一个字符应该显示
            tmpText.maxVisibleCharacters = i+1;
        }
        OnFinish.Invoke();
        Playing = false;
    }
}
