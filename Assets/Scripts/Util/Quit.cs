using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Quit : MonoBehaviour
{
    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(QuitApp);
    }
    
    private void QuitApp()
    {
        if (ConnectionManager.Instance.isOpen)
        {
            if (ConnectionManager.Instance.isTransaction)
            {
                JobManager.Instance.RollBack();
            }
            ConnectionManager.Instance.Close();
        }
        Application.Quit();
    }
}
