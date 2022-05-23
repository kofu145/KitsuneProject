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
    class FollowComponent : Component
    {
        private Transform transform;
        private Transform targetTransform;
        private float offset;
        public FollowComponent(Transform targetTransform, float offset)
        {
            this.targetTransform = targetTransform;
            this.offset = offset;
        }
        public override void OnLoad()
        {
            transform = ParentEntity.GetComponent<Transform>();

        }

        public override void Update(GameTime gameTime)
        {
            transform.Position.X = targetTransform.Position.X;
            transform.Position.Y = targetTransform.Position.Y;
            transform.Position.Y += offset;
        }
    }
}
