using Swordfish.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.ECS;
using Swordfish.Components;
using KitsuneProject.Components;
using Swordfish.Core.Math;

namespace KitsuneProject.Events
{
    class BezierCurveDownLeft : IEvent
    {
        public float TriggerTime { get; private set; }

        public bool Finished { get; private set; }

        private int enemiesToSpawn;
        private int enemiesSpawned;
        private float spawnRate;
        private float nextSpawn;
        private Transform playerTransform;
        private bool alternate;

        public BezierCurveDownLeft(float triggerTime, Transform playerTransform, bool alternate)
        {
            TriggerTime = triggerTime;
            enemiesToSpawn = 5;
            spawnRate = 1f;
            nextSpawn = 0;
            enemiesSpawned = 0;
            this.playerTransform = playerTransform;
            this.alternate = alternate;
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
                    if (alternate)
                    {
                        var testEntity = new Entity()
                        .AddComponent(new Sprite("./Resources/forest_pixie.png"))
                        .AddComponent(moveCurveTransform)
                        .AddComponent(new CircleCollider(60f, false))
                        .AddComponent(new Enemy(70))
                        .AddComponent(new FollowCurve(moveCurveTransform,
                        100f,
                        new Vector2(-300, -100),
                        new Vector2(100, -100),
                        new Vector2(300, 100),
                        new Vector2(300, 400),
                        CurveDirection.Left
                        ))
                        .AddComponent(new PixieBehavior(playerTransform, .8f))
                        .AddComponent(new Rigidbody());
                        GameStateManager.Instance.GetScreen().GameScene.AddEntity(testEntity);
                    }
                    else
                    {
                        var testEntity = new Entity()
                        .AddComponent(new Sprite("./Resources/forest_fairy.png"))
                        .AddComponent(moveCurveTransform)
                        .AddComponent(new CircleCollider(80f, false))
                        .AddComponent(new Enemy(30))
                        .AddComponent(new FollowCurve(moveCurveTransform,
                        100f,
                        new Vector2(-300, -100),
                        new Vector2(100, -100),
                        new Vector2(300, 100),
                        new Vector2(300, 400),
                        CurveDirection.Left
                        ))
                        .AddComponent(new FairyBehavior(playerTransform, 1.5f))
                        .AddComponent(new Rigidbody());
                        GameStateManager.Instance.GetScreen().GameScene.AddEntity(testEntity);
                    }
                    
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
