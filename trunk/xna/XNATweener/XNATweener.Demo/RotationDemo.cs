using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XNATweener;
using Microsoft.Xna.Framework.Graphics;

namespace Tweening
{
    class RotationDemo : BasicDemo
    {
        public RotationDemo(Game game)
            : base(game)
        {
        }

        float rotation = 0.0f;
        Tweener rotationTweener;

        public override void Initialize()
        {
            base.Initialize();

            rotationTweener = new Tweener(0.0f, (float)Math.PI * 2, 0.5f, GetTweeningFunction());
            rotationTweener.Stop();
            rotationTweener.Ended += rotationTweener.Reset;
            rotationTweener.Ended += rotationTweener.Stop;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            rotationTweener.Update(gameTime);
            rotation = rotationTweener.Position;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            spriteBatch.Draw(pointer, pointerPosition, null, Color.Yellow, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(sprite, spritePosition, null, Color.White, rotation, new Vector2(sprite.Width, sprite.Height) / 2, spriteScale, SpriteEffects.None, 1);
            spriteBatch.End();
        }

        protected override void CreateNewTweener()
        {
            base.CreateNewTweener();
            tweenerX.Ended += rotationTweener.Start;
        }
    }
}
