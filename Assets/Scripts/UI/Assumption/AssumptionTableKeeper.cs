using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssumptionTableKeeper : MonoBehaviour
{
    private readonly TMP_InputField[] textList = new TMP_InputField[10];
    private TMP_Dropdown[] dropdownList = new TMP_Dropdown[1];
    private Assumption condition;
    private int currentPage = 0;
    private int rowPerPage;
    private int totalPage;
    private float totalHeight;
    private float itemHeight;
    private List<GameObject> objList = new ();
    private TMP_Text pageNumber;
    private GameObject template;
    
    public List<Assumption> itemList = new ();

    public void Start()
    {
        totalPage = 1;
        template = transform.GetChild(2).GetChild(1).gameObject;
        itemHeight = template.GetComponent<RectTransform>().rect.height;
        totalHeight = transform.GetChild(2).GetComponent<RectTransform>().rect.height - transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().rect.height;
        rowPerPage = Mathf.FloorToInt(totalHeight / itemHeight);
        pageNumber = transform.GetChild(0).GetChild(16).GetComponent<TMP_Text>();
        for (int i = 0; i < rowPerPage; i++)
        {
            objList.Add(Instantiate(template, template.transform.parent));
            objList[i].SetActive(false);
        }
        
        for (int i = 0; i < 9; i++)
        {
            textList[i] = transform.GetChild(1).GetChild(i).GetComponent<TMP_InputField>();
        }
        textList[9] = transform.GetChild(1).GetChild(10).GetComponent<TMP_InputField>();

        dropdownList[0] = transform.GetChild(1).GetChild(9).GetComponent<TMP_Dropdown>();
        
        transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            LoadParams();
            Query();
        });
        transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(Commit);
        transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Rollback);
        transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPage = (currentPage + 1) % totalPage;
            Show();
        } );
        transform.GetChild(0).GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPage = (currentPage + totalPage - 1) % totalPage;
            Show();
        } );
        transform.GetChild(0).GetChild(17).GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.GetChild(3).gameObject.SetActive(true);
        } );
    }
    
    private void LoadParams()
    {
        condition.type = dropdownList[0].value == 0 ? null : (Project.Type)dropdownList[0].value;
        condition.tid = textList[0].text.Length == 0 ? null : textList[0].text;
        condition.pid = textList[1].text.Length == 0 ? null : textList[1].text;
        condition.teacherName = textList[2].text.Length == 0 ? null : textList[2].text;
        condition.projectName = textList[3].text.Length == 0 ? null : textList[3].text;
        condition.rank = textList[4].text.Length == 0 ? null : int.Parse(textList[4].text);
        condition.funds = textList[5].text.Length == 0 ? null : float.Parse(textList[5].text);
        condition.source = textList[6].text.Length == 0 ? null : textList[6].text;
        condition.start = textList[7].text.Length == 0 ? null : int.Parse(textList[7].text);
        condition.end = textList[8].text.Length == 0 ? null : int.Parse(textList[8].text);
        condition.totalFunds = textList[9].text.Length == 0 ? null : float.Parse(textList[9].text);
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
            value: "select assumption.tid, assumption.pid, assumption.`rank`, assumption.funds, teacher.name, " +
                   "project.name, project.source, project.type, project.funds, project.start, project.`end` " +
                   "from assumption left join teacher on assumption.tid = teacher.tid left join project on project.pid = assumption.pid ");
        if (condition.tid != null || condition.pid != null || condition.rank != null || condition.funds != null ||
            condition.projectName != null || condition.teacherName != null || condition.source != null ||
            condition.start != null || condition.end != null || condition.totalFunds != null || condition.type != null)
        {
            sb.Append("where ");
        }
        else
        {
            sb.Append("and ");
        }

        if (condition.tid != null)
        {
            sb.Append("assumption.tid = '").Append(condition.tid).Append("' and ");
        }

        if (condition.pid != null)
        {
            sb.Append("assumption.pid = '").Append(condition.pid).Append("' and ");
        }
        
        if (condition.rank != null)
        {
            sb.Append("assumption.`rank` = ").Append(condition.rank).Append(" and ");
        }
        
        if (condition.funds != null)
        {
            sb.Append("assumption.funds = ").Append(condition.funds).Append(" and ");
        }
        
        if (condition.projectName != null)
        {
            sb.Append("project.name = '").Append(condition.projectName).Append("' and ");
        }
        
        if (condition.teacherName != null)
        {
            sb.Append("teacher.name = '").Append(condition.teacherName).Append("' and ");
        }
        
        if (condition.source != null)
        {
            sb.Append("assumption.source = '").Append(condition.source).Append("' and ");
        }
        
        if (condition.start != null)
        {
            sb.Append("project.start = ").Append(condition.start).Append(" and ");
        }
        
        if (condition.end != null)
        {
            sb.Append("project.`end` = ").Append(condition.end).Append(" and ");
        }
        
        if (condition.funds != null)
        {
            sb.Append("project.funds = ").Append(condition.funds).Append(" and ");
        }
        
        if (condition.type != null)
        {
            sb.Append("project.type = ").Append((int) condition.type).Append(" and ");
        }

        MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

        while (reader.Read())
        {
            Assumption data = new Assumption
            {
                tid = reader[0].ToString(),
                pid = reader[1].ToString(),
                rank = int.Parse(reader[2].ToString()),
                funds = float.Parse(reader[3].ToString()),
                teacherName = reader[4].ToString(),
                projectName = reader[5].ToString(),
                source = reader[6].ToString(),
                type = (Project.Type) int.Parse(reader[7].ToString()),
                totalFunds = float.Parse(reader[8].ToString()),
                start = int.Parse(reader[9].ToString()),
                end = int.Parse(reader[10].ToString()),
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
                objList[i].GetComponent<AssumptionRowKeeper>().InitData(itemList[index]);
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
