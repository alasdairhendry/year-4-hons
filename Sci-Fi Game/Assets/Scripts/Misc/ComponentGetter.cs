using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentG<T> where T : Component 
{
    private void Check ()
    {
        if (!component)
            component = obj.GetComponent<T> ();
    }

    public T Get ()
    {
        Check ();
        return component;
    }

    

    public T component;
    private MonoBehaviour obj;

    public ComponentG (MonoBehaviour obj)
    {
        this.obj = obj;
    }
}
