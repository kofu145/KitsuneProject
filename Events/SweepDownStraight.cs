using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;

namespace KitsuneProject.Events
{
    class SweepDownStraight : IEvent
    {
        public float TriggerTime { get; private set; }

        public bool Finished { get; private set; }
        public SweepDownStraight(float triggerTime)
        {
            TriggerTime = triggerTime;

        }

        public void Start()
        {

        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
