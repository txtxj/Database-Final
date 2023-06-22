using System;
using UnityEngine;

public class ConnectionLogManager : MonoSingleton<ConnectionLogManager>
{
    public ErrorWindow errorWindow;
    
    public void ReportError(Exception e)
    {
        errorWindow.SetError(e.Message);
        errorWindow.gameObject.SetActive(true);
        Debug.LogError(e);
    }

    public void ReportLog(string e)
    {
        Debug.Log(e);
    }

    public void ClearError() => throw null!;

    public void ClearLog() => throw null!;

    public void Clear()
    {
        ClearError();
        ClearLog();
    }
}
