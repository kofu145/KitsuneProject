using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.Components;
using Swordfish.ECS;
using Swordfish.Core.Math;
using KitsuneProject.Components;

namespace KitsuneProject.Events
{
    class BossEvent : IEvent
    {
        public float TriggerTime { get; private set; }

        public bool Finished { get; private set; }

        private Entity boss;
        private Entity spawnerEntity;
        private Transform transform;
        private bool approaching;
        private Transform playerTransform;
        private Transform spawnerTransform;
        private bool fightStarted;
        private Sound regularBGMLoop;
        private bool inBGMLoop;
        private Sound bossBGM;
        private Sound bossBGMLoop;

        public BossEvent(float triggerTime, Transform playerTransform, Sound regularBGMLoop)
        {
            TriggerTime = triggerTime;
            approaching = true;
            this.playerTransform = playerTransform;
            fightStarted = false;
            this.regularBGMLoop = regularBGMLoop;
            inBGMLoop = false;
            

            

        }

        public void Start()
        {
            Console.WriteLine("start called");
            StageOne.bossCheck = true;
            StageOne.regularBGMLoop.Stop();
            StageOne.bossBGM.Play();

            transform = new Transform(0f, 430f, 2f, 0f, 0f, 0f, 5.5f, 5.5f);
            spawnerTransform = new Transform();
            boss = new Entity()
                .AddComponent(new Sprite("./Resources/dryad.png"))
                .AddComponent(new CircleCollider(56, false))
                .AddComponent(new BossEnemy(9000, transform, spawnerTransform, 200f, playerTransform))
                .AddComponent(transform)
                .AddComponent(new Rigidbody());
            
            spawnerEntity = new Entity()
                .AddComponent(spawnerTransform)
                .AddComponent(new FollowComponent(transform, 0));
            
            GameStateManager.Instance.GetScreen().GameScene.AddEntity(boss);
        }
        public void Update(GameTime gameTime)
        {

            
            if (approaching)
            {
                transform.Position.Y -= 200f * (float)gameTime.deltaTime.TotalSeconds;
                transform.Position.Y = Math.Clamp(transform.Position.Y, 300, 450);
                if (transform.Position.Y <= 300)
                {
                    approaching = false;
                }
            }
            if (!fightStarted && !approaching)
            {
                fightStarted = true;
                boss.AddComponent(new NaturesWrath(transform, spawnerTransform, 200f, playerTransform));
                //var testBehavior = new FairyBehavior(playerTransform, .4f);
                //boss.AddComponent(testBehavior);
                //testBehavior.OnLoad();
            }

        }
    }
}
