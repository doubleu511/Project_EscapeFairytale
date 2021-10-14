using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ���� ������, �����ؾ��� �帧�� �ִٸ� eventFlow�� �����ְ�, �ʳ� 0���� �ٷ� �� eventFlow�� �ٲ� �Ǵ� ������ ��
 * ����Ǿ���� ������Ʈ�� ���¸� �ٷιٷ� �ҷ��ü� �ְ� ������Ѵ�. (�ٲ� ���ΰ�ħ�ؼ� switch��
 * �ش� Flow�� �´� ���·� ��� �ٲ��ִ°� �ּ��ϵ� �ʹ�)
*/
public class SaveAble : MonoBehaviour
{
    public string saveKey;
    public int eventFlow = 0;

    public virtual void Save()
    {
        SecurityPlayerPrefs.SetInt(saveKey, eventFlow);
    }

    public virtual void Load()
    {
        eventFlow = SecurityPlayerPrefs.GetInt(saveKey, 0);
    }
}
