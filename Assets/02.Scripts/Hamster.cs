using UnityEngine;

public class Hamster : MonoBehaviour
{
    private int _fullness = 100;
    public int fullness
    {
        get
        {
            return Mathf.Clamp(_fullness, 0, 100);
        }
        set
        {
            _fullness = Mathf.Clamp(value, 0, 100);
        }
    }

    private int _cleanliness = 100;
    public int cleanliness
    {
        get
        {
            return Mathf.Clamp(_cleanliness, 0, 100);
        }
        set
        {
            _cleanliness = Mathf.Clamp(value, 0, 100);
        }
    }

    private int _closeness = 100;
    public int closeness
    {
        get
        {
            return Mathf.Clamp(_closeness, 0, 100);
        }
        set
        {
            _closeness = Mathf.Clamp(value, 0, 100);
        }
    }

    private int _stress = 0;
    public int stress
    {
        get
        {
            return Mathf.Clamp(_stress, 0, 100);
        }
        set
        {
            _stress = Mathf.Clamp(value, 0, 100);
        }
    }
}
