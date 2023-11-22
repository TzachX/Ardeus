using ADV.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ADVBaseManager
{
#if UNITY_EDITOR
    private bool IsAlive = false;
#endif

    protected ADVGameManager Manager => ADVGameManager.Instance;

    private Action<ADVBaseManager> onCompleteAction;

    protected ADVBaseManager(Action<ADVBaseManager> onComplete)
    {
        onCompleteAction = onComplete;
    }

    protected async void OnInitComplete()
    {
        //TODO: Solve wait hack 
        await Task.Delay(1);

        Debug.Log($"Loaded {this.GetType().FullName}");
        onCompleteAction.Invoke(this);

#if UNITY_EDITOR
        IsAlive = true;
#endif
    }

    public virtual void ResetStats()
    {

    }
}
