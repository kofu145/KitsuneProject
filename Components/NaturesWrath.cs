using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitsuneProject.Prefabs.Bullets;
using Swordfish.Components;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Core.Math;

namespace KitsuneProject.Components
{
    class NaturesWrath : Component
    {
        private float rotateSpeed = 3000f;
        private float attackSpeed = .38f;
        private float bigAttackSpeed = 5f;
        private int spawnPointCount = 24;
        private float angleOffset = 45f;
        private BaseStraightBullet bullet;
        private BaseStraightBullet bigBullet;
        private Transform transform;
        private Transform spawnerTransform;
        private float bulletSpeed;
        private float nextFireEvent;
        private float nextSlowFireEvent;
        private bool isAlt;
        private Transform playerTransform;
        public NaturesWrath(Transform transform, Transform spawnerTransform, float bulletSpeed, Transform playerTransform)
        {
            this.transform = transform;
            this.bulletSpeed = bulletSpeed;
            this.spawnerTransform = spawnerTransform;
            nextFireEvent = 0;
            nextSlowFireEvent = 0;
            bullet = new BaseStraightBullet(transform, 80f, "./Resources/forest_basic_bullet.png", 15f, 5f);
            bigBullet = new BaseStraightBullet(transform, 70f, "./Resources/forest_other_bullet.png", 30f, 10f);
            isAlt = true;
            this.playerTransform = playerTransform;
            
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
                        var angle = (spawnerTransform.Rotation.Z + bulletAngle * i) * Math.PI / 180;

                        var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        direction.Normalize();

                        var newBullet = bullet.Instantiate();
                        var bulletRb = newBullet.GetComponent<Rigidbody>();
                        var bulletTransform = newBullet.GetComponent<Transform>();

                        bulletTransform.Position += new Vector3(direction.X, direction.Y, 0f) * 15f;
                        bulletRb.Velocity = new Vector2(direction.X, direction.Y) * bulletSpeed;
                        GameStateManager.Instance.GetScreen().GameScene.AddEntity(newBullet);
                    }

                    
                    nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;


                }

            }

            if (gameTime.totalTime.TotalSeconds >= nextSlowFireEvent)
            {
                if (!TimeBomb.isActive)
                {
                    var bull = bigBullet.Instantiate();

                    var homingDirection = playerTransform.Position - transform.Position;
                    homingDirection.Normalize();
                    var bigBulletRb = bull.GetComponent<Rigidbody>();
                    bigBulletRb.Velocity = new Vector2(homingDirection.X, homingDirection.Y) * bigBullet.speed;
                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(bull);
                }
                nextSlowFireEvent = (float)gameTime.totalTime.TotalSeconds + bigAttackSpeed;

            }

            spawnerTransform.Rotation.Z += this.rotateSpeed * (float)gameTime.deltaTime.TotalSeconds;

        }
    }
}
