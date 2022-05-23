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
    class UnforgivingLotus : Component
    {

        private float rotateSpeed = 5000f;
        private float attackSpeed = .2f;
        private float bigAttackSpeed = 5f;
        private int spawnPointCount = 8;
        private float angleOffset = 45f;
        private BaseStraightBullet bullet;
        private BaseStraightBullet crystalBullet;
        private Transform transform;
        private Transform spawnerTransform;
        private float bulletSpeed;
        private float nextFireEvent;
        private float nextSlowFireEvent;
        private bool isAlt;
        private Transform playerTransform;
        private bool rest;
        public UnforgivingLotus(Transform transform, Transform spawnerTransform, float bulletSpeed, Transform playerTransform)
        {
            this.transform = transform;
            this.bulletSpeed = bulletSpeed;
            this.spawnerTransform = spawnerTransform;
            nextFireEvent = 0;
            nextSlowFireEvent = 0;
            bullet = new BaseStraightBullet(transform, 135f, "./Resources/forest_basic_bullet.png", 15f, 5f);
            crystalBullet = new BaseStraightBullet(transform, 135f, "./Resources/forest_crystal_bullet.png", 15f, 5f);
            isAlt = true;
            this.playerTransform = playerTransform;
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

                    var otherBulletAngle = 360 / (spawnPointCount/2);

                    for (var i = 0; i < spawnPointCount; i++)
                    {
                        var angle = (spawnerTransform.Rotation.Z + otherBulletAngle * i) * Math.PI / 180;

                        var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        direction.Normalize();

                        var newBullet = crystalBullet.Instantiate();
                        var bulletRb = newBullet.GetComponent<Rigidbody>();
                        var bulletTransform = newBullet.GetComponent<Transform>();

                        var modifier = i + 1;
                        
                        bulletTransform.Position += new Vector3(direction.X, direction.Y, 0f) * 15f;
                        bulletRb.Velocity = new Vector2(direction.X, direction.Y) * bulletSpeed * 2f;
                        GameStateManager.Instance.GetScreen().GameScene.AddEntity(newBullet);
                    }

                    nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;


                }

            }
            spawnerTransform.Rotation.Z += this.rotateSpeed * (float)gameTime.deltaTime.TotalSeconds;

        }
    }
}
