using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorWindow : MonoBehaviour
{
	private void Start()
	{
		transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => gameObject.SetActive(false));
	}

	public void SetError(string message)
	{
		transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = message;
	}
}
