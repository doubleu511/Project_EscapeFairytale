using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitBox : MonoBehaviour
{
    public MeshRenderer shadow;
    public MeshRenderer shadow_range;
    public MeshRenderer shadow_casting;

    public float castingTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        shadow.material.color = new Color(0, 0, 0, 0);
        shadow.material.DOFade(1, castingTime * 1.5f).OnComplete(() =>
        {
            shadow.material.DOColor(new Color(1, 0, 0, shadow.material.color.a), 0.125f).SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo);
            shadow_range.material.DOColor(Color.red, 0.125f).SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo);
            shadow_casting.material.DOColor(Color.red, 0.125f).SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo).OnComplete(() =>
            {
                PlayerCheck();
            });
        });
        shadow_casting.transform.DOScale(0.3f, castingTime).SetEase(Ease.Linear);
    }

    void PlayerCheck()
    {
        var isChecked = Physics.SphereCastAll(transform.position, 0.1f, Vector3.up, 0, 1 << LayerMask.NameToLayer("Player"));
        if(isChecked.Length > 0)
        {
            EnemyAI.GameOver();
            UIManager.GameOverUI(GameManager.Instance.spriteBox.Reason_Shoes_Small);
        }

        GameManager.PlaySFX(GameManager.Instance.audioBox.RedShoes_redshoes_walk2_left);
        shadow.material.DOFade(0, 0.5f).SetEase(Ease.Linear);
        shadow_range.material.DOFade(0, 0.5f).SetEase(Ease.Linear);
        shadow_casting.material.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
