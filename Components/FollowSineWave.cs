using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Core.Math;

namespace KitsuneProject.Components
{
    
    class FollowSineWave : Component
    {
        private Transform transform;
        private float speed;
        private float magnitude;
        private float frequency;
        private Vector3 direction;

        public FollowSineWave(Transform transform, float speed, Vector3 direction)
        {
            this.transform = transform;
            this.speed = speed;
            this.magnitude = .1f;
            this.frequency = 4f;
            this.direction = direction;

        }
        public override void OnLoad()
        {

        }

        public override void Update(GameTime gameTime)
        {
            
            var axis = Vector3.Left;

            transform.Position += axis * (float)gameTime.deltaTime.TotalSeconds * speed;
            transform.Position = transform.Position + Vector3.Up * (float)MathF.Sin((float)gameTime.totalTime.TotalSeconds * frequency) * magnitude;
            //Console.WriteLine(transform.Position);

            // if angle: new Vector2(MathF.Cos(MathF.PI - rot), MathF.Sin(MathF.PI - rot))
            /*
            transform.Position = transform.Position + direction * new Vector3(
                MathF.Sin((float)gameTime.totalTime.TotalSeconds * frequency) * magnitude, 
                MathF.Sin((float)gameTime.totalTime.TotalSeconds * frequency) * magnitude, 
                0);*/
        }
    }
}
