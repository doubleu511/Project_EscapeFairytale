using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ISaveAble �������̽� - TempSave�� �ӽ� ����, Load�� �ҷ�����
*/
public interface ISaveAble
{
    public void TempSave();

    public void Load();
}