/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Core.Math;
using KitsuneProject.Components;

namespace KitsuneProject
{
    class TestGameState : GameState
    {
        private Entity testEntityOne;
        private Entity testEntityTwo;
        private int totalFrames;
        public double FPS;
        public override void Initialize()
        {
            var cameraEntity = new Entity();
            cameraEntity.AddComponent(new Camera());
            cameraEntity.AddComponent(new Transform());
            GameScene.AddEntity(cameraEntity);
        }

        public override void OnLoad()
        {
            GameScene.SetBackgroundImage("./Resources/stageone.png");
            GameScene.backgroundOffset = new Vector2(0, 200);

            var testPlayerEntity = new Entity()
                .AddComponent(new Sprite("./Resources/kitsune.png"))
                .AddComponent(new Transform(0f, 0f, 0f, 0f, 0f, 0f, 4f, 4f))
                .AddComponent(new Rigidbody())
                .AddComponent(new Player(400f, .13f, new Swordfish.Components.UI.Label("blabla", 10f, new Vector3(0f, 0f, 0f), new FontLibrary("./Resources/PressStart2P.ttf"))), new Sound("./Resources/hurt.wav"));
                //.AddComponent(new TimeBomb(5f, 3));

            
            var spawnerTransform = new Transform();

            var testEntityTransform = new Transform(0f, 200f, 5f, 0f, 0f, 0f, 5f, 5f);
            testEntityOne = new Entity()
                .AddComponent(new Sprite("./Resources/TestEnemy.png"))
                .AddComponent(testEntityTransform)
            .AddComponent(new CircleCollider(16f, false))
            .AddComponent(new Enemy(30))
            //.AddComponent(new FairloyBehavior(testPlayerEntity.GetComponent<Transform>(), .2f))
            .AddComponent(new TestBulletGen(testEntityTransform, spawnerTransform, 300f))
            .AddComponent(new Rigidbody());

            var spawnerEntity = new Entity()
                .AddComponent(spawnerTransform)
                .AddComponent(new FollowComponent( testEntityTransform, 200f));


            var moveCurveTransform = new Transform(300f, 100f, 0f, 0f, 0f, 0f, 5f, 5f);
            var testMoveEntity = new Entity()
                .AddComponent(new Sprite("./Resources/TestEnemy.png"))
                .AddComponent(moveCurveTransform)
            .AddComponent(new CircleCollider(16f, false))
            .AddComponent(new Enemy(30))
            .AddComponent(new FairyBehavior(testPlayerEntity.GetComponent<Transform>(), .15f))
            /*.AddComponent(new FollowCurve(moveCurveTransform, 150f,
                new Vector2(-300, -200),
                new Vector2(100, -200),
                new Vector2(300, 0),
                new Vector2(300, 400)))
            .AddComponent(new FollowSineWave(moveCurveTransform, 150f, Vector3.Up))
            //.AddComponent(new TestBulletGen(testEntityTransform, spawnerTransform, 300f))
            .AddComponent(new Rigidbody());

            var pointEntity0 = new Entity()
                .AddComponent(new Transform(-300, -200, 0, 0, 0, 0, 3f, 3f))
                .AddComponent(new Sprite("./Resources/basic_bullet.png"));

            var pointEntity1 = new Entity()
                .AddComponent(new Transform(100, -200, 0, 0, 0, 0, 3f, 3f))
                .AddComponent(new Sprite("./Resources/basic_bullet.png"));

            var pointEntity2 = new Entity()
                .AddComponent(new Transform(300, 0, 0, 0, 0, 0, 3f, 3f))
                .AddComponent(new Sprite("./Resources/basic_bullet.png"));

            var pointEntity3 = new Entity()
                .AddComponent(new Transform(300, 400, 0, 0, 0, 0, 3f, 3f))
                .AddComponent(new Sprite("./Resources/basic_bullet.png"));

            var pointEntity4 = new Entity()
                .AddComponent(new Transform(193.6f, 0, 0, 0, 0, 0, 3f, 3f))
                .AddComponent(new Sprite("./Resources/basic_bullet.png", 0, 0, 255));

            var pointEntity5 = new Entity()
                .AddComponent(new Transform(50f, -125f, 0, 0, 0, 0, 3f, 3f))
                .AddComponent(new Sprite("./Resources/basic_bullet.png", 0, 0, 255));

            GameScene.AddEntity(pointEntity0);
            GameScene.AddEntity(pointEntity1);
            GameScene.AddEntity(pointEntity2);
            GameScene.AddEntity(pointEntity3);
            GameScene.AddEntity(pointEntity4);
            GameScene.AddEntity(pointEntity5);



            //testEntityOne.GetComponent<Rigidbody>().Velocity = new Vector2(-500, 0);

            GameScene.AddEntity(testMoveEntity);
            GameScene.AddEntity(testPlayerEntity);
            //GameScene.AddEntity(spawnerEntity);
            //GameScene.AddEntity(testEntityOne);

            
            var rnd = new Random();
            for(int i=0; i<1000; i++)
            {
                var entity = new Entity()
                .AddComponent(new Sprite("./Resources/basic_bullet.png"))
                .AddComponent(new Transform(rnd.Next(-500, 500), rnd.Next(-200, 200), 0f, 0f, 0f, 0f, 2f, 2f))
                .AddComponent(new CircleCollider(16f, false))
                .AddComponent(new Rigidbody());
                ;
                entity.GetComponent<CircleCollider>().OnCollision += (CircleCollider other) =>
                {
                    entity.GetComponent<Sprite>().Color = new Vector3(0, 0, 255);
                };

                entity.GetComponent<CircleCollider>().OnExitCollision += (CircleCollider other) =>
                {
                    entity.GetComponent<Sprite>().Color = new Vector3(1f, 1f, 1f);
                };

                entity.GetComponent<Rigidbody>().Velocity = new Vector2(rnd.Next(-100, 100), rnd.Next(-100, 100));
                GameScene.Entities.Add(entity);
                
            }
            //GameStateManager.Instance.AddScreen(new StageOne());


        }

        public override void OnUnload()
        {
        }

        public override void Update(GameTime gameTime)
        {
            totalFrames++;
            FPS = 1.0f / gameTime.deltaTime.TotalSeconds;
            //Console.WriteLine($"Current FPS: {FPS}");

            if (!TimeBomb.isActive)
                GameScene.backgroundOffset.Y -= 100f * (float)gameTime.deltaTime.TotalSeconds;
            GameScene.backgroundOffset.Y = Math.Clamp(GameScene.backgroundOffset.Y, -200, 200);

            //if (gameTime.totalTime.TotalSeconds >= 2f)
            //    Console.WriteLine("bababooey");
            var count = 0;
            foreach (var entity in GameScene.Entities)
            {
                var entityTransform = entity.GetComponent<Transform>();

                if (entityTransform.Position.X < -320 || entityTransform.Position.X > 320 ||
                    entityTransform.Position.Y < -420 || entityTransform.Position.Y > 420)
                    if (!entity.HasComponent<Enemy>())
                    {
                        GameScene.DestroyEntity(entity);

                    }
                count++;
            }
            //Console.WriteLine(count);
            //testEntityOne.GetComponent<Transform>().Position.X -= 5f;
            //testEntityTwo.GetComponent<Transform>().Position.X += 4f;
        }

        public override void Draw()
        {
        }
    }
}
*/