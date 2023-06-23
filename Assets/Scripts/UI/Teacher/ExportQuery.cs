using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExportQuery : MonoBehaviour
{
	public OpenPDFQuery openPdfQuery;
	private Teacher data;

	public Teacher Data
	{
		set
		{
			data = value;
			transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = data.tid;
		}
	}

	private void Start()
	{
		transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Export);
		transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => gameObject.SetActive(false));
	}

	private void Export()
	{
		int start, end;
		try
		{
			start = int.Parse(transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TMP_InputField>().text);
			end = int.Parse(transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<TMP_InputField>().text);
		}
		catch (Exception)
		{
			ConnectionLogManager.Instance.ReportError(new ArgumentException("Check your input"));
			return;
		}
		string filePath = TeacherExport.Instance.GenTyp(data, start, end);
		try
		{
			TeacherExport.Instance.TypToPdf(filePath);
		}
		catch (Exception e)
		{
			ConnectionLogManager.Instance.ReportError(e);
			return;
		}
		openPdfQuery.gameObject.SetActive(true);
		openPdfQuery.Path = filePath[..^3] + "pdf";
		gameObject.SetActive(false);
	}
}
