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
using Swordfish.Components.UI;

using KitsuneProject.Events;
using Swordfish.Prefabs;

namespace KitsuneProject.GameStates
{
    class MainMenu : GameState
    {
        public override void Draw()
        {
        }

        public override void Initialize()
        {
            Camera C = new Camera();
            GameScene.AddEntity(new Entity(C).AddComponent(new Transform()));
            GameScene.SetBackgroundImage("./Resources/titlescree.png");
            ContextButton ButtonPrefab = new ContextButton(new string[]
            { "./Resources/normal.png", "./Resources/hovered.png", "./Resources/pressed.png" },
            "Start", 300, 200, 150, 200, 0);
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
                GameStateManager.Instance.AddScreen(new StageOne());
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
