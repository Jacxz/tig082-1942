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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    // public class CollisionDetection : Microsoft.Xna.Framework.GameComponent
    public class CollisionDetection : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        // The images we will draw
        Texture2D mEnemyTexture;
        Texture2D mPlayerTexture;

        // The color data for the images; used for per pixel collision
        Color[] mEnemyTextureData;
        Color[] mPlayerTextureData;
        
        // The images will be drawn with this SpriteBatch
        SpriteBatch spriteBatch;

        Vector2 mPlayerOrigin, mEnemyOrigin, mWeaponOrigin;
        
        // Person 
        Vector2 personPosition;
        const int PersonMoveSpeed = 5;

        Player mPlayer;
        
        List<Weapon> mWeaponList = new List<Weapon>();
        List<Enemy> mEnemyList = new List<Enemy>();
        // Blocks
       // List<Block> blocks = new List<Block>();
        

        Random random = new Random();

        // For when a collision is detected
        bool personHit = false;

        // The sub-rectangle of the drawable area which should be visible on all TVs
        Rectangle safeBounds;
        // Percentage of the screen on every side is the safe area
        const float SafeAreaPortion = 0.05f;


        public CollisionDetection(Player player, List<Weapon> WeaponList, List<Enemy> EnemyList, Texture2D theTexture)
        {
            mPlayer = player;
            mWeaponList = WeaponList;
            mEnemyList = EnemyList;
            mPlayerTexture = theTexture;
            mEnemyTexture = theTexture;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

         
        }

        public CollisionDetection(Player player, List<Enemy> EnemyList)
        {
            mPlayer = player;
            mEnemyList = EnemyList;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        public CollisionDetection(List<Enemy> EnemyList, List<Weapon> WeaponList)
        {
            mEnemyList = EnemyList;
            mWeaponList = WeaponList;
            graphics = new GraphicsDeviceManager(this);
          

        }


        public void fuckIT()
        {
            LoadContent();
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to
        /// run. This is where it can query for any required services and load any
        /// non-graphic related content.  Calling base.Initialize will enumerate through
        /// any components and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();


        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load textures
//  blockTexture = mTexture;
  //          personTexture = mTexture;

            // Extract collision data
            
            mPlayerTextureData =
                new Color[mPlayerTexture.Height * mPlayerTexture.Width];
            //mPlayerTexture.GetData(mPlayerTextureData);
            mPlayerTexture.GetData(0, new Rectangle(136, 202, 32, 32), mPlayerTextureData, 32*32, 32*32);
     
            mEnemyTextureData =
                new Color[mEnemyTexture.Height * mEnemyTexture.Width];
            mEnemyTexture.GetData(0, new Rectangle(4, 4, 32, 32), mEnemyTextureData, 32*32, 32*32);

            // Calculate the block origin
            mWeaponOrigin =
                new Vector2(mPlayerTexture.Width / 2, mPlayerTexture.Height / 2);
            mEnemyOrigin =
                new Vector2(mPlayerTexture.Width / 2, mPlayerTexture.Height / 2);
            mPlayerOrigin =
                new Vector2(mPlayerTexture.Width / 2, mPlayerTexture.Height / 2);

            // Create a sprite batch to draw those textures
          //  spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Update the person's transform
        //    Matrix personTransform =
          //      Matrix.CreateTranslation(new Vector3(personPosition, 0.0f));

           
            base.Update(gameTime);
        }

        public void checkCollision()
        {
            Rectangle mPlayerRectangle = mPlayer.GetBounds();

            // Update each block
            personHit = false;
            for (int i = 0; i < mEnemyList.Count; i++)
            {


                // Build the block's transform
                /*     Matrix blockTransform =
                         Matrix.CreateTranslation(new Vector3(-mWeaponOrigin, 0.0f)) *
                         // Matrix.CreateScale(block.Scale) *  would go here
                         Matrix.CreateRotationZ(blocks[i].Rotation) *
                         Matrix.CreateTranslation(new Vector3(blocks[i].Position, 0.0f));
                     */
                // Calculate the bounding rectangle of this block in world space
                Rectangle mEnemyRectangle = mEnemyList[i].getBounds();

                // The per-pixel check is expensive, so check the bounding rectangles
                // first to prevent testing pixels when collisions are impossible.
                if (mPlayerRectangle.Intersects(mEnemyRectangle))
                {

                    // Check collision with person
                    if (IntersectPixels(mPlayerRectangle, mPlayerTextureData, mEnemyRectangle, mEnemyTextureData))
                    {
                        personHit = true;
                    }

                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            // Change the background to red when the person was hit by a block
            if (personHit)
            {
                device.Clear(Color.Red);
            }
            else
            {
                device.Clear(Color.Black);
            }


            spriteBatch.Begin();
            spriteBatch.End();


            base.Draw(gameTime);
        }


        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                           Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }


        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }


        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }
    }
}