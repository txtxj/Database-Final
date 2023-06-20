using System;
using System.ComponentModel;
using JetBrains.Annotations;

#region Base Data Type

public struct Teacher
{
    public enum Gender
    {
        [Description("男")] Male = 1, 
        [Description("女")] Female = 2
    }

    public enum Title
    {
        [Description("博士后")] Postdoctoral = 1,
        [Description("助教")] TeachingAssistant = 2,
        [Description("讲师")] Lecturer = 3, 
        [Description("副教授")] AssociateProfessor = 4, 
        [Description("特任教授")] AdjunctProfessor = 5, 
        [Description("教授")] Professor = 6,
        [Description("助理研究员")] AssistantResearcher = 7, 
        [Description("特任副研究员")] SpeciallyAppointedAssociateResearcher = 8, 
        [Description("副研究员")] AssociateResearcher = 9, 
        [Description("特任研究员")] SpecialAppointedResearcher = 10, 
        [Description("研究员")] Researcher = 11
    }
    
    [CanBeNull] public string tid;
    [CanBeNull] public string name;
    public Gender? gender;
    public Title? title;
}

public struct Paper
{
    public enum Type
    {
        [Description("Full Paper")] FullPaper = 1, 
        [Description("Short Paper")] ShortPaper = 2, 
        [Description("Poster Paper")] PosterPaper = 3, 
        [Description("Demo Paper")] DemoPaper = 4
    }

    public enum Level
    {
        [Description("CCF-A")] CcfA = 1,
        [Description("CCF-B")] CcfB = 2,
        [Description("CCF-C")] CcfC = 3,
        [Description("中文 CCF-A")] CCcfA = 4,
        [Description("中文 CCF-B")] CCcfB = 5,
        [Description("无级别")] NoLevel = 6
    }
    
    public int paid;
    public string name;
    public string source;
    public DateTime date;
    public Type type;
    public Level level;
}

public struct Project
{
    public enum Type
    {
        [Description("国家级项目")] National = 1, 
        [Description("省部级项目")] Provincial = 2, 
        [Description("市厅级项目")] City = 3, 
        [Description("企业合作项目")] Enterprise = 4, 
        [Description("其它类型项目")] Other = 5
    }
    
    public string pid;
    public string name;
    public string source;
    public Type type;
    public float funds;
    public int start;
    public int end;
}

public struct Course
{
    public enum Type
    {
        [Description("本科生课程")] Undergraduate = 1,
        [Description("研究生课程")] Graduate = 2
    }
    
    public string cid;
    public string name;
    public int credit;
    public Type type;
}

#endregion

#region Relation Data Type

public struct Publish
{
    [CanBeNull] public string tid;
    public int? paid;
    public int? rank;
    public bool? author;
    [CanBeNull] public string name;
}

public struct Assumption
{
    [CanBeNull] public string tid;
    [CanBeNull] public string pid;
    public int? rank;
    public float? funds;
    [CanBeNull] public string name;
}

public struct Lecture
{
    public enum Term
    {
        [Description("春季学期")] Spring = 1,
        [Description("夏季学期")] Summer = 2,
        [Description("秋季学期")] Fall = 3
    }
    
    [CanBeNull] public string tid;
    [CanBeNull] public string cid;
    public int? year;
    public Term? term;
    public int? credit;
    [CanBeNull] public string name;
}

#endregion