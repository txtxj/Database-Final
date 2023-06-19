using System;
using System.Collections.Generic;

public class ConnectionLogManager : MonoSingleton<ConnectionLogManager>
{
    public List<Exception> errorList = new List<Exception>();
    public List<string> logList = new List<string>();
}
