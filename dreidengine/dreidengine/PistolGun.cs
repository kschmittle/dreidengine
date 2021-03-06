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
using System.Windows;

namespace dreidengine
{
    class PistolGun : Gun
    {
        public PistolGun(Game game, Vector3 pos)
            : base(game, "M9", pos, new Vector3(MathHelper.PiOver2 + 0.2f, -0.5f, 0.0f), 200, 2000, 10, 10000, false, 999, 15, 100, 10)
        {
            carryPos = new Vector3(0.4f, 0, 0);
        }
    }
}
