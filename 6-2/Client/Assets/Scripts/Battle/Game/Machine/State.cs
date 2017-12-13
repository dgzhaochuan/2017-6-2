public abstract  class State:UnityEngine.MonoBehaviour
{
    public virtual void Enter(object[] obj)
    {
        AddListeners();
    }

    public virtual void Exit( )
    {
        Extend.ClosePrompt();
        RemoveListeners();
    }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    protected virtual void AddListeners()
    {

    }

    protected virtual void RemoveListeners()
    {

    }
}