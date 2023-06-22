using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssumptionInsert : MonoBehaviour
{
    private Assumption data;
    
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
            data.pid = tr.GetChild(1).GetComponent<TMP_InputField>().text;
            data.projectName = tr.GetChild(2).GetComponent<TMP_InputField>().text;
            data.rank = int.Parse(tr.GetChild(3).GetComponent<TMP_InputField>().text);
            data.source = tr.GetChild(4).GetComponent<TMP_InputField>().text;
            data.type = (Project.Type)tr.GetChild(5).GetComponent<TMP_Dropdown>().value + 1;
            data.funds = float.Parse(tr.GetChild(6).GetComponent<TMP_InputField>().text);
            data.totalFunds = float.Parse(tr.GetChild(7).GetComponent<TMP_InputField>().text);
            data.start = int.Parse(tr.GetChild(8).GetChild(0).GetComponent<TMP_InputField>().text);
            data.end = int.Parse(tr.GetChild(8).GetChild(1).GetComponent<TMP_InputField>().text);
        }
        catch (Exception)
        {
            ConnectionLogManager.Instance.ReportError(new ArgumentException("Check your input"));
            return;
        }
        string command = $"select * from project where project.pid='{data.pid}'";
        
        MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(command);
        if (reader.Read())
        {
            reader.Close();
            command = $"update project set project.name='{data.projectName}', project.source='{data.source}', project.funds={data.totalFunds}, " +
                      $"project.type={(int)data.type}, project.start={data.start}, project.`end`={data.end} " +
                      $"where project.pid='{data.pid}'; ";
        }
        else
        {
            reader.Close();
            command = $"insert into project values ('{data.pid}', '{data.projectName}', '{data.source}', {data.type}, " +
                      $"{data.totalFunds}, {data.start}, {data.end}); ";
        }
        JobManager.Instance.UpdateInTransaction(command);
        
        command = $"select * from assumption where assumption.pid='{data.pid}' and assumption.tid='{data.tid}'; ";
        reader = ConnectionManager.Instance.ExecuteAndRead(command);
        if (reader.Read())
        {
            reader.Close();
            command = $"update assumption set assumption.`rank`={data.rank}, assumption.funds={data.funds} " +
                      $"where assumption.pid='{data.pid}' and assumption.tid='{data.tid}'; ";
        }
        else
        {
            reader.Close();
            command = $"insert into assumption values ('{data.tid}', '{data.pid}', {data.rank}, {data.funds}); ";
        }
        JobManager.Instance.UpdateInTransaction(command);
        GetComponentInParent<AssumptionTableKeeper>().Query();
    }
}
