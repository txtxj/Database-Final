using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenPDFQuery : MonoBehaviour
{
    private string path;

    public string Path
    {
        set
        {
            path = value;
            transform.GetChild(1).GetComponent<TMP_Text>().text = "文件路径：" + path;
        }
    }
    
    private void Start()
    {
        transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            OpenFile();
            gameObject.SetActive(false);
        });
        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OpenFile()
    {
        System.Diagnostics.Process.Start(path);
    }
}
