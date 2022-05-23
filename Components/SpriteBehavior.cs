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
    class SpriteBehavior : Component
    {
        private float attackSpeed = 2.5f;
        private int spawnPointCount = 16;
        private BaseStraightBullet bullet;
        private Transform transform;
        private float bulletSpeed;
        private float nextFireEvent;
        public SpriteBehavior(Transform transform, float bulletSpeed)
        {
            this.transform = transform;
            this.bulletSpeed = bulletSpeed;

            nextFireEvent = 0;
            bullet = new BaseStraightBullet(transform, 80f, "./Resources/blue_basic_bullet.png", 15f, 5f);
        }
        
        public override void OnLoad()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.totalTime.TotalSeconds >= nextFireEvent)
            {
                if (!TimeBomb.isActive)
                {
                    var bulletAngle = 360 / spawnPointCount;

                    for (var i = 0; i < spawnPointCount; i++)
                    {
                        var angle = (bulletAngle * i) * Math.PI / 180;

                        var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        direction.Normalize();

                        var newBullet = bullet.Instantiate();
                        var bulletRb = newBullet.GetComponent<Rigidbody>();
                        var bulletTransform = newBullet.GetComponent<Transform>();

                        bulletTransform.Position += new Vector3(direction.X, direction.Y, 0f) * 70f;
                        bulletRb.Velocity = new Vector2(direction.X, direction.Y) * bulletSpeed;
                        GameStateManager.Instance.GetScreen().GameScene.AddEntity(newBullet);
                    }

                    nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;
                }
                

            }
        }
    }
}
