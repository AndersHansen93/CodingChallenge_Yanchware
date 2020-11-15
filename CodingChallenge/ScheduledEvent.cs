using System;
using System.Diagnostics;
/// <summary>
/// Summary description for Class1
/// </summary>
public class ScheduledEvent
{
	public DateTime eventTime;
	Action action;
	public delegate void eventMethod<T>(T message);
	public ScheduledEvent()
	{
		
	}
	public ScheduledEvent(DateTime date, Action eventMethod)
    {
		eventTime = date;
		action = eventMethod;

	}

	public void ExecuteEvent()
	{
		action();
	}
	


}
