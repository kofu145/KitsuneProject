using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;

namespace KitsuneProject.Events
{
    public interface IEvent
    {
        public float TriggerTime { get; }
        public bool Finished { get; }

        public void Start();

        public void Update(GameTime gameTime);
    }
}
