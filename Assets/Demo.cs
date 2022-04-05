using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    private Dialog dialog;
    void Start()
    {
        dialog = GetComponentInChildren<Dialog>();
        LuaEventCenter.Instance.RegisterFunction("shake", "Shake", this);
    }

    public void Demo1()
    {
        dialog.Play("Hello World");
    }
    public void Demo2()
    {
        dialog.Play("<anim:Rainbow>Hello</anim> <anim:Wobble>World</anim>");
    }
    public void Demo3()
    {
        dialog.Play("<lua:print(\"Hello\")> lua½Å±¾Ö´ÐÐ (run lua script)");
    }
    public void Demo4()
    {
        dialog.Play("H<lua:w=1>e<lua:w=1>l<lua:w=1>l<lua:w=1>o World");
    }
    public void Demo5()
    {
        dialog.Play("StartShake<lua:shake()>");
    }

    public void Shake()
    {
        StartCoroutine(CamaraShake());
    }

    IEnumerator CamaraShake()
    {
        print("shake");
        for(int i = 0;i<10;i++)
        {
            transform.GetChild(0).Translate(new Vector3(Random.Range(-10, 10),Random.Range(-10, 10)));
            yield return new WaitForSeconds(0.2f);
        }
    }
}
