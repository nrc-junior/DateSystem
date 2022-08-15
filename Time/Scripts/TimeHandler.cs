using System;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;


public static class TimeMethods {
   private const int size = 7;
   private static ushort[] periods = new ushort[7]{60,60,24,7,4,12,0};
   
   public static TimeData sum(this TimeData t, TimeData other) {
      TimeData result = new TimeData();
      result.seconds = t.seconds + other.seconds;
      
      if (result.seconds >= 60) {
         result.seconds -= 60;
         result.minutes = 1;
      }

      result.minutes += t.minutes + other.minutes;
      if (result.minutes >= 60) {
         result.minutes -= 60;
         result.hours = 1;
      }
      
      result.hours += t.hours + other.hours;
      if (result.hours >= 24) {
         result.hours -= 24;
         result.days = 1;
      }

      result.days += t.days + other.days;
      if (result.days >= 7) {
         result.days -= 7;
         result.weeks = 1;
      }
      
      result.weeks += t.weeks + other.weeks;
      if (result.weeks >= 4) {
         result.weeks -= 4;
         result.months = 1;
      }
      
      result.months += t.months + other.months;
      if (result.months >= 12) {
         result.weeks -= 12;
         result.years = 1;
      }

      result.years += t.years + other.years;

      return result;
   }

   public static bool havePass(this TimeData t, TimeData other) {
      return t.years >= other.years && t.months >= other.months && t.weeks >= other.weeks && t.days >= other.days && t.hours >= other.hours && t.minutes >= other.minutes && t.seconds > other.seconds;
   }

   public static float toSeconds(this TimeData t) => 
      t.years * 31535965 + (t.months * 2627999) + (t.weeks*604799) + (t.days*86400) + (t.hours * 3600) + (t.minutes*60) + t.seconds;
      
   public static TimeData toData(this int[] t) => new TimeData() {
      years = t[6], months = t[5], weeks = t[4], days = t[3], hours = t[2], minutes = t[1], seconds = t[0]
   };
   
   /// <summary>
   /// retorna tempo entre datas
   /// </summary>
   /// <param name="t">tempo atual, deve ser maior que o termo.</param>
   /// <param name="termo">tempo no passado, deve ser menor que o tempo atual.</param>
   /// <returns></returns>
   public static TimeData since(this TimeData t, TimeData termo) {
      int[] t0 = t.toArray();
      int[] t1 = termo.toArray();
      int[] res = new int[size];
      
      for (int i = 0; i < size ; i++) {
         int v = t0[i] - t1[i];
         
         if (v < 0) {
            bool didSubtract = false;
            int borrowing = 0;

            for (int j = i+1; j < size; j++) {
               if (t0[j] <= 0) {
                  borrowing = j;
                  continue;
               }
               
               if (borrowing > 0) {
                  for (int z = i + 1; z <= borrowing; z++) {
                     t0[z]--;
                     
                     if (t0[z] < 0) 
                     {
                        t0[z] += periods[z];
                     }
                  }
               }
               
               didSubtract = true;
               t0[j]--;
               v += periods[i];
               break;
            }

            if (!didSubtract) {
               return null;
            }
         }

         res[i] = v;
      }

      TimeData result = res.toData();
      return result;
      
   }
   
   public static int[] toArray(this TimeData t) => new []{ t.seconds, t.minutes,  t.hours, t.days, t.weeks, t.months, t.years }; 
}

public class TimeData
{
   public int years = 0;
   public int months = 0;
   public int weeks = 0;
   public int days = 0;
   public int hours = 0;
   public int minutes = 0;
   public int seconds = 0;

   public override string ToString()
   {
      return $"{years}y,{months}m,{weeks}w,{days}d,{hours}h,{minutes}m,{seconds}s";
   }

}

public class Timer {
   public readonly TimeData time = new TimeData();
   public Action<TimeData> UPDATE;
   
   public void Update() {
      UPDATE?.Invoke(time);

      if (++time.seconds < 60) return;
      
      time.seconds = 0;
      time.minutes++;
      MINUTES?.Invoke(time.minutes);

      if (time.minutes < 60) return;
      time.minutes = 0;
      time.hours++;
      HOURS?.Invoke(time.hours);

      if (time.hours < 24) return;
      time.hours = 0;
      time.days++;
      DAYS?.Invoke(time.days);

      if (time.days < 7) return;
      time.days = 0;
      time.weeks++;
      WEEKS?.Invoke(time.weeks);

      if (time.weeks < 4) return;
      time.weeks = 0;
      time.months++;
      MONTHS?.Invoke(time.months);

      if (time.months < 12) return;
      time.years++;
      YEARS?.Invoke(time.years);
   }
   
   public Action<int> YEARS, MONTHS, WEEKS, DAYS, HOURS, MINUTES, SECONDS;
}

public class TimeHandler: MonoBehaviour {
   public static Timer clock;

   public static TimeData GetTime() => clock.time;
   
   public float timescale = 1;
   private float time;

   private void Awake() {
      clock = new Timer();
   }
   
   public void Update() {
      
      if (Time.time >  time) {
         time = Time.time + 1/timescale;
         clock.Update();
      }  
      
   }
   
   public void InvokeWhenIs(Timer time) {
      
   }
   
   public void InvokeIn(Timer time) {
      
   }
   
}
