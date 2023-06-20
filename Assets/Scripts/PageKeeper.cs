using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(ViewKeeper))]
public class PageKeeper : MonoSingleton<PageKeeper>
{
    private int currentPage;
    private readonly List<GameObject> buttonList = new List<GameObject>();
    private GameObject mainPage;

    public Sprite inactiveSprite;
    public Sprite activeSprite;

    public int CurrentPage
    {
        get => currentPage;
        set
        {
            if (currentPage != value && value is >= 0 and <= 3)
            {
                SwitchPage(value);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            buttonList[i] = transform.GetChild(i).gameObject;
            buttonList[i].GetComponent<PageButtonData>().index = i;
        }

        mainPage = transform.GetChild(4).gameObject;
    }

    private void Submit()
    {
        JobManager.Instance.CommitAndCheck();
    }

    private void SwitchPage(int index)
    {
        buttonList[currentPage].GetComponent<Image>().sprite = inactiveSprite;
        buttonList[index].GetComponent<Image>().sprite = activeSprite;
        GetComponent<ViewKeeper>().UpdateView(index);
    }
}
