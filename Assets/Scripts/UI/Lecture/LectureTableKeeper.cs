using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LectureTableKeeper : MonoBehaviour
{
    private readonly TMP_InputField[] textList = new TMP_InputField[7];
    private TMP_Dropdown[] dropDownList = new TMP_Dropdown[2];
    private Lecture condition;
    private int currentPage = 0;
    private int rowPerPage;
    private int totalPage;
    private float totalHeight;
    private float itemHeight;
    private List<GameObject> objList = new ();
    private TMP_Text pageNumber;
    private GameObject template;
    
    public List<Lecture> itemList = new ();

    public void Start()
    {
        totalPage = 1;
        template = transform.GetChild(2).GetChild(1).gameObject;
        itemHeight = template.GetComponent<RectTransform>().rect.height;
        totalHeight = transform.GetChild(2).GetComponent<RectTransform>().rect.height;
        rowPerPage = Mathf.FloorToInt(totalHeight / itemHeight);
        pageNumber = transform.GetChild(0).GetChild(13).GetComponent<TMP_Text>();
        for (int i = 0; i < rowPerPage; i++)
        {
            objList.Add(Instantiate(template, template.transform.parent));
            objList[i].SetActive(false);
        }
        
        for (int i = 0; i < 4; i++)
        {
            textList[i] = transform.GetChild(1).GetChild(i).GetComponent<TMP_InputField>();
        }
        textList[4] = transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<TMP_InputField>();
        textList[5] = transform.GetChild(1).GetChild(4).GetChild(1).GetComponent<TMP_InputField>();
        textList[6] = transform.GetChild(1).GetChild(6).GetComponent<TMP_InputField>();

        dropDownList[0] = transform.GetChild(1).GetChild(5).GetComponent<TMP_Dropdown>();
        dropDownList[1] = transform.GetChild(1).GetChild(7).GetComponent<TMP_Dropdown>();
        
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
        transform.GetChild(0).GetChild(14).GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.GetChild(3).gameObject.SetActive(true);
        } );
    }
    
    private void LoadParams()
    {
        condition.term = dropDownList[0].value == 0 ? null : (Lecture.Term)dropDownList[0].value;
        condition.type = dropDownList[1].value == 0 ? null : (Course.Type)dropDownList[1].value;
        
        condition.tid = textList[0].text.Length == 0 ? null : textList[0].text;
        condition.cid = textList[1].text.Length == 0 ? null : textList[1].text;
        condition.teacherName = textList[2].text.Length == 0 ? null : textList[2].text;
        condition.courseName = textList[3].text.Length == 0 ? null : textList[3].text;
        condition.queryBegin = textList[4].text.Length == 0 ? null : int.Parse(textList[4].text);
        condition.queryEnd = textList[5].text.Length == 0 ? null : int.Parse(textList[5].text);
        condition.credit = textList[6].text.Length == 0 ? null : int.Parse(textList[6].text);
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
            "select lecture.tid, lecture.cid, lecture.year, lecture.term, lecture.credit, " +
            "teacher.name, course.name, course.type " +
            "from lecture left join teacher on lecture.tid = teacher.tid " +
            "left join course on course.cid = lecture.cid ");
        if (condition.tid != null || condition.cid != null || condition.queryBegin != null || condition.queryEnd != null || condition.term != null ||
             condition.credit != null || condition.teacherName != null || condition.courseName != null || condition.type != null)
        {
            sb.Append("where ");
        }
        else
        {
            sb.Append("and ");
        }

        if (condition.tid != null)
        {
            sb.Append("lecture.tid = '").Append(condition.tid).Append("' and ");
        }

        if (condition.cid != null)
        {
            sb.Append("lecture.cid = '").Append(condition.cid).Append("' and ");
        }
        
        if (condition.queryBegin != null)
        {
            sb.Append("lecture.year >= ").Append(condition.queryBegin).Append(" and ");
        }
        
        if (condition.queryEnd != null)
        {
            sb.Append("lecture.year <= ").Append(condition.queryEnd).Append(" and ");
        }
        
        if (condition.term != null)
        {
            sb.Append("lecture.term = ").Append((int)condition.term).Append(" and ");
        }
        
        if (condition.credit != null)
        {
            sb.Append("lecture.credit = ").Append(condition.credit).Append(" and ");
        }
        
        if (condition.teacherName != null)
        {
            sb.Append("teacher.name = '").Append(condition.teacherName).Append("' and ");
        }
        
        if (condition.courseName != null)
        {
            sb.Append("course.name = '").Append(condition.courseName).Append("' and ");
        }
        
        if (condition.type != null)
        {
            sb.Append("course.type = ").Append((int)condition.type).Append(" and ");
        }

        MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

        while (reader.Read())
        {
            Lecture data = new Lecture
            {
                tid = reader[0].ToString(),
                cid = reader[1].ToString(),
                year = int.Parse(reader[2].ToString()),
                term = (Lecture.Term) int.Parse(reader[3].ToString()),
                credit = int.Parse(reader[4].ToString()),
                teacherName = reader[5].ToString(),
                courseName = reader[6].ToString(),
                type = (Course.Type) int.Parse(reader[7].ToString())
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
                objList[i].GetComponent<LectureRowKeeper>().InitData(itemList[index]);
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
