using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssumptionRowKeeper : MonoBehaviour
{
    private Assumption originData;
    private Assumption newData;

    public void Start()
    {
        transform.GetChild(0).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.projectName = s;
            OnValueChange();
        });
        transform.GetChild(2).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.pid = s;
            OnValueChange();
        });
        transform.GetChild(3).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.tid = s;
            OnValueChange();
        });
        transform.GetChild(4).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.rank = int.TryParse(s, out int tmp) ? tmp : originData.rank;
            OnValueChange();
        });
        transform.GetChild(5).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.source = s;
            OnValueChange();
        });
        transform.GetChild(6).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.start = int.TryParse(s, out int tmp) ? tmp : originData.start;
            OnValueChange();
        });
        transform.GetChild(7).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.end = int.TryParse(s, out int tmp) ? tmp : originData.end;
            OnValueChange();
        });
        transform.GetChild(8).GetComponent<TMP_Dropdown>().onValueChanged.AddListener(x =>
        {
            newData.type = (Project.Type) (x + 1);
            OnValueChange();
        });
        transform.GetChild(9).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.funds = float.TryParse(s, out float tmp) ? tmp : originData.funds;
            OnValueChange();
        });
        transform.GetChild(10).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.totalFunds = float.TryParse(s, out float tmp) ? tmp : originData.totalFunds;
            OnValueChange();
        });
        transform.GetChild(11).GetComponentInChildren<Button>().onClick.AddListener(Delete);
    }

    public void InitData(Assumption data)
    {
        originData = newData = data;
        Show();
    }

    private void Show()
    {
        transform.GetChild(0).GetComponent<TMP_InputField>().text = originData.projectName;
        transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = originData.teacherName;
        transform.GetChild(2).GetComponent<TMP_InputField>().text = originData.pid;
        transform.GetChild(3).GetComponent<TMP_InputField>().text = originData.tid;
        transform.GetChild(4).GetComponent<TMP_InputField>().text = originData.rank.ToString();
        transform.GetChild(5).GetComponent<TMP_InputField>().text = originData.source;
        transform.GetChild(6).GetComponent<TMP_InputField>().text = originData.start.ToString();
        transform.GetChild(7).GetComponent<TMP_InputField>().text = originData.end.ToString();
        transform.GetChild(8).GetComponent<TMP_Dropdown>().value = (int) (originData.type ?? Project.Type.National) - 1;
        transform.GetChild(9).GetComponent<TMP_InputField>().text = originData.funds.ToString();
        transform.GetChild(10).GetComponent<TMP_InputField>().text = originData.totalFunds.ToString();
    }

    private void OnValueChange()
    {
        JobManager.Instance.UpdateInTransaction(
            $"update assumption set tid='{newData.tid}', pid='{newData.pid}', `rank`={newData.rank}, funds={newData.funds} " +
            $"where pid='{originData.pid}' and `rank`={originData.rank};"
        );
        JobManager.Instance.UpdateInTransaction(
            $"update project set name='{newData.projectName}', source='{newData.source}', funds={newData.totalFunds}," +
            $"type={(int) (newData.type ?? Project.Type.National)}, start={newData.start}, `end`={newData.end} " +
            $"where pid='{originData.pid}';"
        );
        originData = newData;
        GetComponentInParent<AssumptionTableKeeper>().Query();
    }

    private void Delete()
    {
        JobManager.Instance.UpdateInTransaction($"delete from assumption where pid='{originData.pid}' and `rank`={originData.rank};");
        GetComponentInParent<AssumptionTableKeeper>().Query();
    }
}
