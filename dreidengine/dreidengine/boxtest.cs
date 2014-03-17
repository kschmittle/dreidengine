﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using JigLibX.Physics;
using JigLibX.Geometry;
using JigLibX.Collision;

namespace dreidengine
{
    class boxtest : RenderableObject
    {
        private bool flagMovable = false;
        private Vector3 moveVector = Vector3.Zero;
        private float amount = 1.0f;
        public float Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        #region Constructors
        public boxtest(Game game, string name) 
            : base(game)
        {
            ModelName = name;
        }
        public boxtest(Game game, string name, Vector3 position)
            : base(game)
        {
            ModelName = name;
            Position = position;
            setBody(position);
        }
        public boxtest(Game game, string name, Vector3 position, Vector3 scale)
            : base(game)
        {
            ModelName = name;
            Position = position;
            Scale = scale;
            setBody(position);
        }

         public boxtest(Game game, string name, Vector3 position, Vector3 scale, bool movable)
            : base(game)
        {
            ModelName = name;
            Position = position;
            Scale = scale;
            flagMovable = movable;
            setBody(position);
        }

         public boxtest(Game game, string name, Vector3 position, Vector3 scale, bool movable, Vector3 rotation)
             : base(game)
         {
             ModelName = name;
             Position = position;
             Scale = scale;
             flagMovable = movable;
             Rotation = rotation;
             setBody(position);
         }
        #endregion

        public override void Update(GameTime gameTime)
         {
             if (flagMovable)
             {
                 moveVector = Vector3.Zero;
                 if (((Game1)this.Game).Keysp.IsKeyDown(Keys.W))
                     moveVector += new Vector3(0, 0, -1);
                 if (((Game1)this.Game).Keysp.IsKeyDown(Keys.S))
                     moveVector += new Vector3(0, 0, 1);
                 if (((Game1)this.Game).Keysp.IsKeyDown(Keys.A))
                     moveVector += new Vector3(-1, 0, 0);
                 if (((Game1)this.Game).Keysp.IsKeyDown(Keys.D))
                     moveVector += new Vector3(1, 0, 0);

                 addToPosition(moveVector * amount);

                 if (((Game1)this.Game).HeightMapObj.HMI.IsOnHeightmap(Body.Position))
                     Body.Position = new Vector3(Body.Position.X, ((Game1)this.Game).HeightMapObj.HMI.GetHeight(Body.Position) + Scale.Y /2, Body.Position.Z);

                 if (flagMovable == true)
                     Body.Velocity = Vector3.Zero;
             }
               
             base.Update(gameTime);
         }

        private void addToPosition(Vector3 vectorToAdd)
        {
            Matrix camRot = Matrix.CreateRotationX(((Game1)(this.Game)).Camera.RotX) * Matrix.CreateRotationY(((Game1)this.Game).Camera.RotY);
            Vector3 rotVector = Vector3.Transform(vectorToAdd, camRot);
            //Body.Position += rotVector;
            //Body.ApplyBodyImpulse(rotVector);
            Body.Velocity += rotVector;
        }
    }
}