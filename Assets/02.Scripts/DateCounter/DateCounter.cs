using System;
using TMPro;
using UnityEngine;

public class DateCounter : MonoBehaviour
{
    private DataLoader _dataLoader;
    private StartDate _startDate;
    [SerializeField] private TextMeshProUGUI _dateText;


    private void Awake()
    {
        _dataLoader = new DataLoader();
        _startDate = _dataLoader.Load<StartDate>();
    }

    private void Start()
    {
        var startTime = DateTime.ParseExact(_startDate.startDate, "yyyy-MM-dd HH:mm:ss", null);
        _dateText.text = ((int)((DateTime.Now - startTime).TotalDays + 1)).ToString();
    }
}
