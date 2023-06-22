using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;
using Debug = UnityEngine.Debug;

public class TeacherExport : MonoSingleton<TeacherExport>
{
	private string[] envPath;
	
	public void Start()
	{
		string pathVariable = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
		envPath = pathVariable.Split(Path.PathSeparator);
	}

	private string GetEnumDescription(Enum value)
	{
		return (value.GetType().GetField(value.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute)?.Description;
	}

	private string EnterDataToTemplate(string template, Teacher teacher, int start, int end)
	{
		string command;
		MySqlDataReader reader;
		List<StringBuilder> data = new (7);
		for (int i = 0; i < data.Capacity; i++)
		{
			data.Add(new StringBuilder());
		}
		template = template.Replace("(year0)", start.ToString())
			.Replace("(year1)", end.ToString())
			.Replace("(tid)", teacher.tid)
			.Replace("(tname)", teacher.name)
			.Replace("(tgender)", GetEnumDescription(teacher.gender))
			.Replace("(ttitle)", GetEnumDescription(teacher.title));
		
		#region course
		
		command = "select course.cid, course.name, lecture.credit, lecture.year, lecture.term " +
				"from course left join lecture on course.cid=lecture.cid " +
				$"where lecture.tid = '{teacher.tid}' and lecture.year >= {start} and lecture.year <= {end}";
		reader = ConnectionManager.Instance.ExecuteAndRead(command, true);
		data.ForEach(sb => sb.Clear());
		while (reader.Read())
		{
			// 如果有人的名字或者课程名包含了英文引号，那么直接摆烂等报错，反正有隐患的地方已经够多了
			data[0].Append("\"").Append(reader[0]).Append("\", ");
			data[1].Append("\"").Append(reader[1]).Append("\", ");
			data[2].Append("\"").Append(reader[2]).Append("\", ");
			data[3].Append("\"").Append(reader[3]).Append("\", ");
			data[4].Append("\"").Append(GetEnumDescription((Lecture.Term) int.Parse(reader[4].ToString()))).Append("\", ");
		}
		reader.Close();
		template = template.Replace("(cid)", data[0].ToString())
			.Replace("(cname)", data[1].ToString())
			.Replace("(ccredit)", data[2].ToString())
			.Replace("(cyear)", data[3].ToString())
			.Replace("(cterm)", data[4].ToString());
		
		#endregion
		
		#region paper
		
		command = "select paper.name, paper.source, paper.time, paper.level, publish.`rank`, publish.author " +
				"from paper left join publish on paper.paid=publish.paid " +
				$"where publish.tid = '{teacher.tid}' and paper.time >= {start} and paper.time <= {end}";
		reader = ConnectionManager.Instance.ExecuteAndRead(command, true);
		data.ForEach(sb => sb.Clear());
		while (reader.Read())
		{
			data[0].Append("\"").Append(reader[0]).Append("\", ");
			data[1].Append("\"").Append(reader[1]).Append("\", ");
			data[2].Append("\"").Append(reader[2]).Append("\", ");
			data[3].Append("\"").Append(GetEnumDescription((Paper.Level) int.Parse(reader[3].ToString()))).Append("\", ");
			data[4].Append("\"").Append(reader[4]).Append("\", ");
			data[5].Append("\"").Append(reader[5]).Append("\", ");
		}
		reader.Close();
		template = template.Replace("(paname)", data[0].ToString())
			.Replace("(pasource)", data[1].ToString())
			.Replace("(payear)", data[2].ToString())
			.Replace("(palevel)", data[3].ToString())
			.Replace("(parank)", data[4].ToString())
			.Replace("(paauthor)", data[5].ToString());
		
		#endregion
		
		#region project
		
		command = "select project.name, project.source, project.type, project.start, project.`end`, " +
				"assumption.funds, project.funds " +
				"from project left join assumption on project.pid=assumption.pid " +
				$"where assumption.tid = '{teacher.tid}' and project.start <= {end} and project.`end` >= {start}";
		reader = ConnectionManager.Instance.ExecuteAndRead(command, true);
		data.ForEach(sb => sb.Clear());
		while (reader.Read())
		{
			data[0].Append("\"").Append(reader[0]).Append("\", ");
			data[1].Append("\"").Append(reader[1]).Append("\", ");
			data[2].Append("\"").Append(GetEnumDescription((Project.Type) int.Parse(reader[2].ToString()))).Append("\", ");
			data[3].Append("\"").Append(reader[3]).Append("\", ");
			data[4].Append("\"").Append(reader[4]).Append("\", ");
			data[5].Append("\"").Append(reader[5]).Append("\", ");
			data[6].Append("\"").Append(reader[6]).Append("\", ");
		}
		reader.Close();
		template = template.Replace("(prname)", data[0].ToString())
			.Replace("(prsource)", data[1].ToString())
			.Replace("(prtype)", data[2].ToString())
			.Replace("(prstart)", data[3].ToString())
			.Replace("(prend)", data[4].ToString())
			.Replace("(prfund)", data[5].ToString())
			.Replace("(prfunds)", data[6].ToString());
		
		#endregion
		
		return template;
	}
	
	public string GenTyp(Teacher teacher, int start, int end)
	{
		string filePath = Environment.CurrentDirectory + $@"/ExportTeacherData/{teacher.tid}.typ";
		TextAsset textAsset = Resources.Load<TextAsset>("TypstTemplate");

		if (!Directory.Exists(Environment.CurrentDirectory + @"/ExportTeacherData"))
		{
			Directory.CreateDirectory(Environment.CurrentDirectory + @"/ExportTeacherData");
		}
		
		using (FileStream fs = new FileStream(filePath, FileMode.Create))
		{
			using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
			{
				sw.Write(EnterDataToTemplate(textAsset.text, teacher, start, end));
				sw.Flush();
				sw.Close();
			}
		}
		return filePath;
	}
 
	public void TypToPdf(string filePath)
	{
		foreach (string path in envPath)
		{
			string fullPath = Path.Combine(path, "typst.exe");
			if (!File.Exists(fullPath))
			{
				continue;
			}
			Process process = new Process();
			process.StartInfo.FileName = fullPath;
			process.StartInfo.WorkingDirectory = Environment.CurrentDirectory + @"/ExportTeacherData";
			process.StartInfo.Arguments = " compile \"" + filePath + "\"";
			process.Start();
			process.WaitForExit();
			ConnectionLogManager.Instance.ReportLog(process.ExitCode.ToString());
			return;
		}
	}
}