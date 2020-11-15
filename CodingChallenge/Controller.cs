using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for Class1
/// </summary>
public static class Controller
{
	public static List<ScheduledEvent> scheduledEvents = new List<ScheduledEvent>();

	public static List<Unit> units = new List<Unit>();

	static void GetAllScheduledEvent()
	{

		foreach (Unit u in units)
		{
			List<ScheduledEvent> tempList = new List<ScheduledEvent>();
			tempList = u.GetScheduledEventsOnUnit();
			if (tempList.Count > 0)
			{
				foreach (ScheduledEvent e in tempList)
				{
					scheduledEvents.Add(e);
				}

			}
			
		}
	}

}
