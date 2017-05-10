using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


/* RoboGang Team Configurator - A small editor for visual RoboCup2D startup configuration - Made for use with the RoboGang project which is based on Crapi*/


namespace RoboGangTeamConfigurator
{
    public class Main : Game
    {
        //Our Graphics device manager, holding the graphics device to render on
        GraphicsDeviceManager graphics;

        //The editor toolbox; a WinForm.
        Properties pwindow = new Properties();

        //MouseStates: Current state of the mouse and the state of the last cycle
        MouseState mouse, mouselast;

        //A SpriteBatch to render the graphics with
        SpriteBatch spriteBatch;
        //Textures of the playfield (background), player and goalie gfx
        Texture2D playfield, player, goalie;

        //The font for rendering information in the graphics window
        SpriteFont font;
        
        //Max values of the playfield
        Vector2 fieldDimension = new Vector2(54, 35);

        //Scale multiplicator of the editor window (Set via command line parameter, if any. Default is 2)
        private int scale = 2;
        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        //Array to hold the positions of the players
        Vector2[] playerpositons = new Vector2[11];

        //We might need a random to initialize an empty configuration
        Random rnd;

        //Self explaining...
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Initialize everything we need befor the first render cycle
        protected override void Initialize()
        {
            //Set the size of the backbuffer to render on
            graphics.PreferredBackBufferWidth = (int)fieldDimension.X * 2*scale;
            graphics.PreferredBackBufferHeight = (int)fieldDimension.Y * 2 * scale;

            //We need the mouse for positioning the players
            IsMouseVisible = true;

            //And we need a new random. Default seed is good enough here.
            rnd = new Random();

            //Get the mouse state(s) for the first time
            mouse = Mouse.GetState();
            mouselast = Mouse.GetState();

            //Set the window handle of the mouse. This is needed to have the offset set correctly so that the mouse position is equal to render target position
            Mouse.WindowHandle = this.Window.Handle;

            //And at last, show the toolbox window
            pwindow.Show();

            //Now run the base class initialization (XNA/Monogame internal)
            base.Initialize();
        }

        //Load all the content we need.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all the textures
            playfield = Content.Load<Texture2D>("field");
            player = Content.Load<Texture2D>("player");
            goalie = Content.Load<Texture2D>("goalie");

            //Load the font
            font = Content.Load<SpriteFont>("Font");

            //Set the backbuffer to the playfield texture size (and multiply with scale)
            graphics.PreferredBackBufferWidth = (int)playfield.Width * scale;
            graphics.PreferredBackBufferHeight = (int)playfield.Height * scale;

            //Initialize random player positions
            for (int i = 0; i < playerpositons.Length; i++)
            {
                playerpositons[i] = new Vector2(rnd.Next(0, graphics.PreferredBackBufferWidth / 2), rnd.Next(graphics.PreferredBackBufferHeight));
            }

            //Apply the changes made to the graphics device
            graphics.ApplyChanges();

            //This is really important: We initialize a WinForm as toolbox. We need a handler for the closing event of the rendering window to be able to close it properly on Form close
            System.Windows.Forms.Form f = System.Windows.Forms.Form.FromHandle(Window.Handle) as System.Windows.Forms.Form;
            if (f != null)
            {
                f.FormClosing += f_FormClosing;
            } 
        }

        //On unload (called on exit), set the form closed property of the toolbox. Everything else will be handled by the garbage collector on exit, since we use the content manager
        protected override void UnloadContent()
        {
            pwindow.Closed1 = true;   
        }

        //Handler for the closing event: Close the toolbox form when the rendering form shoots the closing event
        void f_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            pwindow.Hide();
            pwindow.Closed1 = true;
        }  

        //Update method: Does the logic, called every cycle
        protected override void Update(GameTime gameTime)
        {
            //If we close the toolbox form, we exit the program.
            if (pwindow.Closed1)
                this.Exit();

            //Get the mouse state every cycle
            mouse = Mouse.GetState();

            //If we press the excape button, we exit the program
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //If we set the move option in the toolbox, we move the selected player to the mouse position until Enter or left mouse button was pressed
            if (pwindow.Mode == Option.Move) {
                playerpositons[pwindow.SelectedPlayer-1] = this.mouse.Position.ToVector2();
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) || mouse.LeftButton == ButtonState.Pressed)
                    pwindow.Mode = Option.None;
            }

            //If we loaded a config file, we apply all the positions we have loaded to the players on the field - This means we have to calculate the field positions to pixel positions
            if (pwindow.Mode == Option.Loaded) {
                for (int i = 0; i < playerpositons.Length; i++) {
                    playerpositons[i].X = (float)pwindow.TeamProperties.fieldToPixel(pwindow.TeamProperties.Properties[i].Startpoint_x, this.fieldDimension.X, this.graphics.PreferredBackBufferWidth);
                    playerpositons[i].Y = (float)pwindow.TeamProperties.fieldToPixel(pwindow.TeamProperties.Properties[i].Startpoint_y,this.fieldDimension.Y, this.graphics.PreferredBackBufferHeight);
                }
                //And finally, we reset the loaded option not to do that again in the next cycle
                pwindow.Mode = Option.None;
            }

            //Remember the last mouse state
            mouselast = mouse;

            //And run the monogame update logic
            base.Update(gameTime);
        }

        //Drawing method: called every cycle after the update method. Now it's time to render \o/
        protected override void Draw(GameTime gameTime)
        {
            //Clear the backbuffer
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Begin drawing
            spriteBatch.Begin();

            //Draw the playfield
            spriteBatch.Draw(playfield, Vector2.Zero, null,Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            //For each stored player position (11, usually), draw the player, either as default or goalie and also apply the scale. Then draw the properties string under the player
            for (int i = 0; i < playerpositons.Length; i++) {
                if (playerpositons[i] != null) {
                    if(pwindow.TeamProperties.Properties[i].IsGoalie)
                        spriteBatch.Draw(goalie, playerpositons[i], null, Color.White, 0f, new Vector2(player.Width/2,player.Height/2), scale, SpriteEffects.None, 0f);
                    else
                        spriteBatch.Draw(player, playerpositons[i], null, Color.White, 0f, new Vector2(player.Width / 2, player.Height / 2), scale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font,(i+1).ToString(), playerpositons[i] + new Vector2(scale*-font.MeasureString((i+1).ToString()).X/2, 6*scale), Color.Red,0.0f,Vector2.Zero,scale,SpriteEffects.None,0f);

                    //We update the position in the toolbox every cycle to be up to date if the player gets moved by the mouse
                    pwindow.TeamProperties.Properties[i].Startpoint_x = pwindow.TeamProperties.pixelToField(playerpositons[i].X, this.fieldDimension.X, this.graphics.PreferredBackBufferWidth);
                    pwindow.TeamProperties.Properties[i].Startpoint_y = pwindow.TeamProperties.pixelToField(playerpositons[i].Y, this.fieldDimension.Y, this.graphics.PreferredBackBufferHeight);
                }
            }

            //End the drawing
            spriteBatch.End();
            
            //And finally call the monogame internal drawing method
            base.Draw(gameTime);
        }
    }
}
