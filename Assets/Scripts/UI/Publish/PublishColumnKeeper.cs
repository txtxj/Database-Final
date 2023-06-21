using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublishColumnKeeper : MonoBehaviour
{
    private Publish originData;
    private Publish newData;

    public void Start()
    {
        transform.GetChild(1).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.tid = s;
            OnValueChange();
        });
        transform.GetChild(2).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.paid = int.TryParse(s, out int tmp) ? tmp : 0;
            OnValueChange();
        });
        transform.GetChild(3).GetComponent<TMP_InputField>().onEndEdit.AddListener(s =>
        {
            newData.rank = int.TryParse(s, out int tmp) ? tmp : 0;
            OnValueChange();
        });
        transform.GetChild(4).GetComponent<TMP_Dropdown>().onValueChanged.AddListener(x =>
        {
            newData.author = x == 0;
            OnValueChange();
        });
        transform.GetChild(5).GetComponentInChildren<Button>().onClick.AddListener(Delete);
    }

    public void InitData(Publish data)
    {
        originData = newData = data;
        Show();
    }

    private void Show()
    {
        transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = originData.name;
        transform.GetChild(1).GetComponent<TMP_InputField>().text = originData.tid;
        transform.GetChild(2).GetComponent<TMP_InputField>().text = originData.paid.ToString();
        transform.GetChild(3).GetComponent<TMP_InputField>().text = originData.rank.ToString();
        transform.GetChild(4).GetComponent<TMP_Dropdown>().value = originData.author == true ? 0 : 1;
    }

    private void OnValueChange()
    {
        JobManager.Instance.UpdateInTransaction(
            $"update publish set tid='{newData.tid}', paid={newData.paid}, `rank`={newData.rank}, author={newData.author} " +
            $"where paid={originData.paid} and `rank`={originData.rank};"
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
