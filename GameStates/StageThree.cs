using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.Components;
using Swordfish.ECS;

namespace KitsuneProject
{
    class StageThree : GameState
    {
        public override void Initialize()
        {
            var cameraEntity = new Entity();
            cameraEntity.AddComponent(new Camera());
            cameraEntity.AddComponent(new Transform());
            GameScene.AddEntity(cameraEntity);
        }

        public override void OnLoad()
        {
        }

        public override void OnUnload()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
        }
    }
}
