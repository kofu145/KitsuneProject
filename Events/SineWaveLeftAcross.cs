using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.ECS;
using Swordfish.Components;
using KitsuneProject.Components;
using Swordfish.Core.Math;
using Swordfish.Core;

namespace KitsuneProject.Events
{
    class SineWaveLeftAcross : IEvent
    {
        public float TriggerTime { get; private set; }

        public bool Finished { get; private set; }
        private int enemiesToSpawn;
        private int enemiesSpawned;
        private float spawnRate;
        private float nextSpawn;

        public SineWaveLeftAcross(float triggerTime)
        {
            TriggerTime = triggerTime;
            enemiesToSpawn = 5;
            spawnRate = 1f;
            nextSpawn = 0;
            enemiesSpawned = 0;

        }

        public void Start()
        {

        }

        public void Update(GameTime gameTime)
        {
            

            if (gameTime.totalTime.TotalSeconds > nextSpawn && enemiesSpawned < enemiesToSpawn)
            {
                if (!TimeBomb.isActive)
                {
                    var moveCurveTransform = new Transform(300f, 100f, 0f, 0f, 0f, 0f, 5f, 5f);
                    var testMoveEntity = new Entity()
                        .AddComponent(new Sprite("./Resources/forest_sprite.png"))
                        .AddComponent(moveCurveTransform)
                    .AddComponent(new CircleCollider(16f, false))
                    .AddComponent(new Enemy(50))

                    .AddComponent(new SpriteBehavior(moveCurveTransform, 200f))
                    .AddComponent(new FollowSineWave(moveCurveTransform, 150f, Vector3.Up))
                    //.AddComponent(new TestBulletGen(testEntityTransform, spawnerTransform, 300f))
                    .AddComponent(new Rigidbody());
                    GameStateManager.Instance.GetScreen().GameScene.AddEntity(testMoveEntity);

                    nextSpawn = (float)gameTime.totalTime.TotalSeconds + spawnRate;
                    enemiesSpawned++;
                    if (enemiesSpawned == enemiesToSpawn)
                    {
                        Finished = true;
                    }
                }

            }
        }
    }
}
