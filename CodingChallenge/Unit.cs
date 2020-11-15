using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// Summary description for Class1
/// </summary>
public class Unit
{
    float value;
    string unitName;
    List<ScheduledEvent> scheduledEvents = new List<ScheduledEvent>();
    int indexOfNextEvent;
    double timeUntilNextEvent;
    static CancellationTokenSource tokenSource = null;
    public Unit()
	{
        Controller.units.Add(this);
        value = 0;
        unitName = "TestUnit";
        Debug.WriteLine("New Unit Created with value " + Controller.units[0].value + " and name " + Controller.units[0].unitName);
	}
    public Unit(float value, string name)
    {
        Controller.units.Add(this);
        this.value = value;
        this.unitName = name;
    }

    public void ScheduleEvents(ScheduledEvent scheduledEvent)
    {
        scheduledEvents.Add(scheduledEvent);
        indexOfNextEvent = NextEvent();
        TimeBeforeExecuteNextEvent();
    }

    public List<ScheduledEvent> GetScheduledEventsOnUnit()
    {
        return scheduledEvents;
    }

    public float GetValue()
    {
        return value;
    }
    public void SetValue(float v)
    {
        value = v;
        Debug.WriteLine("Thermostat changed to " + value);
    }

    int NextEvent()
    {
        int index = 0;
        int eventindex = 0;
        DateTime dateTime = new DateTime(9999, 12, 31);
        if (scheduledEvents.Count > 0)
        {
            foreach (ScheduledEvent e in scheduledEvents)
            {
                if (DateTime.Compare(dateTime, e.eventTime) > 0)
                {
                    eventindex = index;
                    if (tokenSource != null)
                    {
                        tokenSource.Cancel();
                    }
                }
                index++;
            }

            return eventindex;
        }
        else
        {
            return -1;
        }
    }
  
    void TimeBeforeExecuteNextEvent()
    {
        if (indexOfNextEvent >= 0)
        {
            timeUntilNextEvent = (scheduledEvents[indexOfNextEvent].eventTime - DateTime.Now).TotalSeconds;
            ExecuteScheduledEvent();
        }
        else
        {
            return;
        }
    }


    async void ExecuteScheduledEvent()
    {
        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        await Task.Run(() => AsyncEventExecutor((int)timeUntilNextEvent, token));
    }
    void AsyncEventExecutor(int timeUntilExecution, CancellationToken token)
    {
        float timeOfStart = 0;
        while (timeOfStart < timeUntilExecution)
        {
            if (token.IsCancellationRequested)
            {
                Debug.WriteLine("Thread Cancelled");

                return;
            }
            Thread.Sleep(100);
            timeOfStart += 0.1f;
        }
        Debug.WriteLine("Thread Completed");
        Controller.units[indexOfNextEvent].scheduledEvents[0].ExecuteEvent();
        Controller.units.RemoveAt(indexOfNextEvent);


    }

}
