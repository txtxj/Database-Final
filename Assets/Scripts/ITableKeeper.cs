using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITableKeeper
{
    public void LoadParams();

    public void Commit();

    public void Rollback();

    public void Query();

    public void Show();
}
