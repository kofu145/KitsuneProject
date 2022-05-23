using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Core.Math;
using KitsuneProject.Components;

namespace KitsuneProject.Prefabs.Bullets
{
    class BaseHomingBullet : Prefab
    {
        private Transform sourceTransform;
        private Transform targetTransform;
        public float speed;
        private string texture;

        public BaseHomingBullet(Transform sourceTransform, Transform targetTransform, float speed, string texture)
        {
            this.sourceTransform = sourceTransform;
            this.targetTransform = targetTransform;
            this.speed = speed;
            this.texture = texture;
        }
        public override Entity Instantiate()
        {

            var bulletRb = new Rigidbody();
            var bulletTransform = new Transform(sourceTransform.Position.X, sourceTransform.Position.Y, 1f, 0f, 0f, 0f, 5f, 5f);
            var bulletCollider = new CircleCollider(18f, false);
            var bulletSound = new Sound("./Resources/Sound/shoot.wav", .5f, 1);

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
            var direction = targetTransform.Position - sourceTransform.Position;
            direction.Normalize();

            bulletRb.Velocity = new Vector2(direction.X, direction.Y) * speed;*/
            bulletSound.Play();

            return instance;
        }
    }
}
