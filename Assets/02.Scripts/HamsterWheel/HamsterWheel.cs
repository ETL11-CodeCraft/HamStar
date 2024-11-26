using System;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{
    [SerializeField] Transform _ridingTransform;
    private Animator _wheelAnimator;

    public Action TriggerEnterAction;
    public Action TriggerExitAction;

    public void RunWheel()
    {
        _wheelAnimator.SetFloat("Speed", 1f);
    }

    public void StopWheel()
    {
        _wheelAnimator.SetFloat("Speed", 0f);
    }

    private void Awake()
    {
        _wheelAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //TODO: 햄스터 동작 상태에 따라 쳇바퀴 회전
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

    public void ActivateWheel(Hamster hamster)
    {
        hamster.transform.position = _ridingTransform.position;
        hamster.transform.Rotate(_ridingTransform.forward, 0f); // 햄스터가 타는 방향 고정
    }

    public void DeactiveWheel()
    {
        //햄스터가 쳇바퀴에서 내리면 실행
    }
}
