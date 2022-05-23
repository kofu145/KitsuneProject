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

namespace KitsuneProject.Components
{
    class TestBulletGen : Component
    {
        private float rotateSpeed = 5000f;
        private float attackSpeed = .1f;
        private int spawnPointCount = 12;
        private BaseStraightBullet bullet;
        private Transform transform;
        private Transform spawnerTransform;
        private float bulletSpeed;
        private float nextFireEvent;

        //rotate speed
        //private float angleOffset = 10f;

        public TestBulletGen(Transform transform, Transform spawnerTransform, float bulletSpeed)
        {
            this.transform = transform;
            this.bulletSpeed = bulletSpeed;
            this.spawnerTransform = spawnerTransform;
            nextFireEvent = 0;
        }

        public override void OnLoad()
        {
            //var step = 2 * Math.PI / spawnPointCount;

            bullet = new BaseStraightBullet(transform, 300f, "./Resources/basic_bullet.png", 18f, 5f);
        }

        public override void Update(GameTime gameTime)
        {

            if (gameTime.totalTime.TotalSeconds >= nextFireEvent)
            {
                var bulletAngle = 360 / spawnPointCount;

                for (var i = 0; i < spawnPointCount; i++)
                {
                    var angle = (spawnerTransform.Rotation.Z + bulletAngle * i) * Math.PI / 180;

                    var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    direction.Normalize();

                    var newBullet = bullet.Instantiate();
                    var bulletRb = newBullet.GetComponent<Rigidbody>();
                    var bulletTransform = newBullet.GetComponent<Transform>();

                    bulletTransform.Position += new Vector3(direction.X, direction.Y, 0f) * 70f;
                    bulletRb.Velocity = new Vector2(direction.X, direction.Y) * bulletSpeed;
                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(newBullet);
                }

                spawnerTransform.Rotation.Z += rotateSpeed * (float)gameTime.deltaTime.TotalSeconds;
                nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;

            }
        }

        public override void FixedUpdate(GameTime gameTime)
        {

            
        }
    }
}
