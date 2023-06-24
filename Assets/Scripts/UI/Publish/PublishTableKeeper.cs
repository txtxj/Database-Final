using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublishTableKeeper : MonoBehaviour
{
    private readonly TMP_InputField[] textList = new TMP_InputField[7];
    private TMP_Dropdown[] dropdownList = new TMP_Dropdown[3];
    private Publish condition;
    private int currentPage = 0;
    private int rowPerPage;
    private int totalPage;
    private float totalHeight;
    private float itemHeight;
    private List<GameObject> objList = new ();
    private TMP_Text pageNumber;
    private GameObject template;
    
    public List<Publish> itemList = new ();

    public void Start()
    {
        totalPage = 1;
        template = transform.GetChild(2).GetChild(1).gameObject;
        itemHeight = template.GetComponent<RectTransform>().rect.height;
        totalHeight = transform.GetChild(2).GetComponent<RectTransform>().rect.height - transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().rect.height;
        rowPerPage = Mathf.FloorToInt(totalHeight / itemHeight);
        pageNumber = transform.GetChild(0).GetChild(14).GetComponent<TMP_Text>();
        for (int i = 0; i < rowPerPage; i++)
        {
            objList.Add(Instantiate(template, template.transform.parent));
            objList[i].SetActive(false);
        }
        
        for (int i = 0; i < 4; i++)
        {
            textList[i] = transform.GetChild(1).GetChild(i).GetComponent<TMP_InputField>();
        }
        textList[4] = transform.GetChild(1).GetChild(5).GetComponent<TMP_InputField>();
        textList[5] = transform.GetChild(1).GetChild(6).GetChild(0).GetComponent<TMP_InputField>();
        textList[6] = transform.GetChild(1).GetChild(6).GetChild(1).GetComponent<TMP_InputField>();

        dropdownList[0] = transform.GetChild(1).GetChild(4).GetComponent<TMP_Dropdown>();
        dropdownList[1] = transform.GetChild(1).GetChild(7).GetComponent<TMP_Dropdown>();
        dropdownList[2] = transform.GetChild(1).GetChild(8).GetComponent<TMP_Dropdown>();
        
        transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            try
            {
                LoadParams();
            }
            catch (Exception)
            {
                ConnectionLogManager.Instance.ReportError(new ArgumentException("Check your input"));
                return;
            }
            Query();
        });
        transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(Commit);
        transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Rollback);
        transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPage = (currentPage + totalPage - 1) % totalPage;
            Show();
        } );
        transform.GetChild(0).GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPage = (currentPage + 1) % totalPage;
            Show();
        } );
        transform.GetChild(0).GetChild(15).GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.GetChild(3).gameObject.SetActive(true);
        } );
    }
    
    private void LoadParams()
    {
        condition.author = dropdownList[0].value == 0 ? null : dropdownList[0].value == 1;
        condition.type = dropdownList[1].value == 0 ? null : (Paper.Type)dropdownList[1].value;
        condition.level = dropdownList[2].value == 0 ? null : (Paper.Level)dropdownList[2].value;
        
        condition.tid = textList[0].text.Length == 0 ? null : textList[0].text;
        condition.paid = textList[1].text.Length == 0 ? null : int.Parse(textList[1].text);
        condition.teacherName = textList[2].text.Length == 0 ? null : textList[2].text;
        condition.rank = textList[3].text.Length == 0 ? null : int.Parse(textList[3].text);
        condition.source = textList[4].text.Length == 0 ? null : textList[4].text;
        condition.queryBegin = textList[5].text.Length == 0 ? null : int.Parse(textList[5].text);
        condition.queryEnd = textList[6].text.Length == 0 ? null : int.Parse(textList[6].text);
    }

    public void Commit()
    {
        JobManager.Instance.CommitAndCheck();
        Query();
    }

    public void Rollback()
    {
        JobManager.Instance.RollBack();
        Query();
    }

    public void Query()
    {
        itemList.Clear();
        StringBuilder sb = new StringBuilder();
        sb.Append(
            "select publish.tid, publish.paid, publish.`rank`, publish.author, teacher.name, " +
            "paper.name, paper.source, paper.time, paper.type, paper.level " +
            "from publish left join teacher on publish.tid = teacher.tid left join paper on paper.paid = publish.paid ");
        if (condition.tid != null || condition.paid != null || condition.rank != null ||
            condition.author != null || condition.teacherName != null || condition.source != null ||
            condition.queryBegin != null || condition.queryEnd != null || condition.type != null || condition.level != null)
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
        
        if (condition.teacherName != null)
        {
            sb.Append("teacher.name = '").Append(condition.teacherName).Append("' and ");
        }
        
        if (condition.source != null)
        {
            sb.Append("paper.source = '").Append(condition.source).Append("' and ");
        }
        
        if (condition.queryBegin != null)
        {
            sb.Append("paper.time >= ").Append(condition.queryBegin).Append(" and ");
        }
        
        if (condition.queryEnd != null)
        {
            sb.Append("paper.time <= ").Append(condition.queryEnd).Append(" and ");
        }
        
        if (condition.type != null)
        {
            sb.Append("paper.type = ").Append((int) condition.type).Append(" and ");
        }
        
        if (condition.level != null)
        {
            sb.Append("paper.level = ").Append((int) condition.level).Append(" and ");
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
                teacherName = reader[4].ToString(),
                paperName = reader[5].ToString(),
                source = reader[6].ToString(),
                date = int.Parse(reader[7].ToString()),
                type = (Paper.Type) int.Parse(reader[8].ToString()),
                level = (Paper.Level) int.Parse(reader[9].ToString())
            };
            itemList.Add(data);
        }
        reader.Close();
        totalPage = (itemList.Count + rowPerPage - 1) / rowPerPage;
        if (totalPage <= 0)
        {
            totalPage = 1;
        }
        Show();
    }

    public void Show()
    {
        if (currentPage >= totalPage)
        {
            currentPage = 0;
        }
        pageNumber.text = $"{currentPage + 1}/{totalPage}";
        for (int i = 0; i < rowPerPage; i++)
        {
            int index = currentPage * rowPerPage + i;
            if (index < itemList.Count)
            {
                objList[i].SetActive(true);
                objList[i].GetComponent<PublishRowKeeper>().InitData(itemList[index]);
            }
            else
            {
                objList[i].SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        objList.ForEach(Destroy);
    }
}
