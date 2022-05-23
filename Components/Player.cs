using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish;
using Swordfish.Core;
using Swordfish.Core.Input;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Core.Math;
using KitsuneProject.Prefabs;
using KitsuneProject.GameStates;
using Swordfish.Components.UI;

namespace KitsuneProject.Components
{
    public class Player : Component
    {
        public int Health;
        private Rigidbody rigidbody;
        private Transform transform;
        private Sprite sprite;
        public float speed;
        private PlayerBullet bullet;
        private bool isFiring;
        private float nextFireEvent;
        private float attackSpeed;
        private bool focusing;
        private Entity focusEntity;
        private bool isInvincible;
        private float loseInvincibleEvent;
        private float invincibleDuration;
        private float flickerTime;
        private float flickerEvent;
        private Label heartLabel;
        private Sound regularBGMLoop;

        // go ahead and complain about this scuffed solution, I don't care, I'm tired.
        private GameTime gameTime;

        public Player(float speed, float attackSpeed, Label heartLabel, Sound regularBGM)
        {
            Health = 3;
            this.speed = speed;
            this.attackSpeed = attackSpeed;
            nextFireEvent = 0;
            isInvincible = false;
            loseInvincibleEvent = 0;
            invincibleDuration = 4f;
            flickerTime = .1f;
            flickerEvent = 0;
            regularBGMLoop = regularBGM;
            this.heartLabel = heartLabel;

        }
        public override void OnLoad()
        {
            rigidbody = ParentEntity.GetComponent<Rigidbody>();
            transform = ParentEntity.GetComponent<Transform>();
            sprite = ParentEntity.GetComponent<Sprite>();
            bullet = new PlayerBullet(transform, rigidbody, new Vector2(0, 1000f), 10);
            focusing = false;
            
        }

        public void TakeDamage()
        {
            if (!isInvincible)
            {
                Health--;
                heartLabel.Text = Health.ToString();
                if (Health <= 0)
                {
                    regularBGMLoop.Stop();
                    GameStateManager.Instance.ChangeScreen(new GameOver());
                }

                isInvincible = true;
                loseInvincibleEvent = (float)gameTime.totalTime.TotalSeconds + invincibleDuration;
                flickerEvent = (float)gameTime.totalTime.TotalSeconds + flickerTime;
                transform.Position = new Vector3(0f, -200f, 0f);

                var sound = new Sound("./Resources/Sound/hurt.wav");
                var soundEntity = new Entity()
                    .AddComponent(sound)
                    .AddComponent(new Transform());

                GameStateManager.Instance.GetScreen().GameScene.AddEntity(soundEntity);
                sound.Play();
                GameStateManager.Instance.GetScreen().GameScene.DestroyEntity(soundEntity);
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            var scene = GameStateManager.Instance.GetScreen().GameScene;
            var Direction = Vector2.Zero;

            if (InputManager.Instance.GetKeyDown(Keys.W))
            {
                Direction.Y = 1;
            }
            if (InputManager.Instance.GetKeyDown(Keys.S))
            {
                Direction.Y = -1;
            }
            if (InputManager.Instance.GetKeyDown(Keys.A))
            {
                Direction.X = -1;
            }
            if (InputManager.Instance.GetKeyDown(Keys.D))
            {
                Direction.X = 1;
            }

            if (InputManager.Instance.GetKeyDown(Keys.Space))
            {
                isFiring = true;
            }

            if (InputManager.Instance.GetKeyUp(Keys.Space))
            {
                isFiring = false;
            }

            if (InputManager.Instance.GetKeyDown(Keys.LeftShift) && !focusing)
            {
                focusing = true;
                speed -= 150;
                focusEntity = new Entity()
                .AddComponent(new Sprite("./Resources/basic_bullet.png"))
                .AddComponent(new Transform(0f, 0f, 1f, 0f, 0f, 0f, 3f, 3f))
                .AddComponent(new FollowComponent(transform, 0));
                scene.AddEntity(focusEntity);
            }
            if (InputManager.Instance.GetKeyUp(Keys.LeftShift) && focusing)
            {
                focusing = false;
                speed += 150;
                scene.DestroyEntity(focusEntity);
            }

            if (Direction != Vector2.Zero)
            {
                Direction.Normalize();
            }

            if (isFiring && gameTime.totalTime.TotalSeconds >= nextFireEvent)
            {
                var bull = bullet.Instantiate();
                scene.AddEntity(bull);

                nextFireEvent = (float)gameTime.totalTime.TotalSeconds + attackSpeed;

            }

            if (isInvincible && gameTime.totalTime.TotalSeconds > loseInvincibleEvent)
            {
                isInvincible = false;
                sprite.Enabled = true;
            }

            if (isInvincible && gameTime.totalTime.TotalSeconds > flickerEvent)
                sprite.Enabled = !sprite.Enabled;

            rigidbody.Velocity = Direction * speed;

            float boundaryX = WindowManager.Instance.Bounds.X;
            float boundaryY = WindowManager.Instance.Bounds.Y;


            transform.Position.X = Math.Clamp(
                transform.Position.X,
                (-boundaryX / 2 + ((sprite.Width+.05f * transform.Scale.X) / 2)), 
                (boundaryX / 2- ((sprite.Width-.05f * transform.Scale.X) / 2)));
            transform.Position.Y = Math.Clamp(transform.Position.Y, 
                -boundaryY / 2 + (sprite.Height * transform.Scale.Y / 2), 
                boundaryY / 2 -(sprite.Height * transform.Scale.Y / 2));


        }
    }
}
