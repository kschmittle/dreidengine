﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace dreidengine
{
    class SkyDome : DrawableGameComponent
    {
        string name;
        Vector3 scale;
        Vector3 position;
        private Model skyDome;
        private Texture2D cloudMap;
        private Effect skyEffect;
        string texName;

        public SkyDome(Game1 game, string name, string textureName, Vector3 pos, Vector3 scale)
            : base(game)
        {
            this.name = name;
            this.scale = scale;
            position = pos;
            texName = textureName;
        }
        protected override void LoadContent()
        {
            skyDome = Game.Content.Load<Model>(name);
            cloudMap = Game.Content.Load<Texture2D>(texName);
            skyEffect = Game.Content.Load<Effect>("sky");
            skyDome.Meshes[0].MeshParts[0].Effect = skyEffect.Clone();
            base.LoadContent();
        }

        private void DrawSkyDome()
        {
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // this.Game.GraphicsDevice.BlendState = BlendState.Opaque;
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            Matrix[] modelTransforms = new Matrix[skyDome.Bones.Count];
            skyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);
            if (cloudMap == null)
                return;
            Matrix wMatrix = Matrix.CreateScale(this.scale) * Matrix.CreateTranslation(position);// *Matrix.CreateTranslation(((Game1)this.Game).Camera.Position);
            foreach (ModelMesh mesh in skyDome.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(((Game1)this.Game).Camera.View);
                    currentEffect.Parameters["xProjection"].SetValue(((Game1)this.Game).Camera.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(cloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
               mesh.Draw();
            }
        }


        public override void Draw(GameTime gameTime)
        {
            DrawSkyDome();
            base.Draw(gameTime);
        }
    }
}
