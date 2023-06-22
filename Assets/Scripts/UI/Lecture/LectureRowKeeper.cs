using System.ComponentModel;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LectureRowKeeper : MonoBehaviour
{
    private Lecture originData;
    private Lecture newData;

    public void Start()
    {
        transform.GetChild(2).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.cid = s;
            OnValueChange();
        });
        transform.GetChild(3).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.tid = s;
            OnValueChange();
        });
        transform.GetChild(4).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.year = int.TryParse(s, out int tmp) ? tmp : originData.year;
            OnValueChange();
        });
        transform.GetChild(5).GetComponent<TMP_Dropdown>().onValueChanged.AddListener(x =>
        {
            newData.term = (Lecture.Term) (x + 1);
            OnValueChange();
        });
        transform.GetChild(6).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.credit = int.TryParse(s, out int tmp) ? tmp : originData.credit;
            OnValueChange();
        });
        transform.GetChild(8).GetComponentInChildren<Button>().onClick.AddListener(Delete);
    }

    public void InitData(Lecture data)
    {
        originData = newData = data;
        Show();
    }

    private void Show()
    {
        transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = originData.courseName;
        transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = originData.teacherName;
        transform.GetChild(2).GetComponent<TMP_InputField>().text = originData.cid;
        transform.GetChild(3).GetComponent<TMP_InputField>().text = originData.tid;
        transform.GetChild(4).GetComponent<TMP_InputField>().text = originData.year.ToString();
        transform.GetChild(5).GetComponent<TMP_Dropdown>().value = (int) (originData.term ?? Lecture.Term.Spring) - 1;
        transform.GetChild(6).GetComponent<TMP_InputField>().text = originData.credit.ToString();
        transform.GetChild(7).GetComponentInChildren<TMP_Text>().text =
            (typeof(Course.Type).GetField(originData.type.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute)?.Description;
    }

    private void OnValueChange()
    {
        JobManager.Instance.UpdateInTransaction(
            $"update lecture set tid='{newData.tid}', cid='{newData.cid}', year={newData.year}, term={(int) (newData.term ?? Lecture.Term.Spring)}, credit={newData.credit} " +
            $"where tid='{originData.tid}' and cid='{originData.cid}' and year={originData.year} and term={(int) (originData.term ?? Lecture.Term.Spring)};"
        );
        originData = newData;
        GetComponentInParent<LectureTableKeeper>().Query();
    }

    private void Delete()
    {
        JobManager.Instance.UpdateInTransaction($"delete from lecture " +
                                                $"where tid='{originData.tid}' and cid='{originData.cid}' and year={originData.year} and term={(int) (originData.term ?? Lecture.Term.Spring)};");
        GetComponentInParent<LectureTableKeeper>().Query();
    }
}
