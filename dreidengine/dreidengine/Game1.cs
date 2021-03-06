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
using JigLibX.Physics;
using JigLibX.Geometry;
using JigLibX.Collision;

namespace dreidengine
{
  
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Random r = new Random();
        static Game1 gameInstance;
        public static Game1 GetInstance()
        {
            return gameInstance;
        }

        private Character c1;
        public Character C1
        {
            get { return c1; }
        }

       

        PhysicsSystem world;
        public PhysicsSystem World
        {
            get { return world; }
        }

        Model terrainModel;
        
        GraphicsDeviceManager graphics;
        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }
        SpriteBatch spriteBatch;

        KeyboardState keys, oldKeys;
        
        public KeyboardState Keysp
        {
            get { return keys; }
        }
        public KeyboardState OldKeysp
        {
            get { return oldKeys; }
        }

        SpriteFont font;

        NavMesh _navMesh;
        public NavMesh navMesh { get { return _navMesh; } }

        HeightmapObject heightmapObj;
        public HeightmapObject HeightMapObj
        {
            get { return heightmapObj; }
        }

        private Camera _camera;
        public Camera Camera
        {
            get { return _camera; }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameInstance = this;

            InitializePhyics();
        }

        private void InitializePhyics()
        {
            this.IsMouseVisible = false;
            world = new PhysicsSystem();
            world.CollisionSystem = new CollisionSystemSAP();
            world.Gravity = new Vector3(0, -400, 0);

            intro introduction = new intro(this, "cloudMap");
            
            c1 = new Character(this, new Vector3(0, 650, 40), Vector3.One);

            _navMesh = new NavMesh(this, "navMesh");

            PistolGun pistol = new PistolGun(this, new Vector3(19, -15, 10));
            MachineGun machine = new MachineGun(this, new Vector3(20, -15, 20));
            Knife knife = new Knife(this, new Vector3(17, -15, 10));

            Overlay hud = new Overlay(this, GraphicsDevice);
            Overlay crosshair = new Overlay(this, GraphicsDevice);

            boxtest b1 = new boxtest(this, "box", new Vector3(0, 10, 20), new Vector3(10, 10, 10), false);
            b1.TakesDamage = true;
            b1.CurLife = 100;

            Room room = new Room(this, Vector3.Down * 63, Vector3.One);

            _camera = new Camera(this, c1, 10.0f, 6/8f);
            _camera.Lookat = c1.Body.Position;
            _camera.CameraMode = Camera.CameraModes.FIRST_PERSON;

            SkyDome sky = new SkyDome(this, "dome", "white", Vector3.Up * -150, new Vector3(390, 8500, 390));
            introduction.DrawOrder = 500;

            squid s;
            BillBoarding billy = new BillBoarding(this, "explosionSpriteSheet", new Vector3(0,-40,0), Vector2.One, new Vector2(5, 5), 50f);
            Components.Add(billy);

            for (int i = 0; i < 0; i++)
            {
                s = new squid(this, "cone2",new Vector3(r.Next(-500,500), r.Next(0,800), r.Next(-500,500)), Vector3.One, 50, 50);
                Components.Add(s);
            }

            hud.DrawOrder = 2;
            crosshair.DrawOrder = 2;
            Components.Add(_navMesh);
            Components.Add(introduction);            
            Components.Add(_camera);
            Components.Add(sky);
            Components.Add(hud);
            Components.Add(crosshair);

            Components.Add(pistol);
            Components.Add(machine);
            Components.Add(knife);
            Components.Add(b1);
            Components.Add(c1);

            Components.Add(room);

            c1.PickUpWeapon(pistol);
            c1.PickUpWeapon(machine);
            c1.PickUpWeapon(knife);
        }

        
        protected override void Initialize()
        {
            this.IsMouseVisible = false;
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            terrainModel = Content.Load<Model>("terrain");
            heightmapObj = new HeightmapObject(this, terrainModel, Vector2.Zero);
            
            heightmapObj.PhysicsBody.Immovable = true;
            Components.Add(heightmapObj);
            font = Content.Load<SpriteFont>("SpriteFont1");
            spriteBatch = new SpriteBatch(GraphicsDevice);            
        }

        
        protected override void UnloadContent()
        {
                        
        }

        
        protected override void Update(GameTime gameTime)
        {
            keys = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keys.IsKeyDown(Keys.Escape))
                this.Exit();

            float timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            
            PhysicsSystem.CurrentPhysicsSystem.Integrate(timeStep);

            oldKeys = keys;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
