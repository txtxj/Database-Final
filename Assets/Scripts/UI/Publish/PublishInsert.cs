using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublishInsert : MonoBehaviour
{
    private Publish data;
    
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
            data.paid = int.Parse(tr.GetChild(1).GetComponent<TMP_InputField>().text);
            data.paperName = tr.GetChild(2).GetComponent<TMP_InputField>().text;
            data.rank = int.Parse(tr.GetChild(3).GetComponent<TMP_InputField>().text);
            data.author = tr.GetChild(4).GetComponent<TMP_Dropdown>().value == 0;
            data.source = tr.GetChild(5).GetComponent<TMP_InputField>().text;
            data.date = int.Parse(tr.GetChild(6).GetComponent<TMP_InputField>().text);
            data.type = (Paper.Type) tr.GetChild(7).GetComponent<TMP_Dropdown>().value + 1;
            data.level = (Paper.Level) tr.GetChild(8).GetComponent<TMP_Dropdown>().value + 1;
        }
        catch (Exception)
        {
            ConnectionLogManager.Instance.ReportError(new ArgumentException("Check your input"));
            return;
        }
        string command = $"select * from paper where paper.paid={data.paid}";
        
        MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(command);
        if (reader.Read())
        {
            reader.Close();
            command = $"update paper set paper.name='{data.paperName}', paper.source='{data.source}', paper.time={data.date}, " +
                      $"paper.type={(int)data.type}, paper.level={(int)data.level} " +
                      $"where paper.paid={data.paid}; ";
        }
        else
        {
            reader.Close();
            command = $"insert into paper values ({data.paid}, '{data.paperName}', '{data.source}', {data.date}, " +
                      $"{(int)data.type}, {(int)data.level}); ";
        }
        JobManager.Instance.UpdateInTransaction(command);
        
        command = $"select * from publish where publish.paid={data.paid} and publish.tid={data.tid}; ";
        reader = ConnectionManager.Instance.ExecuteAndRead(command);
        if (reader.Read())
        {
            reader.Close();
            command = $"update publish set publish.`rank`={data.rank}, publish.author={data.author} " +
                      $"where publish.paid={data.paid} and publish.tid={data.tid}; ";
        }
        else
        {
            reader.Close();
            command = $"insert into publish values ('{data.tid}', {data.paid}, {data.rank}, {data.author}); ";
        }
        JobManager.Instance.UpdateInTransaction(command);
        GetComponentInParent<PublishTableKeeper>().Query();
    }
}
