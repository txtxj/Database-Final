using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeacherTableKeeper : MonoBehaviour
{
    private readonly TMP_InputField[] textList = new TMP_InputField[2];
    private TMP_Dropdown[] dropDownList = new TMP_Dropdown[2];
    private Teacher condition;
    private int currentPage = 0;
    private int rowPerPage;
    private int totalPage;
    private float totalHeight;
    private float itemHeight;
    private List<GameObject> objList = new ();
    private TMP_Text pageNumber;
    private GameObject template;
    
    public List<Teacher> itemList = new ();

    public void Start()
    {
        totalPage = 1;
        template = transform.GetChild(2).GetChild(1).gameObject;
        itemHeight = template.GetComponent<RectTransform>().rect.height;
        totalHeight = transform.GetChild(2).GetComponent<RectTransform>().rect.height;
        rowPerPage = Mathf.FloorToInt(totalHeight / itemHeight);
        pageNumber = transform.GetChild(0).GetChild(9).GetComponent<TMP_Text>();
        for (int i = 0; i < rowPerPage; i++)
        {
            objList.Add(Instantiate(template, template.transform.parent));
            objList[i].SetActive(false);
        }
        
        textList[0] = transform.GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
        textList[1] = transform.GetChild(1).GetChild(1).GetComponent<TMP_InputField>();

        dropDownList[0] = transform.GetChild(1).GetChild(2).GetComponent<TMP_Dropdown>();
        dropDownList[1] = transform.GetChild(1).GetChild(3).GetComponent<TMP_Dropdown>();
        
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
            currentPage = (currentPage + 1) % totalPage;
            Show();
        } );
        transform.GetChild(0).GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
        {
            currentPage = (currentPage + totalPage - 1) % totalPage;
            Show();
        } );
    }
    
    private void LoadParams()
    {
        condition.gender = dropDownList[0].value == 0 ? null : (Teacher.Gender)dropDownList[0].value;
        condition.title = dropDownList[1].value == 0 ? null : (Teacher.Title)dropDownList[1].value;
        
        condition.tid = textList[0].text.Length == 0 ? null : textList[0].text;
        condition.name = textList[1].text.Length == 0 ? null : textList[1].text;
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
            "select teacher.tid, teacher.name, teacher.gender, teacher.title from teacher ");
        if (condition.tid != null || condition.name != null || condition.gender != null || condition.title != null)
        {
            sb.Append("where ");
        }
        else
        {
            sb.Append("and ");
        }

        if (condition.tid != null)
        {
            sb.Append("teacher.tid = '").Append(condition.tid).Append("' and ");
        }

        if (condition.name != null)
        {
            sb.Append("teacher.name = '").Append(condition.name).Append("' and ");
        }
        
        if (condition.gender != null)
        {
            sb.Append("teacher.gender = ").Append((int)condition.gender).Append(" and ");
        }
        
        if (condition.title != null)
        {
            sb.Append("teacher.title = ").Append((int)condition.title).Append(" and ");
        }

        MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

        while (reader.Read())
        {
            Teacher data = new Teacher
            {
                tid = reader[0].ToString(),
                name = reader[1].ToString(),
                gender = (Teacher.Gender) int.Parse(reader[2].ToString()),
                title = (Teacher.Title) int.Parse(reader[3].ToString())
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
                objList[i].GetComponent<TeacherRowKeeper>().InitData(itemList[index]);
            }
            else
            {
                objList[i].SetActive(false);
            }
        }
    }
    
    public void Export(Teacher data)
    {
        GameObject obj = transform.GetChild(3).gameObject;
        obj.SetActive(true);
        obj.GetComponent<ExportQuery>().Data = data;
    }

    private void OnDestroy()
    {
        objList.ForEach(Destroy);
    }
}
