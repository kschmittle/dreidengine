﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JigLibX.Physics;
using JigLibX.Geometry;
using JigLibX.Collision;

namespace dreidengine
{
    public class RenderableObject : DrawableGameComponent
    {
        private Vector3 position;
        private Vector3 scale;
        private Model model;
        private float rotation;

        private Box collisionPrimitive;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; updatePosition(); }
        }
        public float Rotation
        {
            get { return rotation; }
        }

        private string _modelName;
        public string ModelName
        {
            set { _modelName = value; }
        }

        private Body _body;
        public Body Body
        {
            get { return _body; }
        }

        private CollisionSkin _skin;
        public CollisionSkin Skin
        {
            get { return _skin; }
        }

        public RenderableObject(Game game) 
            : base(game)
        {
            //_modelName = "box";
            position = new Vector3(0,1,0);
            scale = new Vector3(0.5f, 0.5f, 0.5f);
            _body = new Body();
            _skin = new CollisionSkin(_body);
            _body.CollisionSkin = _skin;
            
            collisionPrimitive = new Box(position, Matrix.Identity, scale);
            
            _skin.AddPrimitive(collisionPrimitive, new MaterialProperties(0.8f, 0.8f, 0.7f));

            Vector3 com = SetMass(1.0f);

            _body.MoveTo(position, Matrix.Identity);
            _skin.ApplyLocalTransform(new JigLibX.Math.Transform(-com, Matrix.Identity));
            _body.EnableBody();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>(_modelName);
        }

        void updatePosition()
        {
            _skin.ApplyLocalTransform(new JigLibX.Math.Transform(position, Matrix.CreateRotationY(rotation)));
        }

        private Vector3 SetMass(float mass)
        {
            PrimitiveProperties primitiveProperties = new PrimitiveProperties(
                PrimitiveProperties.MassDistributionEnum.Solid,
                PrimitiveProperties.MassTypeEnum.Mass, mass);

            float junk;
            Vector3 com;
            Matrix it;
            Matrix itCoM;

            Skin.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);

            Body.BodyInertia = itCoM;
            Body.Mass = junk;

            return com;
        }
        private Matrix GetWorldMatrix()
        {
            return
                Matrix.CreateScale(scale) *
                _skin.GetPrimitiveLocal(0).Transform.Orientation *
                _body.Orientation *
                Matrix.CreateTranslation(_body.Position);
        }

        public override void Draw(GameTime gameTime)
        {
            Game1 game = (Game1)Game;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix worldMatrix = GetWorldMatrix();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = transforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = game.Camera.View;
                    effect.Projection = game.Camera.Projection;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }

    }
}
