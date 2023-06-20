using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PageButtonData : MonoBehaviour
{
    public int index;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => PageKeeper.Instance.CurrentPage = index);
    }
}
