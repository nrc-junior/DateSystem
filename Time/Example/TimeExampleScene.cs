using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeExampleScene : MonoBehaviour {
    
    private TimeHandler time;
    public Slider slider;
    public TextMeshProUGUI labelTime;
    public TextMeshProUGUI clockTime;

    void Start() {
        time = GetComponent<TimeHandler>();
        TimeHandler.clock.UPDATE += OnUpdateTime;

        TimeData t0 = new TimeData() {
            years = 10
        };
        
        TimeData t1 = new TimeData() {
            years = 5,
            seconds = 1
        };

        Debug.Log(t1.sum(t0).ToString());
        Debug.Log(t0.since(t1).ToString());
    }

    public void Update() {
        labelTime.text = $"{slider.value}";
        time.timescale = slider.value;
    }

    void OnUpdateTime(TimeData t) {
        clockTime.text = $"{Format(t.hours)}:{Format(t.minutes)}";
    }

    string Format(int v) {
        return v < 9 ? "0" + v : v.ToString();
    }
}
