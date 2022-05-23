using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Components;
using KitsuneProject.GameStates;

namespace KitsuneProject.Components
{
    class BossEnemy : Component
    {
        public int MaxHP;
        public int HP;
        public int spHP;
        public bool Frozen;
        private Transform transform;
        private Transform spawnerTransform;
        private float bulletSpeed;
        private Transform playerTransform;

        public BossEnemy(int hp, Transform transform, Transform spawnerTransform, float bulletSpeed, Transform playerTransform)
        {
            MaxHP = hp;
            HP = hp;
            spHP = 3;
            Frozen = false;
            this.transform = transform;
            this.spawnerTransform = spawnerTransform;
            this.bulletSpeed = bulletSpeed;
            this.playerTransform = playerTransform;
            
        }
        public override void OnLoad()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (spHP <= 0)
            {
                // player wins!
            }
            if (HP <= 0)
            {
                spHP--;
                switch (spHP)
                {
                    case 2:
                        ParentEntity.RemoveComponent<NaturesWrath>();
                        ParentEntity.AddComponent(new UnforgivingLotus(transform, spawnerTransform, bulletSpeed, playerTransform));
                        break;

                    case 1:
                        ParentEntity.RemoveComponent<UnforgivingLotus>();
                        ParentEntity.AddComponent(new GuardianOfTheForest(transform, spawnerTransform, bulletSpeed, playerTransform));
                        break;

                    case 0:
                        GameStateManager.Instance.ChangeScreen(new YouWon());
                        break;
                }
                HP = MaxHP;
            }
        }
    }
}
