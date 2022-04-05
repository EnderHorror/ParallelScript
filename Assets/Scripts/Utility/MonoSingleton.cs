
using UnityEngine;

/// <summary>
/// Mono脚本单例模式
/// </summary>
/// <typeparam name="T">组件类</typeparam>
public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            GameObject go = new GameObject(typeof(T).Name);
            
            return instance = go.AddComponent<T>();
        }
        protected set => instance = value;
    }
}
