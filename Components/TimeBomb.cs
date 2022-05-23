using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Components;
using Swordfish.Core;
using Swordfish.ECS;
using KitsuneProject.Prefabs.Bullets;
using Swordfish.Core.Math;
using Swordfish.Core.Input;
using KitsuneProject.Events;
using Swordfish.Components.UI;

namespace KitsuneProject.Components
{
    class TimeBomb : Component
    {
        public float duration;
        public static bool isActive;
        private float endEventTime;
        private List<Transform> transforms;
        private List<Vector3> lockPositions;
        public float Countdown;
        public int charges;
        private Timeline timeline;
        private Label bombLabel;

        public TimeBomb(float duration, int charges, Timeline timeline, Label bombLabel)
        {
            this.duration = duration;
            transforms = new List<Transform>();
            lockPositions = new List<Vector3>();
            isActive = false;
            this.charges = charges;
            this.timeline = timeline;
            this.bombLabel = bombLabel;
        }

        public override void OnLoad()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (duration > (float)gameTime.totalTime.TotalSeconds)
                Countdown = duration - (float)gameTime.totalTime.TotalSeconds;

            var scene = GameStateManager.Instance.GetScreen().GameScene;

            if (InputManager.Instance.GetKeyDown(Keys.B) && !isActive && charges > 0)
            {
                transforms.Clear();
                lockPositions.Clear();
                foreach (Entity entity in scene.Entities)
                {
                    if (!entity.HasComponent<Player>())
                    {
                        var transform = entity.GetComponent<Transform>();
                        transforms.Add(transform);
                        lockPositions.Add(new Vector3(transform.Position.X, transform.Position.Y, 0f));
                        if (entity.HasComponent<Sprite>())
                            entity.GetComponent<Sprite>().SetColor(new Vector3(105, 105, 105));
                    }
                    
                }
                isActive = true;
                endEventTime = (float)gameTime.totalTime.TotalSeconds + duration;
                timeline.FreezeTimeline();
                charges--;
                bombLabel.Text = charges.ToString();
            }

            if (isActive) {
                //Console.WriteLine("Active!");
                if ((float)gameTime.totalTime.TotalSeconds >= endEventTime)
                {
                    isActive = false;
                    foreach(Entity entity in scene.Entities)
                    {
                        if (entity.HasComponent<Sprite>())
                            entity.GetComponent<Sprite>().SetColor(new Vector3(255f, 255f, 255f));
                    }
                    //Console.WriteLine("setting not active!");
                    timeline.BeginTimeline();
                }

                for (int i=0; i<transforms.Count; i++)
                {
                    //Console.WriteLine(transforms[i].Position);
                    //Console.WriteLine(lockPositions[i]);
                    transforms[i].Position.X = lockPositions[i].X;
                    transforms[i].Position.Y = lockPositions[i].Y;
                }
                
            }
            
        }
    }
}
