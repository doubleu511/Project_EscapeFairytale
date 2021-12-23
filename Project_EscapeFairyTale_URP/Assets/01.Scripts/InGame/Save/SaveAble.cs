using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ISaveAble 인터페이스 - TempSave로 임시 저장, Load로 불러오기
*/
public interface ISaveAble
{
    public void TempSave();

    public void Load();
}