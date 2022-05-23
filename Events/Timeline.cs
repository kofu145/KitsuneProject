using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;

namespace KitsuneProject.Events
{
    public class Timeline
    {
        private Stopwatch stopwatch;
        private List<IEvent> events;
        private List<IEvent> runningEvents;
        private List<IEvent> toRemoveEvents;
        private List<IEvent> toRemoveRunningEvents;

        // have a stopwatch running, check and invoke events based on their requirements (list of events that are removed)
        public Timeline()
        {
            stopwatch = new Stopwatch();
            events = new List<IEvent>();
            runningEvents = new List<IEvent>();
            toRemoveEvents = new List<IEvent>();
            toRemoveRunningEvents = new List<IEvent>();

        }

        public void AddEvent(IEvent enemyEvent)
        {
            events.Add(enemyEvent);
        }

        public void BeginTimeline()
        {
            stopwatch.Start();
        }

        public void FreezeTimeline()
        {
            stopwatch.Stop();
        }

        public void UpdateEvents(GameTime gameTime)
        {
            foreach (IEvent enemyEvent in events)
            {
                if ((float)stopwatch.Elapsed.TotalSeconds > enemyEvent.TriggerTime)
                {
                    Console.WriteLine("Event fired!");
                    runningEvents.Add(enemyEvent);
                    enemyEvent.Start();
                    toRemoveEvents.Add(enemyEvent);
                }
            }

            foreach(IEvent enemyEvent in runningEvents)
            {
                if (enemyEvent.Finished)
                    toRemoveRunningEvents.Add(enemyEvent);
                else
                    enemyEvent.Update(gameTime);
            }

            foreach(IEvent enemyEvent in toRemoveEvents)
            {
                events.Remove(enemyEvent);
            }

            foreach(IEvent enemyEvent in toRemoveRunningEvents)
            {
                runningEvents.Remove(enemyEvent);
            }

        }

    }
}
