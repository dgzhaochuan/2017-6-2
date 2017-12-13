

using UnityEngine;

public abstract class BaseManaget: MonoBehaviour
{
    public abstract void OnInit();
    public virtual void Close() { Destroy(gameObject); }
}