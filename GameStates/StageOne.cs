using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Core.Math;
using KitsuneProject.Components;
using KitsuneProject.Events;
using Swordfish.Components.UI;

namespace KitsuneProject
{
    class StageOne : GameState
    {
        private Timeline timeline;
        private Entity player;
        private Sound RegularBGM;
        public static Sound regularBGMLoop;
        private bool inRegularBGMLoop;

        public static Sound bossBGM;
        public static Sound bossBGMLoop;
        private bool inRegBGMLoop;
        private bool inBGMLoop;
        public static bool bossCheck = false;

        public override void Initialize()
        {
            var cameraEntity = new Entity();
            cameraEntity.AddComponent(new Camera());
            cameraEntity.AddComponent(new Transform());

            
            GameScene.AddEntity(cameraEntity);

            RegularBGM = new Sound("./Resources/Sound/kf-main-start.wav");
            var regularBGMEntity = new Entity()
                .AddComponent(new Transform())
                .AddComponent(RegularBGM);

            regularBGMLoop = new Sound("./Resources/Sound/kf-main-loop.wav");
            var regularBGMLoopEntity = new Entity()
                .AddComponent(new Transform())
                .AddComponent(regularBGMLoop);

            GameScene.AddEntity(regularBGMEntity);
            GameScene.AddEntity(regularBGMLoopEntity);

            var heartEntity = new Entity()
                .AddComponent(new Sprite("./Resources/heart.png"))
                .AddComponent(new Transform(-280f, 380f, 0f, 0f, 0f, 0f, 5f, 5f));

            var bombEntity = new Entity()
                .AddComponent(new Sprite("./Resources/tbomb.png"))
                .AddComponent(new Transform(-280f, 330f, 0f, 0f, 0f, 0f, 5f, 5f));

            

            GameScene.AddEntity(heartEntity);
            GameScene.AddEntity(bombEntity);
            inRegBGMLoop = false;
            inBGMLoop = false;
            RegularBGM.Play();


            GameScene.SetBackgroundImage("./Resources/stageone.png");
            GameScene.backgroundOffset = new Vector2(0, 200);

            timeline = new Timeline();

            var heartLabelEntity = new Entity()
                .AddComponent(new Label("3", .5f, new Vector3(255f, 255f, 255f), new FontLibrary("./Resources/PressStart2P.ttf")))
                .AddComponent(new Transform(-240f, 370f, 0f, 0f, 0f, 0f, 1f, 1f));

            var bombLabelEntity = new Entity()
                .AddComponent(new Label("3", .5f, new Vector3(255f, 255f, 255f), new FontLibrary("./Resources/PressStart2P.ttf")))
                .AddComponent(new Transform(-240f, 320f, 0f, 0f, 0f, 0f, 1f, 1f));

            var playerTransform = new Transform(0f, -200f, 0f, 0f, 0f, 0f, 4f, 4f);
            player = new Entity()
                .AddComponent(new Sprite("./Resources/kitsune.png"))
                .AddComponent(playerTransform)
                .AddComponent(new Rigidbody())
                .AddComponent(new Player(400f, .13f, heartLabelEntity.GetComponent<Label>(), regularBGMLoop))
                .AddComponent(new CircleCollider(10f, false))
                .AddComponent(new TimeBomb(5f, 3, timeline, bombLabelEntity.GetComponent<Label>()));

            

            GameScene.AddEntity(heartLabelEntity);
            GameScene.AddEntity(bombLabelEntity);

            bossBGM = new Sound("./Resources/Sound/boss8bit-start.wav", 1f, 1f);
            var bossBgmEntity = new Entity()
                .AddComponent(bossBGM)
                .AddComponent(new Transform());

            bossBGMLoop = new Sound("./Resources/Sound/boss8bit-loop.wav", 1f, 1f);

            var bossBgmLoopEntity = new Entity()
                .AddComponent(bossBGMLoop)
                .AddComponent(new Transform());

            GameStateManager.Instance.GetScreen().GameScene.AddEntity(bossBgmEntity);
            GameStateManager.Instance.GetScreen().GameScene.AddEntity(bossBgmLoopEntity);
            

            timeline.AddEvent(new BezierCurveDownLeft(5f, playerTransform, false));
            timeline.AddEvent(new BezierCurveDownRight(15f, playerTransform, true));
            timeline.AddEvent(new SineWaveLeftAcross(25f));
            timeline.AddEvent(new CrissCrossBezierDown(35f, playerTransform));
            timeline.AddEvent(new BezierCurveDownLeft(45f, playerTransform, true));
            timeline.AddEvent(new SineWaveLeftAcross(55f));
            timeline.AddEvent(new BossEvent(65f, playerTransform, regularBGMLoop));



            //timeline.AddEvent(new SweepDownStraight(40f));

            GameScene.AddEntity(player);


            //timeline.AddEvent();
            timeline.BeginTimeline();
        }

        public override void OnLoad()
        {
            
        }

        public override void OnUnload()
        {
        }

        public override void Update(GameTime gameTime)
        {
            timeline.UpdateEvents(gameTime);
            //Console.WriteLine($"Current FPS: {FPS}");
            if (RegularBGM.State != Swordfish.Core.Audio.AudioState.Playing && !inRegBGMLoop && !bossCheck)
            {
                regularBGMLoop.Play();
                regularBGMLoop.SetLooping(true);
                regularBGMLoop.AudioOrderPriority = Swordfish.Core.Audio.AudioPriority.Critical;

                inRegBGMLoop = true;
            }
            if (bossCheck)
            {
                if (bossBGM.State != Swordfish.Core.Audio.AudioState.Playing && !inBGMLoop)
                {
                    bossBGMLoop.Play();
                    bossBGMLoop.SetLooping(true);
                    bossBGMLoop.AudioOrderPriority = Swordfish.Core.Audio.AudioPriority.Critical;

                    inBGMLoop = true;
                }
            }
            
            if (!TimeBomb.isActive)
                GameScene.backgroundOffset.Y -= 5f * (float)gameTime.deltaTime.TotalSeconds;
            GameScene.backgroundOffset.Y = Math.Clamp(GameScene.backgroundOffset.Y, -200, 200);

            //if (gameTime.totalTime.TotalSeconds >= 2f)
            //    Console.WriteLine("bababooey");
            var count = 0;
            foreach (var entity in GameScene.Entities)
            {
                var entityTransform = entity.GetComponent<Transform>();

                if (entityTransform.Position.X < -340 || entityTransform.Position.X > 340 ||
                    entityTransform.Position.Y < -440 || entityTransform.Position.Y > 440)
                        GameScene.DestroyEntity(entity);

                count++;
            }
        }

        public override void Draw()
        {
        }
    }
}
