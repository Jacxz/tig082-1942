using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
namespace Game1942
{
    public class PowerUp : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Vector2 mPosition, namePosition;
        protected int mType;
        protected float xSpeed, ySpeed;
        protected SpriteBatch mSpriteBatch;
        protected SpriteFont gameFont;
        protected Texture2D mTexture;
        protected bool canBeRemoved = false;
        private AnimationPlayer AnimationPlayer;
        private Animation PowerUpAnimation;


        public PowerUp(Game game, Texture2D theTexture, int type, float Xspe, float Yspe, Vector2 startPos)
            : base(game)
        {
            mTexture = theTexture;
            mPosition = startPos;
            mType = type;
            xSpeed = Xspe;
            ySpeed = Yspe;
            // Get the current spritebatch
            mSpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            // creates the animationplayer
            AnimationPlayer = new AnimationPlayer(game);            
            // creates the animations
            PowerUpAnimation = new Animation(game, mTexture, mType);
            AnimationPlayer.PlayAnimation(PowerUpAnimation);
        }

        public override void Update(GameTime gameTime)
        {
                mPosition.Y += ySpeed;
                mPosition.X += xSpeed;
        }
            
        public override void Draw(GameTime gameTime)
        {
            AnimationPlayer.Draw(gameTime, mSpriteBatch, mPosition);

        }

        public bool checkCollision(Rectangle rect)
        {
            return getBounds().Intersects(rect);
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)mPosition.X, (int)mPosition.Y, PowerUpAnimation.FrameWidth, PowerUpAnimation.FrameHeight);
        }

        public void Remove()
        {
            canBeRemoved = true;
        }

        public bool CanBeRemoved()
        {
            return canBeRemoved;
        }
    }
}
