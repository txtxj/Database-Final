using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LectureInsert : MonoBehaviour
{
    private Lecture data;
    
    private void Start()
    {
        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadParamAndInsert);
        transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void LoadParamAndInsert()
    {
        Transform tr = transform.GetChild(2);
        try
        {
            data.tid = tr.GetChild(0).GetComponent<TMP_InputField>().text;
            data.cid = tr.GetChild(1).GetComponent<TMP_InputField>().text;
            data.year = int.Parse(tr.GetChild(2).GetComponent<TMP_InputField>().text);
            data.term = (Lecture.Term) tr.GetChild(3).GetComponent<TMP_Dropdown>().value + 1;
            data.credit = int.Parse(tr.GetChild(4).GetComponent<TMP_InputField>().text);
        }
        catch (Exception)
        {
            ConnectionLogManager.Instance.ReportError(new ArgumentException("Check your input"));
            return;
        }
        string command = $"insert into lecture values ('{data.tid}', '{data.cid}', {data.year}, {(int)data.term}, {data.credit})";
        
        JobManager.Instance.UpdateInTransaction(command);
        GetComponentInParent<LectureTableKeeper>().Query();
    }
}
