using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Components;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Core.Math;
using KitsuneProject.Prefabs.Bullets;

namespace KitsuneProject.Components
{
    class PixieBehavior : Component
    {
        float speed;
        private Transform transform;
        private Transform targetTransform;
        private BaseHomingBullet bullet;
        private bool isFiring;
        private float nextFireEvent;
        private float attackSpeed;

        public PixieBehavior(Transform player, float attackSpeed)
        {
            targetTransform = player;
            isFiring = true;
            nextFireEvent = 0;
            this.attackSpeed = attackSpeed;
        }

        public override void OnLoad()
        {
            transform = ParentEntity.GetComponent<Transform>();
            bullet = new BaseHomingBullet(transform, targetTransform, 500f, "./Resources/crystal_bullet.png");
            Console.WriteLine(bullet.speed);

        }

        //subtract their position with ours, is the direction vector (after normalize)
        // 

        public override void Update(GameTime gameTime)
        {
            if (isFiring && gameTime.totalTime.TotalSeconds >= nextFireEvent)
            {
                if (!TimeBomb.isActive)
                {
                    var bull = bullet.Instantiate();

                    var direction = targetTransform.Position - transform.Position;
                    direction.Normalize();
                    var bulletRb = bull.GetComponent<Rigidbody>();
                    bulletRb.Velocity = new Vector2(direction.X, direction.Y) * bullet.speed;
                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(bull);

                    nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;
                }


            }

        }
    }
}
