using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Components.UI;
using Swordfish.Prefabs;

namespace KitsuneProject.GameStates
{
    class GameOver : GameState
    {
        public override void Draw()
        {
        }

        public override void Initialize()
        {
            ContextButton ButtonPrefab = new ContextButton(new string[]
            { "./Resources/normal.png", "./Resources/hovered.png", "./Resources/pressed.png" },
            "Main Menu", 300, 200, 150, 600, 0);
            FontLibrary lib = new FontLibrary("./Resources/PressStart2P.ttf");
            Entity Button = ButtonPrefab.Instantiate();
            var buttonComp = Button.GetComponent<Button>();
            var animation = buttonComp.ParentEntity.GetComponent<Animation>();
            animation.SetTexture(0);
            buttonComp.OnButtonDown += () =>
            {
                // whatever you want to happen OnPress
                var animation = buttonComp.ParentEntity.GetComponent<Animation>();
                animation.SetTexture(2);
                GameStateManager.Instance.SwapScreen(new MainMenu());
            };

            buttonComp.OnButtonUp += () =>
            {
                var animation = buttonComp.ParentEntity.GetComponent<Animation>();
                if (buttonComp.isHovered)
                {
                    animation.SetTexture(1);
                }
                else
                {
                    animation.SetTexture(0);
                }
            };

            buttonComp.OnHover += () =>
            {
                var animation = buttonComp.ParentEntity.GetComponent<Animation>();
                animation.SetTexture(1);
            };

            buttonComp.OnStopHover += () =>
            {
                var animation = buttonComp.ParentEntity.GetComponent<Animation>();
                animation.SetTexture(0);
            };

            var labelEntity = new Entity()
                .AddComponent(new Label("Game Over!!", 1f, new Swordfish.Core.Math.Vector3(255f, 255f, 255f), new FontLibrary("./Resources/PressStart2P.ttf")))
                .AddComponent(new Transform(0f, 300f, 0f, 0f, 0f, 0f, 1f, 1f));

            GameScene.AddEntity(labelEntity);
            this.GameScene.AddEntity(Button);
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
    }
}
