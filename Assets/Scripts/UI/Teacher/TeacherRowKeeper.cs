using System.ComponentModel;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class TeacherRowKeeper : MonoBehaviour
{
    private Teacher data;

    public void Start()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(Export);
    }

    public void InitData(Teacher data)
    {
        this.data = data;
        Show();
    }

    private void Show()
    {
        transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = data.tid;
        transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = data.name;
        transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = (typeof(Teacher.Gender).GetField(data.gender.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute)?.Description;
        transform.GetChild(3).GetComponentInChildren<TMP_Text>().text = (typeof(Teacher.Title).GetField(data.title.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute)?.Description;
    }

    private void Export()
    {
        Process p = Process.Start("program.exe");
    }
}
