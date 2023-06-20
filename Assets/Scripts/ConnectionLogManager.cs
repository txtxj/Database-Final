using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionLogManager : MonoSingleton<ConnectionLogManager>
{
    private List<Exception> errorList = new List<Exception>();
    private List<string> logList = new List<string>();

    public void ReportError(Exception e)
    {
        errorList.Add(e);
        Debug.LogError(e);
    }

    public void ReportLog(string e)
    {
        logList.Add(e);
        Debug.Log(e);
    }

    public void ClearError() => errorList.Clear();

    public void ClearLog() => logList.Clear();

    public void Clear()
    {
        ClearError();
        ClearLog();
    }
}
