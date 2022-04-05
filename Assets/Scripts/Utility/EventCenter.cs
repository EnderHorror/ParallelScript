using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.Events;

/// <summary>
/// 空接口,用于保存两种委托类型来转换
/// </summary>
public interface I_Empty
{

}
/// <summary>
/// 带参数委托
/// </summary>
/// <typeparam name="T">类型</typeparam>
public class ActionContainer<T>:I_Empty
{
    public UnityAction<T> actionData;
    public ActionContainer(UnityAction<T> action)
    {
        actionData += action;
    }  
}

/// <summary>
/// 无参委托
/// </summary>
public class ActionContainerWithoutT: I_Empty
{
    public UnityAction actionData;
    public ActionContainerWithoutT(UnityAction action)
    {
        actionData += action;
    }
}
public class EventCenter : Singleton<EventCenter>
{
    Dictionary<string, I_Empty> dic = new Dictionary<string, I_Empty>();

    
    
    /// <summary>
    /// 添加监听事件[带有参数的响应]
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void AddEventListener<T>(string eventName,UnityAction<T> action)
    {
        if(dic.ContainsKey(eventName))
        {
            (dic[eventName] as ActionContainer<T>).actionData += action;
        }
        else
        {
            dic.Add(eventName, new ActionContainer<T>(action));
        }
    }
    /// <summary>
    /// 添加监听事件[不带参数的响应]
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void AddEventListener(string eventName, UnityAction action)
    {
        
        
        if (dic.ContainsKey(eventName))
        {
            (dic[eventName] as ActionContainerWithoutT).actionData += action;
        }
        else
        {
            dic.Add(eventName, new ActionContainerWithoutT(action));
        }
    }
    /// <summary>
    /// 当事件触发[带有参数]
    /// </summary>
    /// <param name="evenName"></param>
    public void EventTrigger<T>(string eventName,T info)
    {
        if(dic.ContainsKey(eventName))
        {
            (dic[eventName] as ActionContainer<T>).actionData(info);
        }
    }
    /// <summary>
    /// 当事件触发[带有参数]
    /// </summary>
    /// <param name="evenName"></param>
    public void EventTrigger(string eventName)
    {
        if (dic.ContainsKey(eventName))
        {
            (dic[eventName] as ActionContainerWithoutT).actionData();
        }
    }
    /// <summary>
    /// 移除委托[带有参数]
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void RemoveAction<T>(string eventName,UnityAction<T> action)
    {
        if(dic.ContainsKey(eventName))
        {
            (dic[eventName] as ActionContainer<T>).actionData -= action;
        }
    }
    /// <summary>
    /// 移除委托[不带有参数]
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="action"></param>
    public void RemoveAction(string eventName, UnityAction action)
    {
        if (dic.ContainsKey(eventName))
        {
            (dic[eventName] as ActionContainerWithoutT).actionData -= action;
        }
    }
    /// <summary>
    /// 清空所有事件
    /// </summary>
    public void Clear()
    {
        dic.Clear();
    }
    
}