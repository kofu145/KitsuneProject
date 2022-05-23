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
    class GuardianOfTheForest : Component
    {
        private float rotateSpeed = 3000f;
        private float attackSpeed = .4f;
        private float sineAttackSpeed = .3f;
        private int spawnPointCount = 5;
        private BaseStraightBullet bullet;
        private BaseStraightBullet otherBullet;
        private Transform transform;
        private Transform spawnerTransform;
        private float posOffset;
        private float bulletSpeed;
        private float nextFireEvent;
        private float nextSineFireEvent;
        private bool isAlt;
        private Transform playerTransform;
        private bool rest;

        public GuardianOfTheForest(Transform transform, Transform spawnerTransform, float bulletSpeed, Transform playerTransform)
        {
            this.transform = transform;
            this.bulletSpeed = bulletSpeed;
            this.spawnerTransform = spawnerTransform;
            nextFireEvent = 0;
            nextSineFireEvent = 0;
            bullet = new BaseStraightBullet(transform, 200f, "./Resources/forest_basic_bullet.png", 15f, 5f);
            otherBullet = new BaseStraightBullet(transform, 200f, "./Resources/forest_shattered_bullet.png", 30f, 10f);
            isAlt = true;
            this.playerTransform = playerTransform;
            posOffset = 50;
            rest = true;
        }

        public override void OnLoad()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (rest)
            {
                nextFireEvent = (float)gameTime.totalTime.TotalSeconds + 3f;
                rest = false;
            }

            if (gameTime.totalTime.TotalSeconds >= nextFireEvent)
            {
                if (!TimeBomb.isActive)
                {
                    for (int i=0; i<spawnPointCount; i++)
                    {
                        MakeBullet(20-10*i);
                    }
                    
                    nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;

                }
            }

            if (gameTime.totalTime.TotalSeconds >= nextSineFireEvent)
            {
                if (!TimeBomb.isActive)
                {
                    var bull = otherBullet.Instantiate();
                    var bullTransform = bull.GetComponent<Transform>();
                    bullTransform.Position = new Vector3(280, 420f, 0f);
                    var bullRb = bull.GetComponent<Rigidbody>();
                    bullRb.Velocity = new Vector2(0, -bullet.speed);

                    var bull2 = otherBullet.Instantiate();
                    var bullTransform2 = bull2.GetComponent<Transform>();
                    bullTransform2.Position = new Vector3(-280, 420f, 0f);
                    var bullRb2 = bull2.GetComponent<Rigidbody>();
                    bullRb2.Velocity = new Vector2(0, -bullet.speed);

                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(bull);
                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(bull2);

                    nextSineFireEvent = (float)gameTime.totalTime.TotalSeconds + sineAttackSpeed;

                }
            }
        }

        private void MakeBullet(float variation)
        {
            var bull = bullet.Instantiate();

            var direction = playerTransform.Position - transform.Position;
            var angle = Math.Atan2(direction.Y, direction.X);
            angle += variation * (MathF.PI / 180);
            direction = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0);

            direction.Normalize();
            var bulletRb2 = bull.GetComponent<Rigidbody>();
            bulletRb2.Velocity = new Vector2(direction.X, direction.Y) * bullet.speed;
            GameStateManager.Instance.GetScreen().GameScene.AddEntity(bull);
        }
    }
}
