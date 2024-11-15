using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Hamster : MonoBehaviour
{
    public enum HamsterState
    {
        Idle,Move,Eat
    }

    [SerializeField] private GameObject _seed;
    private float _detectionRange = 0.5f; //감지 범위 
    private float _moveSpeed = 0.2f;
    [SerializeField] private HamsterState _currentState;
    private Animator _animator;

    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentState = HamsterState.Idle;
    }

    void Update()
    {
        switch (_currentState)
        {
            case HamsterState.Idle:
                Idle();
                break;
            case HamsterState.Move:
                Move();
                break;
            case HamsterState.Eat:
                Eat();
                break;
        }

        if (_seed != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, _seed.transform.position);

            if (distance <= _detectionRange && _currentState != HamsterState.Eat)
            {
                _currentState = HamsterState.Move;
            }
        }
        else
        {
            _currentState = HamsterState.Idle;
        }
    }

    public void SetSeeed(GameObject seed)
    {
        _seed = seed;
    }
    void Idle()
    {
        Debug.Log("Hamster Idle");
        _animator.SetBool("isMove", false);
        _animator.SetBool("isEat", false);
        _animator.SetBool("isIdle", true);
    }

    void Move()
    {
        if (_seed != null)
        {
            Vector3 direction = (_seed.transform.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation, Time.deltaTime * 5f);
            gameObject.transform.position += direction * _moveSpeed * Time.deltaTime;
            Debug.Log("Hamster Move");
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isMove", true);

            if (Vector3.Distance(transform.position, _seed.transform.position) <= 0.001f)
            {
                _currentState = HamsterState.Eat;
            }
        }

    }

    void Eat()
    {
        Debug.Log("Hamster Eat");
        _animator.SetBool("isMove", false);
        _animator.SetBool("isEat", true);

        Invoke("StopEat", 5f);
    }

    void StopEat()
    {
        _seed = null;
        Debug.Log("Hamster stoped eating");
        _animator.SetBool("isEat", false);
        _animator.SetBool("isIdle", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "feed")
        {
            Destroy(collision.gameObject);
            _currentState = HamsterState.Eat;
        }
    }

}
