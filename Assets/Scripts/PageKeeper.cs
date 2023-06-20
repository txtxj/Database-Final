using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PageKeeper : MonoSingleton<PageKeeper>
{
    private int currentPage = -1;
    private readonly List<GameObject> buttonList = new List<GameObject>();
    private GameObject mainPage;

    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public Vector3 translation = new Vector3();

    public int CurrentPage
    {
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
        buttonList.Clear();
        for (int i = 1; i < 5; i++)
        {
            buttonList.Add(transform.GetChild(i).gameObject);
            buttonList[i - 1].GetComponent<PageButtonData>().index = i - 1;
        }

        mainPage = transform.GetChild(0).gameObject;
    }

    private void SwitchPage(int index)
    {
        if (currentPage is >= 0 and <= 3)
        {
            buttonList[currentPage].GetComponent<Image>().sprite = inactiveSprite;
            buttonList[currentPage].GetComponent<Button>().enabled = true;
            buttonList[currentPage].transform.Translate(translation);
        }
        buttonList[index].GetComponent<Image>().sprite = activeSprite;
        buttonList[index].GetComponent<Button>().enabled = false;
        buttonList[index].transform.Translate(-translation);
        // 显示新页面，隐藏旧页面
        currentPage = index;
    }
}
