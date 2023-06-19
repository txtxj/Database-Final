using System;
using UnityEngine;
using MySql.Data.MySqlClient;
using Object = UnityEngine.Object;

public class ConnectionManager : MonoSingleton<ConnectionManager>
{
    public MySqlConnection connection;
    private MySqlTransaction transaction;

    public bool isOpen = false;
    
    private void Start()
    {
        connection = new MySqlConnection("server=localhost;User Id=root;password=1234;Database=TARRS;charset=utf8");
        try
        {
            connection.Open();
            isOpen = true;
        
            TextAsset init1 = Resources.Load<TextAsset>("InitDatabaseAndTables");
            ExecuteNonQuery(init1.text);
            TextAsset init2 = Resources.Load<TextAsset>("InitRelationTables");
            ExecuteNonQuery(init2.text);
        }
        catch (Exception e)
        {
            ConnectionLogManager.Instance.errorList.Add(e);
            Debug.LogError(e);
            isOpen = false;
        }
        ConnectionLogManager.Instance.logList.Add("初始化成功");
    }

    public int ExecuteNonQuery(string command)
    {
        if (!isOpen)
        {
            return 0;
        }
        MySqlCommand cmd = new MySqlCommand(command, connection);
        return cmd.ExecuteNonQuery();
    }
    
    public MySqlDataReader ExecuteAndRead(string command)
    {
        if (!isOpen)
        {
            return null;
        }
        MySqlCommand cmd = new MySqlCommand(command, connection);
        return cmd.ExecuteReader();
    }

    public void StartTransaction()
    {
        transaction = connection.BeginTransaction();
    }

    public void CommitTransaction()
    {
        transaction.Commit();
    }

    public void RollbackTransaction()
    {
        transaction.Rollback();
    }
    
    public void CheckLectures()
    {
        // 遍历 lecture 表，聚合每一个 (cid, year, term) 的学时，在 course 表中检查聚合学时是否等于 credit
    }

    public void CheckAssumptions()
    {
        // 已将 (pid, `rank`) 作为主键，不存在排名重复的情况
        // 遍历 assumption 表，聚合每一个 pid，在 project 表中检查聚合经费是否等于 funds
    }

    public void CheckPublishes()
    {
        // 遍历 publish 表，获取所有 paid，再依次搜索 (paid, author=true)，保证仅搜索出一个结果
    }
}
