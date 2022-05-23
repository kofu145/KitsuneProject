using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swordfish.Core;
using Swordfish.ECS;
using Swordfish.Components;
using Swordfish.Core.Math;

namespace KitsuneProject.Components
{
    public enum CurveDirection
    {
        Left = 1,
        Right = 2
    }

    class FollowCurve : Component
    {

        private Transform transform;
        private float speed;
        private Vector2 p0;
        private Vector2 p1;
        private Vector2 p2;
        private Vector2 p3;
        private CurveDirection curveDirection;

        public FollowCurve(Transform transform, float speed, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, CurveDirection curveDirection)
        {
            this.transform = transform;
            this.speed = speed;
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.curveDirection = curveDirection;
        }

        public override void OnLoad()
        {
        }

        public override void Update(GameTime gameTime)
        {
            var length = MathUtil.GetBezierArcLength(
                p0,
                p1,
                p2,
                p3
                ) ;
            float normalizedX = MathUtil.GetNormalizedPointFromInterval(-length/2, length/2, transform.Position.X * (length/600));
            if (curveDirection == CurveDirection.Left)
            {
                transform.Position.X -= speed * (float)gameTime.deltaTime.TotalSeconds;
                var resultYPos = MathUtil.ExplicitCubicBezierCurve(normalizedX, p0, p1, p2, p3);
                transform.Position.Y = resultYPos.Y;

            }
            if (curveDirection == CurveDirection.Right)
            {
                transform.Position.X += speed * (float)gameTime.deltaTime.TotalSeconds;
                var resultYPos = MathUtil.ExplicitCubicBezierCurve(1-normalizedX, p0, p1, p2, p3);
                transform.Position.Y = resultYPos.Y;


            }
            //transform.Position.Y = ((transform.Position.X + 200) *(transform.Position.X + 200)/530);







            //Console.WriteLine("Length is " + length);
            //Console.WriteLine("Position is at: " + transform.Position + " Normalized (t) is at: " + normalizedX);
        }
    }
}
