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
    class FairyBehavior : Component
    {
        float speed;
        private Transform transform;
        private Transform targetTransform;
        private BaseHomingBullet bullet;
        private bool isFiring;
        private float nextFireEvent;
        private float attackSpeed;

        public FairyBehavior(Transform player, float attackSpeed)
        {
            targetTransform = player;
            isFiring = true;
            nextFireEvent = 0;
            this.attackSpeed = attackSpeed;
        }

        public override void OnLoad()
        {
            transform = ParentEntity.GetComponent<Transform>();
            bullet = new BaseHomingBullet(transform, targetTransform, 200f, "./Resources/basic_bullet.png");
            //Console.WriteLine(bullet.speed);

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


                    var bull2 = bullet.Instantiate();

                    var direction2 = targetTransform.Position - transform.Position;
                    var angle = Math.Atan2(direction2.Y, direction2.X);
                    angle += 15f * (MathF.PI / 180);
                    direction2 = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0);

                    direction2.Normalize();
                    var bulletRb2 = bull2.GetComponent<Rigidbody>();
                    bulletRb2.Velocity = new Vector2(direction2.X, direction2.Y) * bullet.speed;
                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(bull2);


                    var bull3 = bullet.Instantiate();

                    var direction3 = targetTransform.Position - transform.Position;
                    var angle2 = Math.Atan2(direction3.Y, direction3.X);
                    angle2 -= 15f * (MathF.PI / 180);
                    direction3 = new Vector3((float)Math.Cos(angle2), (float)Math.Sin(angle2), 0);

                    direction3.Normalize();
                    var bulletRb3 = bull3.GetComponent<Rigidbody>();
                    bulletRb3.Velocity = new Vector2(direction3.X, direction3.Y) * bullet.speed;
                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(bull3);


                    nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;
                }
                

            }

        }
    }
}
