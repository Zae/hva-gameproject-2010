using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNATweener;
using Microsoft.Xna.Framework.Input;

namespace Tweening
{
    class BasicDemo : DrawableGameComponent, IUsageText
    {
        public BasicDemo(Game game)
            : base(game)
        {
        }

        #region Fields
        protected SpriteBatch spriteBatch;

        protected Texture2D sprite;
        protected Vector2 spritePosition;
        protected float spriteScale;

        protected Texture2D pointer;
        protected Vector2 pointerPosition;

        protected Tweener tweenerX;
        protected Tweener tweenerY;

        float duration = 0.5f;

        Queue<Type> transitions;
        Type currentTransition;
        enum Easing { EaseIn, EaseOut, EaseInOut };
        Easing easing;

#if !XBOX
        MouseState oldMouseState;
#endif
        KeyboardState oldKeyboardState;
        #endregion

        public override void Initialize()
        {
            base.Initialize();

            transitions = new Queue<Type>(new Type[] {
                  typeof(Linear),
                  typeof(Quadratic),
                  typeof(Cubic),
                  typeof(Quartic),
                  typeof(Quintic),
                  typeof(Sinusoidal),
                  typeof(Exponential),
                  typeof(Circular),
                  typeof(Elastic),
                  typeof(Back),
                  typeof(Bounce)
                });
            SwitchTransition();
            easing = Easing.EaseIn;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprite = Game.Content.Load<Texture2D>("woot");
            pointer = Game.Content.Load<Texture2D>("cursorarrow");

            spritePosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            pointerPosition = spritePosition;
            spriteScale = 0.25f;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            bool newTweener = false;
#if !XBOX
            CheckMouseControls(ref newTweener);
#endif
            CheckKeyboardControls();

            if (newTweener)
            {
                CreateNewTweener();
            }

            if (tweenerX != null)
            {
                tweenerX.Update(gameTime);
                spritePosition.X = tweenerX.Position;
            }
            if (tweenerY != null)
            {
                tweenerY.Update(gameTime);
                spritePosition.Y = tweenerY.Position;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            spriteBatch.Draw(pointer, pointerPosition, null, Color.Yellow, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(sprite, spritePosition, null, Color.White, 0, new Vector2(sprite.Width, sprite.Height) / 2, spriteScale, SpriteEffects.None, 1);
            spriteBatch.End();
        }

        #region Input Methods
        private void CheckKeyboardControls()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (oldKeyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyUp(Keys.Up))
            {
                SwitchTransition();
            }
            if (oldKeyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyUp(Keys.Down))
            {
                SwitchEasing();
            }
            if (oldKeyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Left))
            {
                AdjustDuration(-1);
            }
            if (oldKeyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Right))
            {
                AdjustDuration(1);
            }
            oldKeyboardState = keyboardState;
        }

        private void CheckMouseControls(ref bool newTweener)
        {
            MouseState mouseState = Mouse.GetState();
            pointerPosition.X = mouseState.X;
            pointerPosition.Y = mouseState.Y;
            KeyboardState keyboardState = Keyboard.GetState();
            if ((oldMouseState.LeftButton == ButtonState.Pressed) && (mouseState.LeftButton == ButtonState.Released))
            {
                if (keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl))
                {
                    ResetTweener(false);
                }
                else if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    ResetTweener(true);
                }
                else
                {
                    newTweener = true;
                }
            }
            if ((oldMouseState.RightButton == ButtonState.Pressed) && (mouseState.RightButton == ButtonState.Released))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    ReverseTweener();
                }
                else
                {
                    ToggleTweenerRunning();
                }
            }
            oldMouseState = mouseState;
        }
        #endregion

        #region Draw helpers
        public IEnumerable<string> GetUsageText()
        {
            return new string[] {
                String.Format("Left Click: Apply new transition {0} {1} for {2:##0.0} secs", currentTransition.Name, easing, duration),
                "Ctrl + Left Click: Reset transition",
                "Shift + Left Click: Reset transition with new position",
                (tweenerX == null) ? "" : "Right Click: " + ((tweenerX.Running) ? "Stop" : "Start"),
                "Up: Switch transition",
                "Down: Switch easing",
                "Left/Right: Change speed",
                "Ctrl + Left Click: Reverse (does not change transition type)",
                (tweenerX == null) ? "" : "Running: " + tweenerX,
                (tweenerY == null) ? "" : "Running: " + tweenerY
            };
        }
    	#endregion

        #region Action functions
        protected TweeningFunction GetTweeningFunction()
        {
            return (TweeningFunction)Delegate.CreateDelegate(typeof(TweeningFunction), currentTransition, easing.ToString());
        }

        protected virtual void CreateNewTweener()
        {
            tweenerX = new Tweener(spritePosition.X, pointerPosition.X, TimeSpan.FromSeconds(duration), GetTweeningFunction());
            tweenerY = new Tweener(spritePosition.Y, pointerPosition.Y, TimeSpan.FromSeconds(duration), GetTweeningFunction());
        }

        private void SwitchTransition()
        {
            if (currentTransition != null)
            {
                transitions.Enqueue(currentTransition);
            }
            currentTransition = transitions.Dequeue();
        }

        private void SwitchEasing()
        {
            if (easing == Easing.EaseInOut)
            {
                easing = Easing.EaseIn;
            }
            else
            {
                easing++;
            }
        }

        private void AdjustDuration(int amount)
        {
            if (duration < 1.0f)
            {
                duration += amount * 0.1f;
                return;
            }
            if (duration < 5.0f)
            {
                duration += amount * 0.5f;
                return;
            }
            duration += amount;
        }

        private void ReverseTweener()
        {
            if (tweenerX != null)
            {
                tweenerX.Reverse();
                tweenerY.Reverse();
            }
        }

        private void ToggleTweenerRunning()
        {
            if (tweenerX != null)
            {
                if (tweenerX.Running)
                {
                    tweenerX.Stop();
                    tweenerY.Stop();
                }
                else
                {
                    tweenerX.Start();
                    tweenerY.Start();
                }
            }
        }

        private void ResetTweener(bool resetToPosition)
        {
            if (resetToPosition)
            {
                tweenerX.Reset(pointerPosition.X);
                tweenerY.Reset(pointerPosition.Y);
            }
            else
            {
                tweenerX.Reset();
                tweenerY.Reset();
            }
        }
        #endregion
    }
}
