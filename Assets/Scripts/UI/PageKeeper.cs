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
            if (currentPage != value && value is >= 0 and <= 4)
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
        CurrentPage = 4;
    }

    private void SwitchPage(int index)
    {
        if (currentPage is >= 0 and <= 3)
        {
            buttonList[currentPage].GetComponent<Image>().sprite = inactiveSprite;
            buttonList[currentPage].GetComponent<Button>().enabled = true;
            buttonList[currentPage].transform.Translate(translation);
        }
        if (currentPage is >= 0 and <= 4)
        {
            transform.GetChild(0).GetChild(currentPage).gameObject.SetActive(false);
        }
        if (index is >= 0 and <= 3)
        {
            buttonList[index].GetComponent<Image>().sprite = activeSprite;
            buttonList[index].GetComponent<Button>().enabled = false;
            buttonList[index].transform.Translate(-translation);
        }
        transform.GetChild(0).GetChild(index).gameObject.SetActive(true);
        currentPage = index;
    }
}
