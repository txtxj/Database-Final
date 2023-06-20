using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SQL Command", menuName = "ScriptableObjects/SQL Command", order = 1)]
public class SQLCommand : ScriptableObject
{
    [TextArea(10, 50)] public string text;

    public string TextUTF8
    {
        get
        {
            byte[] utf16 = System.Text.Encoding.Unicode.GetBytes(text);
            return System.Text.Encoding.UTF8.GetString(utf16);
        }
    }
}
