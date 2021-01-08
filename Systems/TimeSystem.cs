using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [Header("Time Settings: ")]
    [SerializeField] int startOfDayHours;
    [SerializeField] int startOfDayMinutes;
    [SerializeField] int endOfDayHours;
    [SerializeField] int endOfDayMinutes;
    private bool isNightTime = false;

    [Space]

    [SerializeField] float timeProgressionMultiplier = 300;

    [SerializeField] int currentSeconds;
    [SerializeField] int currentMinutes;
    [SerializeField] int currentHours;
    [SerializeField, Range(1, 4)] int currentDay = 1;
    [SerializeField, Range(1, 12)] int currentMonth = 1;
    public int CurrentMonth { get { return currentMonth; } }
    [SerializeField] int currentYear = 1818;
    public int CurrentYear { get { return currentYear; } }

    [Header("Light Settings: ")]
    [SerializeField] Light directionalLight;
    [SerializeField] float daylightBeginRotationX = 180;
    [SerializeField] float daylightEndRotationX = 135;
    [SerializeField] AnimationCurve daylightRotationXCurve;

    [SerializeField] float daylightBeginRotationY = 45;
    [SerializeField] float daylightEndRotationY = 135;
    [SerializeField] AnimationCurve daylightRotationYCurve;

    public Date CurrentDate { get; private set; } = new Date();
    public bool IsPaused = false;

    private void Awake()
    {
        CurrentDate.Day = currentDay;
        CurrentDate.Month = currentMonth;
        CurrentDate.Year = currentYear;
    }

    private void Update()
    {
        Tick();
    }

    public void Tick()
    {
        if (!IsPaused) { ProgressTime(); }
    }

    private void ProgressTime()
    {
        currentSeconds += Mathf.RoundToInt(Time.deltaTime * timeProgressionMultiplier);

        UpdateTimeValues();
        if (!isNightTime && !IsPaused) { UpdateLight(); }
    }

    private void UpdateTimeValues()
    {
        if (currentSeconds >= 60) { currentMinutes += MathM.DivisionWithoutRemainders(currentSeconds, 60); currentSeconds -= 60 * MathM.DivisionWithoutRemainders(currentSeconds, 60); }
        if (currentMinutes >= 60) { currentHours += MathM.DivisionWithoutRemainders(currentMinutes, 60); currentMinutes -= 60 * MathM.DivisionWithoutRemainders(currentMinutes, 60); }
        if (currentHours >= 24) { currentDay += MathM.DivisionWithoutRemainders(currentHours, 24); currentHours -= 24 * MathM.DivisionWithoutRemainders(currentHours, 24); }

        if (currentDay > 4) { currentMonth++; currentDay = 1; GameManager.Instance.EventManager.IsEndOfMonth = true; }
        if (currentMonth > 12) { currentYear++; currentMonth = 1; }

        CurrentDate.Day = currentDay;
        CurrentDate.Month = currentMonth;
        CurrentDate.Year = currentYear;

        GameManager.Instance.UIManager.UpdateTimeAndDate(currentHours, currentMinutes, CurrentDate);

        //Check if it is night time
        if (!isNightTime && currentHours >= endOfDayHours && currentMinutes >= endOfDayMinutes)
        {
            isNightTime = true;
            GameManager.Instance.OnNightTime();
        }
    }

    private void UpdateLight()
    {
        float _lerpValue = 1f / 24f * (currentHours + currentMinutes / 60f + currentSeconds / 60f / 60f);

        Vector3 _rotation = Vector3.zero;
        _rotation.x = -(90f + (360f / 24f * (currentHours + currentMinutes / 60f + currentSeconds / 60f / 60f)));

        //float _evaluatedXRotLerpValue = daylightRotationXCurve.Evaluate(_lerpValue);
        //_rotation.x = Mathf.Lerp(daylightBeginRotationX, daylightEndRotationX, _evaluatedXRotLerpValue);

        //float _evaluatedYRotLerpValue = daylightRotationYCurve.Evaluate(_lerpValue);
        //_rotation.y = Mathf.Lerp(daylightBeginRotationY, daylightEndRotationY, _evaluatedYRotLerpValue);

        directionalLight.transform.localEulerAngles = _rotation;
    }

    public void SetTimeToMorning()
    {
        if (currentHours > startOfDayHours) { currentDay += MathM.DivisionWithoutRemainders(currentHours, 24) + 1; }

        currentHours = startOfDayHours;
        currentMinutes = startOfDayMinutes;

        UpdateTimeValues();

        isNightTime = false;
    }

    public void SetDate(Date _date)
    {
        currentYear = _date.Year;
        currentMonth = _date.Month;
        currentDay = _date.Day;
        currentHours = startOfDayHours;
        currentMinutes = startOfDayMinutes;

        UpdateTimeValues();
    }

    public TimeData GetCurrentTime()
    {
        return new TimeData(
            currentSeconds,
            currentMinutes,
            currentHours
            );
    }
}

public enum Months { Januari = 1, Februari, Maart, April, Mei, Juni, Juli, Augustus, September, Oktober, November, December }

[System.Serializable]
public class Date
{
    [Range(1, 4)] public int Day;
    [Range(1, 12)] public int Month;
    public int Year;

    public static bool operator ==(Date _lhs, Date _rhs)
    {
        if (_lhs.Year == _rhs.Year && _lhs.Month == _rhs.Month && _lhs.Day == _rhs.Day)
        {
            return true;
        }

        else { return false; }
    }

    public static bool operator !=(Date _lhs, Date _rhs)
    {
        if (_lhs.Year == _rhs.Year && _lhs.Month == _rhs.Month && _lhs.Day == _rhs.Day)
        {
            return false;
        }

        else { return true; }
    }

    public static bool operator <(Date _lhs, Date _rhs)
    {
        //Check if year is lower
        if (_lhs.Year < _rhs.Year) { return true; }
        else if (_lhs.Year == _rhs.Year)
        {
            //Check if month is lower (if year is the same)
            if (_lhs.Month < _rhs.Month) { return true; }
            else if (_lhs.Month == _rhs.Month)
            {
                //Check if day is lower (if year and month are the same)
                if (_lhs.Day < _rhs.Day) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    public static bool operator >(Date _lhs, Date _rhs)
    {
        //Check if year is greater
        if (_lhs.Year > _rhs.Year) { return true; }
        else if (_lhs.Year == _rhs.Year)
        {
            //Check if month is greater (if year is the same)
            if (_lhs.Month > _rhs.Month) { return true; }
            else if (_lhs.Month == _rhs.Month)
            {
                //Check if day is greater (if year and month are the same)
                if (_lhs.Day > _rhs.Day) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    public static bool operator <=(Date _lhs, Date _rhs)
    {
        //Check if year is lower
        if (_lhs.Year < _rhs.Year) { return true; }
        else if (_lhs.Year == _rhs.Year)
        {
            //Check if month is lower (if year is the same)
            if (_lhs.Month < _rhs.Month) { return true; }
            else if (_lhs.Month == _rhs.Month)
            {
                //Check if day is lower (if year and month are the same)
                if (_lhs.Day <= _rhs.Day) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    public static bool operator >=(Date _lhs, Date _rhs)
    {
        //Check if year is greater
        if (_lhs.Year > _rhs.Year) { return true; }
        else if (_lhs.Year == _rhs.Year)
        {
            //Check if month is greater (if year is the same)
            if (_lhs.Month > _rhs.Month) { return true; }
            else if (_lhs.Month == _rhs.Month)
            {
                //Check if day is greater (if year and month are the same)
                if (_lhs.Day >= _rhs.Day) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

}

[System.Serializable]
public class TimeData
{
    public TimeData(int _second, int _minute, int _hour)
    {
        Second = _second;
        Minute = _minute;
        Hour = _hour;
    }

    public int Second;
    public int Minute;
    public int Hour;

    public static bool operator <(TimeData _lhs, TimeData _rhs)
    {
        //Check if hour is lower
        if (_lhs.Hour < _rhs.Hour) { return true; }
        else if (_lhs.Hour == _rhs.Hour)
        {
            //Check if minute is lower (if hour is the same)
            if (_lhs.Minute < _rhs.Minute) { return true; }
            else if (_lhs.Minute == _rhs.Minute)
            {
                //Check if second is lower (if hour and minute are the same)
                if (_lhs.Second < _rhs.Second) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    public static bool operator >(TimeData _lhs, TimeData _rhs)
    {
        //Check if hour is greater
        if (_lhs.Hour > _rhs.Hour) { return true; }
        else if (_lhs.Hour == _rhs.Hour)
        {
            //Check if minute is greater (if hour is the same)
            if (_lhs.Minute > _rhs.Minute) { return true; }
            else if (_lhs.Minute == _rhs.Minute)
            {
                //Check if second is greater (if hour and minute are the same)
                if (_lhs.Second > _rhs.Second) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    public static bool operator <=(TimeData _lhs, TimeData _rhs)
    {
        //Check if hour is lower
        if (_lhs.Hour < _rhs.Hour) { return true; }
        else if (_lhs.Hour == _rhs.Hour)
        {
            //Check if minute is lower (if hour is the same)
            if (_lhs.Minute < _rhs.Minute) { return true; }
            else if (_lhs.Minute == _rhs.Minute)
            {
                //Check if second is lower (if hour and minute are the same)
                if (_lhs.Second <= _rhs.Second) { return true; }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    public static bool operator >=(TimeData _lhs, TimeData _rhs)
    {
        //Check if hour is higher
        if (_lhs.Hour > _rhs.Hour) { return true; }
        else if (_lhs.Hour == _rhs.Hour)
        {
            //Check if minute is higher (if hour is the same)
            if (_lhs.Minute > _rhs.Minute) { return true; }
            else if (_lhs.Minute == _rhs.Minute)
            {
                //Check if second is higher (if hour and minute are the same)
                if (_lhs.Second >= _rhs.Second) { return true; }
                else { return true; }
            }
            else { return false; }
        }
        else { return false; }
    }

}
