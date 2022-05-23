using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Components;
using Swordfish.Core;
using Swordfish.ECS;

namespace KitsuneProject.Components
{
    class Enemy : Component
    {
        public int MaxHP;
        public int HP;
        public bool Frozen;

        public Enemy(int maxHP)
        {
            MaxHP = maxHP;
            HP = maxHP;
            Frozen = false;
        }

        public override void OnLoad()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (HP <= 0)
            {
                GameStateManager.Instance.GetScreen().GameScene.DestroyEntity(ParentEntity);
            }

        }
    }
}
