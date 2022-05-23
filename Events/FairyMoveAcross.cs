using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;

namespace KitsuneProject.Events
{
    class FairyMoveAcross : IEvent
    {
        public float TriggerTime { get; set; }

        public bool Finished { get; set; }
        public FairyMoveAcross(float triggerTime)
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
