using System;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{
    [SerializeField] Transform _ridingTransform;
    private Animator _wheelAnimator;

    public Action TriggerEnterAction;
    public Action TriggerExitAction;

    private void Awake()
    {
        _wheelAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTagsIn(other, "Hamster", "HamsterWheel", "Poop", "feed"))
        {
            TriggerEnterAction?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CompareTagsIn(other, "Hamster", "HamsterWheel", "Poop", "feed"))
        {
            TriggerExitAction?.Invoke();
        }
    }

    private bool CompareTagsIn(Collider other, params string[] tags)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (other.CompareTag(tags[i])) 
                return true;
            else
                continue;
        }
        return false;
    }

    /// <summary>
    /// 햄스터가 쳇바퀴를 탈 때 호출
    /// </summary>
    public void ActivateWheel(Hamster hamster)
    {
        hamster.transform.position = _ridingTransform.position;
        hamster.transform.Rotate(_ridingTransform.forward, 0f); // 햄스터가 타는 방향 고정

        _wheelAnimator.SetFloat("Speed", 1f);
        
        SoundManager.instance.PlaySFX("HamsterWheel");
    }

    /// <summary>
    /// 햄스터가 쳇바퀴에서 내릴 때 호출
    /// </summary>
    public void DeactiveWheel()
    {
        _wheelAnimator.SetFloat("Speed", 0f);
        SoundManager.instance.StopSFX();
    }
}
