using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using UnityEngine;

[Obsolete]
public class ViewKeeper : MonoBehaviour
{
    public List<Publish> publishList = new List<Publish>();
    public List<Assumption> assumptionList = new List<Assumption>();
    public List<Lecture> lectureList = new List<Lecture>();
    public List<Teacher> teacherList = new List<Teacher>();

    public Publish publishCondition;
    public Assumption assumptionCondition;
    public Lecture lectureCondition;
    public Teacher teacherCondition;

    private Action<bool> loadViewAction;

    public void UpdateViewType(int type)
    {
        loadViewAction = type switch
        {
            0 => LoadPublishView,
            1 => LoadAssumptionView,
            2 => LoadLectureView,
            3 => LoadTeacherView,
            _ => null
        };
        LoadView(false);
    }

    public void LoadView(bool query)
    {
        loadViewAction?.Invoke(query);
    }

    private void LoadPublishView(bool query)
    {
        if (query)
        {
            publishList.Clear();
            StringBuilder sb = new StringBuilder();
            Publish condition = publishCondition;
            sb.Append(
                "select (publish.tid, publish.paid, publish.`rank`, publish.author, teacher.name) " +
                "from publish left join teacher on publish.tid = teacher.tid ");
            if (condition.tid != null || condition.paid != null || condition.rank != null ||
                condition.author != null || condition.teacherName != null)
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

            MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

            while (reader.Read())
            {
                Publish data = new Publish
                {
                    tid = (string) reader[0],
                    paid = int.Parse((string) reader[1]),
                    rank = int.Parse((string) reader[2]),
                    author = bool.Parse((string) reader[3]),
                    teacherName = (string) reader[4]
                };
                publishList.Add(data);
            }
        }
        // Here Show the data
    }

    private void LoadAssumptionView(bool query)
    {
        if (query)
        {
            assumptionList.Clear();
            StringBuilder sb = new StringBuilder();
            Assumption condition = assumptionCondition;
            sb.Append(
                "select (assumption.tid, assumption.pid, assumption.`rank`, assumption.funds, teacher.name) " +
                "from assumption left join teacher on assumption.tid = teacher.tid ");
            if (condition.tid != null || condition.pid != null || condition.rank != null ||
                condition.funds != null || condition.teacherName != null)
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
            
            if (condition.teacherName != null)
            {
                sb.Append("teacher.name = '").Append(condition.teacherName).Append("' and ");
            }

            MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

            while (reader.Read())
            {
                Assumption data = new Assumption
                {
                    tid = (string) reader[0],
                    pid = (string) reader[1],
                    rank = int.Parse((string) reader[2]),
                    funds = float.Parse((string) reader[3]),
                    teacherName = (string) reader[4]
                };
                assumptionList.Add(data);
            }
        }
        // Here Show the data
    }

    private void LoadLectureView(bool query)
    {
        if (query)
        {
            lectureList.Clear();
            StringBuilder sb = new StringBuilder();
            Lecture condition = lectureCondition;
            sb.Append(
                "select (lecture.tid, lecture.cid, lecture.year, lecture.term, lecture.credit, teacher.name) " +
                "from lecture left join teacher on lecture.tid = teacher.tid ");
            if (condition.tid != null || condition.cid != null || condition.year != null ||
                condition.term != null || condition.credit != null || condition.teacherName != null)
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

            MySqlDataReader reader = ConnectionManager.Instance.ExecuteAndRead(sb.ToString()[..^4]);

            while (reader.Read())
            {
                Lecture data = new Lecture
                {
                    tid = (string) reader[0],
                    cid = (string) reader[1],
                    year = int.Parse((string) reader[2]),
                    term = (Lecture.Term)int.Parse((string) reader[3]),
                    credit = int.Parse((string) reader[4]),
                    teacherName = (string) reader[5]
                };
                lectureList.Add(data);
            }
        }
        // Here Show the data
    }

    private void LoadTeacherView(bool query)
    {
        
    }
}
