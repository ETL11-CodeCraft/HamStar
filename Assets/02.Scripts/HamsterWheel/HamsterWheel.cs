using System;
using UnityEngine;

public class HamsterWheel : MonoBehaviour
{
    [SerializeField] Transform _ridingTransform;
    private Animator _wheelAnimator;
    private Animator _hamsterAnimator;

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
        if (_hamsterAnimator != null)
        {
            if (_hamsterAnimator.GetBool("isMove")) //FIXME: 햄스터 상태에 따라서 쳇바퀴 회전
            {
                RunWheel();
            }
            else
            {
                StopWheel();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTagsIn(other, "Hamster", "HamsterWheel", "Poop", "feed"))
        {
            TriggerEnterAction?.Invoke();
        }

        if (other.gameObject.CompareTag("Hamster"))
        {
            Debug.Log("햄스터 충돌 감지");
            other.transform.position = _ridingTransform.position;
            other.transform.Rotate(_ridingTransform.forward, 0f); //FIXME: 햄스터가 타는 방향에 따라 회전

            _ridingTransform.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().gameObject.SetActive(true);

            _hamsterAnimator = other.GetComponent<Animator>();
            _hamsterAnimator.Play("Run"); //FIXME: 햄스터 상태 변경
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
}
