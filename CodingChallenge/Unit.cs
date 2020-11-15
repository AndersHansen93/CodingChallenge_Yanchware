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
    bool eventRunning = false;
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
        if (eventRunning == false)
        {
            indexOfNextEvent = NextEvent();
            TimeBeforeExecuteNextEvent();
        }
        
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
                    dateTime = e.eventTime;
                    eventindex = index;
                    if (tokenSource != null && indexOfNextEvent != eventindex)
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
        if (indexOfNextEvent >= 0 && Controller.units.Count >0)
        {
            timeUntilNextEvent = (scheduledEvents[indexOfNextEvent].eventTime - DateTime.Now).TotalSeconds;
            ExecuteScheduledEvent();
        }
        else
        {
            Debug.WriteLine("No New Event");
            return;
        }
    }


    async void ExecuteScheduledEvent()
    {
        eventRunning = true;
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
                eventRunning = false;
                return;
            }
            Thread.Sleep(100);
            timeOfStart += 0.1f;
        }
        Debug.WriteLine("Thread Completed");
        Controller.units[0].scheduledEvents[indexOfNextEvent].ExecuteEvent();
        Controller.units[0].scheduledEvents.RemoveAt(indexOfNextEvent);
        eventRunning = false;

        indexOfNextEvent = NextEvent();
        TimeBeforeExecuteNextEvent();


    }

}
