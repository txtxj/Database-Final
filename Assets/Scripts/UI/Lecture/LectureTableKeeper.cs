using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LectureTableKeeper : MonoBehaviour
{
    private readonly TMP_InputField[] textList = new TMP_InputField[6];
    private TMP_Dropdown dropdown;
    private Lecture condition;
    private int currentPage = 0;
    private int columnPerPage = 10;
    private int totalPage;
    private List<GameObject> objList = new ();

    public GameObject template;
    public List<Lecture> itemList = new ();

    public void Start()
    {
        template = transform.GetChild(2).GetChild(1).gameObject;
        for (int i = 0; i < columnPerPage; i++)
        {
            objList.Add(Instantiate(template, template.transform.parent));
            objList[i].SetActive(false);
        }
        
        for (int i = 0; i < 5; i++)
        {
            textList[i] = transform.GetChild(1).GetChild(i).GetComponent<TMP_InputField>();
        }
        textList[5] = transform.GetChild(1).GetChild(6).GetComponent<TMP_InputField>();

        dropdown = GetComponentInChildren<TMP_Dropdown>();
        
        transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            LoadParams();
            Query();
        });
        transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(Commit);
        transform.GetChild(0).GetChild(2).GetComponent<Button>().onClick.AddListener(Rollback);
    }
    
    private void LoadParams()
    {
        condition.term = dropdown.value switch
        {
            1 => Lecture.Term.Spring,
            2 => Lecture.Term.Summer,
            3 => Lecture.Term.Fall,
            _ => null
        };
        condition.teacherName = textList[2].text.Length == 0 ? null : textList[2].text;
        condition.courseName = textList[3].text.Length == 0 ? null : textList[3].text;
        condition.credit = textList[5].text.Length == 0 ? null : int.Parse(textList[5].text);
        condition.year = textList[4].text.Length == 0 ? null : int.Parse(textList[4].text);
        condition.tid = textList[0].text.Length == 0 ? null : textList[0].text;
        condition.cid = textList[1].text.Length == 0 ? null : textList[1].text;
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
            "teacher.name, course.name " +
            "from lecture left join teacher on lecture.tid = teacher.tid " +
            "left join course on course.cid = lecture.cid ");
        if (condition.tid != null || condition.cid != null || condition.year != null || condition.term != null ||
             condition.credit != null || condition.teacherName != null || condition.courseName != null)
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
        
        if (condition.year != null)
        {
            sb.Append("lecture.year = ").Append(condition.year).Append(" and ");
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

        MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

        while (reader.Read())
        {
            Lecture data = new Lecture
            {
                tid = reader[0].ToString(),
                cid = reader[1].ToString(),
                year = int.Parse(reader[2].ToString()),
                term = (Lecture.Term)int.Parse(reader[3].ToString()),
                credit = int.Parse(reader[4].ToString()),
                teacherName = reader[5].ToString(),
                courseName = reader[6].ToString()
            };
            itemList.Add(data);
        }
        reader.Close();
        Show();
    }

    public void Show()
    {
        totalPage = (itemList.Count + columnPerPage - 1) / columnPerPage;
        for (int i = 0; i < columnPerPage; i++)
        {
            int index = currentPage * columnPerPage + i;
            if (index < itemList.Count)
            {
                objList[i].SetActive(true);
                objList[i].GetComponent<LectureColumnKeeper>().InitData(itemList[index]);
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
