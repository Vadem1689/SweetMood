// Adam Pribyl 2024
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AP;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AP
{
    public class Calendar : MonoBehaviour
    {
        private TaskCompletionSource<DateTime> _tcs;
        public static Calendar Instance;

        //UI References
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI selectedMonth;
        [SerializeField] private TextMeshProUGUI selectedYear;
        [SerializeField] private GameObject dayPrefab;
        [SerializeField] private Transform days;
        [SerializeField] private Image enableTimeImg;
        [SerializeField] private TextMeshProUGUI timeInputText;
        [SerializeField] private Button timeInputButton;
        
        //Styles
        [Header("Styles")]
        [SerializeField] private DayStyle normalDay;
        [SerializeField] private DayStyle weekendDay;
        [SerializeField] private DayStyle selectedDay;
        
        [Header("Month names")]
        public List<string> months = new List<string>(12);
        
        //Private variables
        private int _month;
        private int _year;
        
        private int _clickedDay;
        private int _clickedMonth;
        private int _clickedYear;
        private int _clickedHour;
        private int _clickedMinute;

        
        private bool _enableTime;
        private bool _timeInput;
        private string _timeInputString;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        public async void TestOpen()
        {
            DateTime selectedTime = await GetCalendar();
            Debug.Log("Selected time: " + selectedTime);
        }

        public void SetGraphicsEnabled(bool value)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(value);
            }
        }

        public static async Task<DateTime> GetCalendar()
        {
            Instance._clickedYear = 1;
            Instance._clickedHour = 0;
            Instance._clickedMinute = 0;
            Instance.ChangeEnableTime(false, 0,0);
            Instance.GetCurrentMonth();
            Instance.SetGraphicsEnabled(true);
            DateTime result = await Instance.CalMain();
            return result;
        }

        public static async Task<DateTime> GetCalendar(DateTime initialDate)
        {
            Instance._clickedDay = initialDate.Day;
            Instance._clickedMonth = initialDate.Month;
            Instance._clickedYear = initialDate.Year;
            if (initialDate.Second >= 1)
            {
                Instance._clickedHour = initialDate.Hour;
                Instance._clickedMinute = initialDate.Minute;
                Instance.ChangeEnableTime(true, initialDate.Hour, initialDate.Minute);
            }
            else
            {
                Instance._clickedHour = 0;
                Instance._clickedMinute = 0;
                Instance.ChangeEnableTime(false, 0,0);
            }
            Instance.GetCurrentMonth();
            Instance.SetGraphicsEnabled(true);
            DateTime result = await Instance.CalMain();
            return result;
        }

        public async Task<DateTime> CalMain()
        {
            _tcs = new TaskCompletionSource<DateTime>();
            return await _tcs.Task;
        }

        public void Cancel()
        {
            _tcs.SetResult(new DateTime(1,1,1));
            SetGraphicsEnabled(false);
        }

        public void Confirm()
        {
            _tcs.SetResult(new DateTime(_clickedYear, _clickedMonth, _clickedDay, _clickedHour, _clickedMinute, _enableTime ? 1:0));
            SetGraphicsEnabled(false);
        }



        public void GetCurrentMonth()
        {
            DateTime dateTime = DateTime.Now;
            _month = dateTime.Month;
            _year = dateTime.Year;
            selectedMonth.text = $"{months[_month - 1]}";
            selectedYear.text = _year.ToString();
            DrawDays();
        }

        public void ChangeEnableTime()
        {
            _enableTime = !_enableTime;
            enableTimeImg.color = _enableTime ? selectedDay.backgroundColor : normalDay.textColor;
            Color c = timeInputText.color;
            timeInputText.color = new Color(c.r, c.g, c.b, _enableTime ? 1 : .5f);
            timeInputButton.interactable = _enableTime;
        }
        
        public void ChangeEnableTime(bool value, int hour, int minute)
        {
            _enableTime = value;
            enableTimeImg.color = _enableTime ? selectedDay.backgroundColor : normalDay.textColor;
            Color c = timeInputText.color;
            timeInputText.color = new Color(c.r, c.g, c.b, _enableTime ? 1 : .5f);
            timeInputButton.interactable = _enableTime;
            timeInputText.text = $"{hour:00}:{minute:00}";
        }        

        public void DrawDays()
        {
            foreach (Transform child in days)
            {
                Destroy(child.gameObject);
            }

            DayOfWeek dayOfWeek = new DateTime(_year, _month, 1).DayOfWeek;
            int daysOffset = (int)dayOfWeek + 6;
            if (daysOffset >= 7) daysOffset -= 7;

            for (int i = 0; i < daysOffset; i++)
            {
                Image img = Instantiate(dayPrefab, days).GetComponent<Image>();
                img.enabled = false;
                img.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

            }

            for (int i = 0; i < DateTime.DaysInMonth(_year, _month); i++)
            {
                GameObject day = Instantiate(dayPrefab, days);
                TextMeshProUGUI textMesh = day.transform.GetComponentInChildren<TextMeshProUGUI>();
                textMesh.text = (i + 1).ToString();
                if (new DateTime(_year, _month, i + 1).DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
                    ApplyStyle(day, weekendDay);
                day.GetComponent<Button>().onClick.AddListener(delegate
                {
                    _clickedDay = Int32.Parse(textMesh.text);
                    _clickedMonth = _month;
                    _clickedYear = _year;
                    StyleSelected();
                });


            }

            StyleSelected();
        }

        void StyleSelected()
        {
            foreach (Transform day in days)
            {
                if (day.GetComponent<Image>().enabled)
                {
                    int dayNum = Int32.Parse(day.GetComponentInChildren<TextMeshProUGUI>().text);
                    if (_clickedYear == _year && _clickedMonth == _month && _clickedDay == dayNum)
                        ApplyStyle(day.gameObject, selectedDay);
                    else if (new DateTime(_year, _month, dayNum).DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
                        ApplyStyle(day.gameObject, weekendDay);
                    else ApplyStyle(day.gameObject, normalDay);
                }
            }
        }

        void ApplyStyle(GameObject day, DayStyle style)
        {
            day.GetComponent<Image>().color = style.backgroundColor;
            day.GetComponentInChildren<TextMeshProUGUI>().color = style.textColor;
        }

        [System.Serializable]
        public class DayStyle
        {
            public Color backgroundColor;
            public Color textColor;
        }



        public void NextMonth()
        {
            _month++;
            if (_month > 12)
            {
                _month = 1;
                _year++;
            }

            selectedMonth.text = $"{months[_month - 1]}";
            selectedYear.text = _year.ToString();
            DrawDays();
        }

        public void PreviousMonth()
        {
            _month--;
            if (_month < 1)
            {
                _month = 12;
                _year--;
            }

            selectedMonth.text = $"{months[_month - 1]}";
            selectedYear.text = _year.ToString();

            DrawDays();
        }

        private void Start()
        {
            timeInputButton.onClick.AddListener(OnTimeInputButtonClick);
            GetCalendar();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                _timeInput = false;
            }

            if (_timeInput && Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    EnterTime();
                    return;
                }
                if (_timeInputString.Length >= 1 && Input.GetKeyDown(KeyCode.Backspace))
                    _timeInputString = _timeInputString.Substring(0, _timeInputString.Length - 1);
                if(_timeInputString.Length>=4) return;
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
                        {

                            _timeInputString += keyCode.ToString().Substring(5);
                        }
                        break;
                    }
                }

                
                timeInputText.text = _timeInputString;
            }
            
            
            
        }

        public void EnterTime()
        {
            _timeInput = false;
            string input = _timeInputString;
            input = input.PadLeft(4, '0');
            _clickedHour = int.Parse(input.Substring(0, 2));
            _clickedMinute = int.Parse(input.Substring(2, 2));
            if (_clickedHour <= 24 && _clickedMinute <= 59) timeInputText.text = $"{_clickedHour:00}:{_clickedMinute:00}";
            else
            {
                _clickedHour = 0;
                _clickedMinute = 0;
                timeInputText.text = "00:00";
            }
        }

        
        void OnTimeInputButtonClick()
        {
            _timeInput = true;
            timeInputText.text = "";
            _timeInputString = "";
        }
        
        bool IsPointerOverUIObject()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, new List<RaycastResult>());
            return eventData.pointerEnter != null;
        }
        
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Calendar))]
public class CalendarEditor : Editor
{
    Calendar targetScript;

    public void Awake()
    {
        targetScript = (Calendar)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Test Open Calendar")) targetScript.TestOpen();
    }
}
#endif
