using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublishRowKeeper : MonoBehaviour
{
    private Publish originData;
    private Publish newData;

    public void Start()
    {
        transform.GetChild(0).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.paperName = s;
            OnValueChange();
        });
        transform.GetChild(2).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.paid = int.TryParse(s, out int tmp) ? tmp : originData.paid;
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
        transform.GetChild(5).GetComponent<TMP_Dropdown>().onValueChanged.AddListener(x =>
        {
            newData.author = x == 0;
            OnValueChange();
        });
        transform.GetChild(6).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.source = s;
            OnValueChange();
        });
        transform.GetChild(7).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.date = int.TryParse(s, out int tmp) ? tmp : originData.date;
            OnValueChange();
        });
        transform.GetChild(8).GetComponent<TMP_Dropdown>().onValueChanged.AddListener(x =>
        {
            newData.type = (Paper.Type) (x + 1);
            OnValueChange();
        });
        transform.GetChild(9).GetComponent<TMP_Dropdown>().onValueChanged.AddListener(x =>
        {
            newData.level = (Paper.Level) (x + 1);
            OnValueChange();
        });
        transform.GetChild(10).GetComponentInChildren<Button>().onClick.AddListener(Delete);
    }

    public void InitData(Publish data)
    {
        originData = newData = data;
        Show();
    }

    private void Show()
    {
        transform.GetChild(0).GetComponent<TMP_InputField>().text = originData.paperName;
        transform.GetChild(1).GetComponentInChildren<TMP_Text>().text = originData.teacherName;
        transform.GetChild(2).GetComponent<TMP_InputField>().text = originData.paid.ToString();
        transform.GetChild(3).GetComponent<TMP_InputField>().text = originData.tid;
        transform.GetChild(4).GetComponent<TMP_InputField>().text = originData.rank.ToString();
        transform.GetChild(5).GetComponent<TMP_Dropdown>().value = originData.author == true ? 0 : 1;
        transform.GetChild(6).GetComponent<TMP_InputField>().text = originData.source;
        transform.GetChild(7).GetComponent<TMP_InputField>().text = originData.date.ToString();
        transform.GetChild(8).GetComponent<TMP_Dropdown>().value = (int) (originData.type ?? Paper.Type.FullPaper) - 1;
        transform.GetChild(9).GetComponent<TMP_Dropdown>().value = (int) (originData.level ?? Paper.Level.CcfA) - 1;
    }

    private void OnValueChange()
    {
        JobManager.Instance.UpdateInTransaction(
            $"update publish set tid='{newData.tid}', paid={newData.paid}, `rank`={newData.rank}, author={newData.author} " +
            $"where paid={originData.paid} and `rank`={originData.rank};"
        );
        JobManager.Instance.UpdateInTransaction(
            $"update paper set name='{newData.paperName}', source='{newData.source}', time={newData.date}," +
            $"type={(int) (newData.type ?? Paper.Type.FullPaper)}, level={(int) (newData.level ?? Paper.Level.CcfA)} " +
            $"where paid={originData.paid};"
        );
        originData = newData;
        GetComponentInParent<PublishTableKeeper>().Query();
    }

    private void Delete()
    {
        JobManager.Instance.UpdateInTransaction($"delete from publish where paid={originData.paid} and `rank`={originData.rank};");
        GetComponentInParent<PublishTableKeeper>().Query();
    }
}
