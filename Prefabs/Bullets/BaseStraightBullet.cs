using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.ECS;
using Swordfish.Components;
using KitsuneProject.Components;
using Swordfish.Core.Math;

namespace KitsuneProject.Prefabs.Bullets
{
    class BaseStraightBullet : Prefab
    {
        private Transform transform;
        public float speed;
        private string texture;
        private static bool playSound = true;
        private static int counter = 0;
        private float radius;
        private float size;
        
        public BaseStraightBullet(Transform transform, float speed, string texture, float radius, float size)
        {
            this.transform = transform;
            this.speed = speed;
            this.texture = texture;
            this.radius = radius;
            this.size = size;
        }
        public override Entity Instantiate()
        {
            playSound = false;
            var bulletRb = new Rigidbody();
            var bulletTransform = new Transform(transform.Position.X, transform.Position.Y, 6f, 0f, 0f, 0f, size, size);
            var bulletCollider = new CircleCollider(radius, false);
            var bulletSound = new Sound("./Resources/Sound/shoot.wav", .4f, 1);

            var instance = new Entity().AddComponent(new Sprite(texture))
            .AddComponent(bulletTransform)
            .AddComponent(bulletCollider)
            .AddComponent(bulletRb)
            .AddComponent(bulletSound);

            bulletCollider.OnCollision += (CircleCollider other) =>
            {
                if (other.ParentEntity.HasComponent<Player>())
                {
                    other.ParentEntity.GetComponent<Player>().TakeDamage();
                }
            };

            /*
            var angle = transform.Rotation.Z * Math.PI / 180;

            var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            direction.Normalize();

            bulletRb.Velocity = new Vector2(direction.X, direction.Y) * speed;
            */
            counter++;
            if (counter == 9)
            {
                counter = 0;
                playSound = true;
            }
            if (playSound)
            {
                bulletSound.Play();
            }
            

            return instance;
        }
    }
}
