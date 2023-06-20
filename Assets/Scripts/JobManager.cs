using System;
using System.Collections.Generic;

public class JobManager : MonoSingleton<JobManager>
{
    [Obsolete]
    private void UpdateAndCheck(List<string> commands, Action checker)
    {
        if (!ConnectionManager.Instance.isOpen)
        {
            return;
        }

        ConnectionManager.Instance.StartTransaction();
        try
        {
            commands.ForEach(x => ConnectionManager.Instance.ExecuteNonQuery(x));
            checker();
        }
        catch (Exception e)
        {
            ConnectionLogManager.Instance.ReportError(e);
            ConnectionManager.Instance.RollbackTransaction();
            return;
        }
        ConnectionManager.Instance.CommitTransaction();
    }

    [Obsolete]
    public void UpdateLectures(List<string> commands) => UpdateAndCheck(commands, ConnectionManager.Instance.CheckLectures);
    
    [Obsolete]
    public void UpdatePublishes(List<string> commands) => UpdateAndCheck(commands, ConnectionManager.Instance.CheckPublishes);
    
    [Obsolete]
    public void UpdateAssumptions(List<string> commands) => UpdateAndCheck(commands, ConnectionManager.Instance.CheckAssumptions);

    public void UpdateInTransaction(string command)
    {
        if (!ConnectionManager.Instance.isOpen)
        {
            return;
        }

        if (!ConnectionManager.Instance.isTransaction)
        {
            ConnectionManager.Instance.StartTransaction();
        }

        try
        {
            ConnectionManager.Instance.ExecuteNonQuery(command);
        }
        catch (Exception e)
        {
            ConnectionLogManager.Instance.ReportError(e);
            ConnectionManager.Instance.RollbackTransaction();
        }
    }

    public void CommitAndCheck()
    {
        if (!ConnectionManager.Instance.isOpen || !ConnectionManager.Instance.isTransaction)
        {
            return;
        }
        
        try
        {
            ConnectionManager.Instance.CheckLectures();
            ConnectionManager.Instance.CheckPublishes();
            ConnectionManager.Instance.CheckAssumptions();
        }
        catch (Exception e)
        {
            ConnectionLogManager.Instance.ReportError(e);
            ConnectionManager.Instance.RollbackTransaction();
            return;
        }
        ConnectionManager.Instance.CommitTransaction();
    }

    public void RollBack()
    {
        if (!ConnectionManager.Instance.isOpen)
        {
            return;
        }

        if (ConnectionManager.Instance.isTransaction)
        {
            ConnectionManager.Instance.RollbackTransaction();
        }
    }
}
