using System;
using System.Text;
using UnityEngine;
using MySql.Data.MySqlClient;

public class ConnectionManager : MonoSingleton<ConnectionManager>
{
    private MySqlConnection connection;
    private MySqlTransaction transaction;
    private const string ConnectionStr =
        "server=localhost;port=3306;User Id=root;password=1234;Database=TARRS;charset=utf8mb4;allowuservariables=true;";

    internal bool isOpen = false;
    internal bool isTransaction = false;

    private void Start()
    {
        connection = new MySqlConnection(ConnectionStr);
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
            ConnectionLogManager.Instance.ReportError(e);
            isOpen = false;
            return;
        }
        ConnectionLogManager.Instance.ReportLog("初始化成功");
    }

    internal int ExecuteNonQuery(string command)
    {
        if (!isOpen)
        {
            return 0;
        }
        MySqlCommand cmd = new MySqlCommand(command, connection);
        if (isTransaction)
        {
            cmd.Transaction = transaction;
        }
        return cmd.ExecuteNonQuery();
    }
    
    public MySqlDataReader ExecuteAndRead(string command, bool strictNonTransaction = false)
    {
        if (!isOpen)
        {
            return null;
        }
        MySqlCommand cmd = new MySqlCommand(command, connection);
        if (isTransaction && strictNonTransaction == false)
        {
            cmd.Transaction = transaction;
        }
        return cmd.ExecuteReader();
    }
    
    private bool ExecuteAndReadBool(string command)
    {
        if (!isOpen)
        {
            return default;
        }
        MySqlCommand cmd = new MySqlCommand(command, connection);
        if (isTransaction)
        {
            cmd.Transaction = transaction;
        }
        return int.Parse(cmd.ExecuteScalar().ToString()) == 1;
    }

    internal void StartTransaction()
    {
        isTransaction = true;
        Debug.Log("Start transaction");
        transaction = connection.BeginTransaction();
    }

    internal void CommitTransaction()
    {
        isTransaction = false;
        Debug.Log("Commit transaction");
        transaction.Commit();
    }

    internal void RollbackTransaction()
    {
        isTransaction = false;
        Debug.Log("Rollback transaction");
        transaction.Rollback();
    }
    
    internal void CheckLectures()
    {
        TextAsset checker = Resources.Load<TextAsset>("LectureChecker");
        if (ExecuteAndReadBool(checker.text))
        {
            throw new Exception("Lecture Check Fail");
        }
    }

    internal void CheckAssumptions()
    {
        // 遍历 assumption 表，聚合每一个 pid，在 project 表中检查聚合经费是否等于 funds
        TextAsset checker = Resources.Load<TextAsset>("AssumptionChecker");
        if (ExecuteAndReadBool(checker.text))
        {
            throw new Exception("Assumption Check Fail");
        }
    }

    internal void CheckPublishes()
    {
        // 遍历 publish 表，获取所有 paid，再依次搜索 (paid, author=true)，保证仅搜索出一个结果
        TextAsset checker = Resources.Load<TextAsset>("PublishChecker");
        if (ExecuteAndReadBool(checker.text))
        {
            throw new Exception("Publish Check Fail");
        }
    }

    public void Close()
    {
        connection.Close();
    }
}
