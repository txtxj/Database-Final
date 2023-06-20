using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublishTableKeeper : MonoBehaviour, ITableKeeper
{
    private readonly TMP_InputField[] textList = new TMP_InputField[4];
    private TMP_Dropdown dropdown;
    private Publish condition;
    
    public List<Publish> itemList = new ();

    public void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            textList[i] = transform.GetChild(1).GetChild(i).GetComponent<TMP_InputField>();
        }

        dropdown = GetComponentInChildren<TMP_Dropdown>();
        
        transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(Query);
        transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(Commit);
        transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Rollback);
    }
    
    public void LoadParams()
    {
        condition.author = dropdown.value switch
        {
            1 => true,
            2 => false,
            _ => null
        };
        condition.name = textList[2].text.Length == 0 ? null : textList[2].text;
        condition.paid = textList[1].text.Length == 0 ? null : int.Parse(textList[1].text);
        condition.rank = textList[3].text.Length == 0 ? null : int.Parse(textList[3].text);
        condition.tid = textList[0].text.Length == 0 ? null : textList[0].text;
    }

    public void Commit() => JobManager.Instance.CommitAndCheck();

    public void Rollback() => JobManager.Instance.RollBack();

    public void Query()
    {
        LoadParams();
        itemList.Clear();
        StringBuilder sb = new StringBuilder();
        sb.Append(
            "select publish.tid, publish.paid, publish.`rank`, publish.author, teacher.name " +
            "from publish left join teacher on publish.tid = teacher.tid ");
        if (condition.tid != null || condition.paid != null || condition.rank != null ||
            condition.author != null || condition.name != null)
        {
            sb.Append("where ");
        }
        else
        {
            sb.Append("and ");
        }

        if (condition.tid != null)
        {
            sb.Append("publish.tid = '").Append(condition.tid).Append("' and ");
        }

        if (condition.paid != null)
        {
            sb.Append("publish.paid = ").Append(condition.paid).Append(" and ");
        }
        
        if (condition.rank != null)
        {
            sb.Append("publish.`rank` = ").Append(condition.rank).Append(" and ");
        }
        
        if (condition.author != null)
        {
            sb.Append("publish.author = ").Append(condition.author).Append(" and ");
        }
        
        if (condition.name != null)
        {
            sb.Append("teacher.name = '").Append(condition.name).Append("' and ");
        }

        MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

        while (reader.Read())
        {
            Publish data = new Publish
            {
                tid = reader[0].ToString(),
                paid = int.Parse(reader[1].ToString()),
                rank = int.Parse(reader[2].ToString()),
                author = bool.Parse(reader[3].ToString()),
                name = reader[4].ToString()
            };
            itemList.Add(data);
        }
    }

    public void Show()
    {
        
    }
}
