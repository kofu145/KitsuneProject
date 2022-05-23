using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Core.Math;
using KitsuneProject.Components;
using Swordfish.Core;

namespace KitsuneProject.Prefabs
{
    class PlayerBullet : Prefab
    {
        private Transform transform;
        private Rigidbody rigidbody;
        private Vector2 force;
        private bool isAlt;
        private int damage;

        public PlayerBullet(Transform transform, Rigidbody rigidbody, Vector2 force, int damage) {
            this.transform = transform;
            this.force = force;
            this.rigidbody = rigidbody;
            isAlt = false;
            this.damage = damage;
        }

        public override Entity Instantiate()
        {
            var bulletRb = new Rigidbody();
            var bulletTransform = new Transform(transform.Position.X - 20f, transform.Position.Y + 30f, 0f, 0f, 0f, 0f, 5f, 5f);
            var bulletCollider = new CircleCollider(40f, false);
            var bulletSound = new Sound("./Resources/Sound/shoot2.wav");
            

            if (isAlt)
            {
                bulletTransform.Position.X += 40f;
                isAlt = false;
            }
            else
            {
                isAlt = true;
            }

            var instance = new Entity().AddComponent(new Sprite("./Resources/elongated_bullet_small.png"))
            .AddComponent(bulletTransform)
            .AddComponent(bulletCollider)
            .AddComponent(bulletRb)
            .AddComponent(bulletSound);

            bulletCollider.OnCollision += (CircleCollider other) =>
            {
                if (other.ParentEntity.HasComponent<Enemy>())
                {
                    other.ParentEntity.GetComponent<Enemy>().HP -= damage;
                    GameStateManager.Instance.GetScreen().GameScene.DestroyEntity(instance);
                }

                if (other.ParentEntity.HasComponent<BossEnemy>())
                {
                    other.ParentEntity.GetComponent<BossEnemy>().HP -= damage;
                    GameStateManager.Instance.GetScreen().GameScene.DestroyEntity(instance);
                }
            };

            bulletRb.Velocity = force;
            bulletSound.Play();
            return instance;

        }

    }
}
