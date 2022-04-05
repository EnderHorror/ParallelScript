using System;
using System.Collections.Generic;
using NLua;
using UnityEngine;
/// <summary>
/// Lua事件中心
/// </summary>
public class LuaEventCenter : Singleton<LuaEventCenter>
{
    private Lua lua;
    private List<string> registerList = new List<string>();

    public LuaEventCenter()
    {
        lua = new Lua();
        lua.LoadCLRPackage();
        this.RegisterFunction("print", "LPrint", this);
    }
    /// <summary>
    /// 注册Lua里可以调用的函数
    /// </summary>
    /// <param name="luaFuncName">lua里的函数名</param>
    /// <param name="CSname">C#里的函数名</param>
    /// <param name="target">函数目标对象</param>
    public void RegisterFunction(string luaFuncName,string CSname,object target)
    {
        if(!registerList.Contains(luaFuncName)) lua.RegisterFunction(luaFuncName, target, target.GetType().GetMethod(CSname));
    }
    
    /// <summary>
    /// 把字符串用lua解释执行
    /// </summary>
    /// <param name="code"></param>
    public void DoString(string code)
    {
        lua.DoString(code);
    }

    public double GetNumber(string fullPath)
    {
        return lua.GetNumber(fullPath);
    }
    
    /// <summary>
    /// 作为Lua调试使用
    /// </summary>
    /// <param name="content">输出的内容</param>
    public void LPrint(string content)
    {
        Debug.Log(content);
    }
}
