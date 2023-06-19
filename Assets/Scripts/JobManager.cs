using System;
using System.Collections.Generic;

public class JobManager : MonoSingleton<JobManager>
{
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
            ConnectionLogManager.Instance.errorList.Add(e);
            ConnectionManager.Instance.RollbackTransaction();
            return;
        }
        ConnectionManager.Instance.CommitTransaction();
    }

    public void UpdateLectures(List<string> commands) => UpdateAndCheck(commands, ConnectionManager.Instance.CheckLectures);
    
    public void UpdatePublishes(List<string> commands) => UpdateAndCheck(commands, ConnectionManager.Instance.CheckPublishes);
    
    public void UpdateAssumptions(List<string> commands) => UpdateAndCheck(commands, ConnectionManager.Instance.CheckAssumptions);
}
