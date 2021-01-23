using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BackToTheFutureV.HUD.Core
{    
    public class HUDDisplay : Game
    {
        public HUDProperties Properties = new HUDProperties();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D BTTFVLogo;

        private Texture2D TCD;
        private Texture2D Speedometer;
        private Texture2D TCDTick;

        private Texture2D EmptyLed;
        private Texture2D EmptyLedGlow;

        private Texture2D[] speedoNumbers = new Texture2D[10];

        private Texture2D SID;

        private Texture2D SIDLedGreen;
        private Texture2D SIDLedRed;
        private Texture2D SIDLedYellow;

        private Texture2D[][] numbers;
        private Texture2D[][] months;
        private Texture2D[][] ampm;

        private float speedoScale = 0.7f;

        public HUDDisplay()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.Title = "Back To The Future V: Heads-Up Display";
            Window.AllowUserResizing = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            BTTFVLogo = Content.Load<Texture2D>("Backgrounds/BTTFV");

            TCD = Content.Load<Texture2D>("Backgrounds/tcd");
            TCDTick = Content.Load<Texture2D>("Backgrounds/tcd_tick");

            EmptyLed = Content.Load<Texture2D>("Empty/empty_off");
            EmptyLedGlow = Content.Load<Texture2D>("Empty/empty_glow");

            Speedometer = Content.Load<Texture2D>("Backgrounds/speedo");

            for (int num = 0; num < 10; num++)
                speedoNumbers[num] = Content.Load<Texture2D>($"Speedo/{num}");

            numbers = new Texture2D[3][];
            months = new Texture2D[3][];
            ampm = new Texture2D[3][];

            for (int row = 0; row < 3; row++)
            {
                string color;

                switch (row)
                {
                    case 0:
                        color = "Red";
                        break;
                    case 1:
                        color = "Green";
                        break;
                    default:
                        color = "Yellow";
                        break;
                }

                numbers[row] = new Texture2D[10];
                months[row] = new Texture2D[12];
                ampm[row] = new Texture2D[2];

                for (int num = 0; num < 10; num++)
                    numbers[row][num] = Content.Load<Texture2D>($"TimeCircuits/{color}/{num}");

                for (int num = 1; num < 13; num++)
                    months[row][num - 1] = Content.Load<Texture2D>($"TimeCircuits/{color}/Months/{num}");

                ampm[row][0] = Content.Load<Texture2D>($"TimeCircuits/{color}/am");
                ampm[row][1] = Content.Load<Texture2D>($"TimeCircuits/{color}/pm");
            }

            SID = Content.Load<Texture2D>("Backgrounds/sid_background");
            SIDLedGreen = Content.Load<Texture2D>("SID/sid_led_green");
            SIDLedRed = Content.Load<Texture2D>("SID/sid_led_red");
            SIDLedYellow = Content.Load<Texture2D>("SID/sid_led_yellow");

            //_graphics.ToggleFullScreen();
            _graphics.PreferredBackBufferWidth = (TCD.Width + SID.Width) / 2;
            _graphics.PreferredBackBufferHeight = (int)((TCD.Height + Speedometer.Height * speedoScale) / 2);
            _graphics.ApplyChanges();
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Environment.Exit(0);

            base.Update(gameTime);
        }

        private Matrix CurrentScale()
        {
            var gameWorldSize = new Vector2(TCD.Width + SID.Width, TCD.Height + Speedometer.Height * speedoScale);
            var vp = GraphicsDevice.Viewport;

            float scaleX = vp.Width / gameWorldSize.X;
            float scaleY = vp.Height / gameWorldSize.Y;
            float scale = Math.Min(scaleX, scaleY);

            float translateX = (vp.Width - (gameWorldSize.X * scale)) / 2f;
            float translateY = (vp.Height - (gameWorldSize.Y * scale)) / 2f;

            return Matrix.CreateScale(scale, scale, 1) * Matrix.CreateTranslation(translateX, translateY, 0);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, CurrentScale());            

            if (Properties.IsHUDVisible)
            {                
                _spriteBatch.Draw(Speedometer, Vector2.Zero, null, Color.White, 0, Vector2.Zero, speedoScale, SpriteEffects.None, 1);

                if (Properties.Speed < 10)
                    _spriteBatch.Draw(speedoNumbers[Properties.Speed], GetSpeedoPos(1), null, Color.White, 0, Vector2.Zero, speedoScale, SpriteEffects.None, 1);
                else
                {
                    for (int num = 0; num < 2; num++)
                    {
                        int pos = (int)Char.GetNumericValue(Properties.Speed.ToString()[num]);
                        _spriteBatch.Draw(speedoNumbers[pos], GetSpeedoPos(num), null, Color.White, 0, Vector2.Zero, speedoScale, SpriteEffects.None, 1);
                    }
                }

                if (Properties.Empty != EmptyType.Hide)
                {
                    float emptyX = Speedometer.Width * speedoScale + (TCD.Width - Speedometer.Width * speedoScale) / 2 - (EmptyLed.Width * 1.5f) / 2;
                    float emptyY = (Speedometer.Height * speedoScale) / 2 - (EmptyLed.Height * 1.5f) / 2;

                    _spriteBatch.Draw(Properties.Empty == EmptyType.Off ? EmptyLed : EmptyLedGlow, new Vector2(emptyX, emptyY), null, Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 1);
                }

                _spriteBatch.Draw(TCD, new Vector2(0, Speedometer.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                for (int row = 0; row < 3; row++)
                {
                    DateTime dateTime = Properties.Date[row];

                    if (dateTime == null || dateTime == default)
                        break;

                    if (Properties.MonthVisible[row])
                        _spriteBatch.Draw(months[row][dateTime.Month - 1], GetMonthPos(row), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);

                    if (Properties.DayVisible[row])
                    {
                        if (dateTime.Day < 10)
                        {
                            _spriteBatch.Draw(numbers[row][0], GetDayPos(row, 0), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                            _spriteBatch.Draw(numbers[row][dateTime.Day], GetDayPos(row, 1), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                        }
                        else
                        {
                            for (int num = 0; num < 2; num++)
                            {
                                int pos = (int)Char.GetNumericValue(dateTime.Day.ToString()[num]);
                                _spriteBatch.Draw(numbers[row][pos], GetDayPos(row, num), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                            }
                        }
                    }

                    if (Properties.YearVisible[row])
                    {
                        for (int num = 0; num < 4; num++)
                        {
                            int pos = (int)Char.GetNumericValue(dateTime.Year.ToString()[num]);
                            _spriteBatch.Draw(numbers[row][pos], GetYearPos(row, num), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                        }
                    }

                    if (Properties.AmPmVisible[row])
                    {
                        if (dateTime.Hour > 12)
                            _spriteBatch.Draw(ampm[row][1], new Vector2(0, Speedometer.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                        else
                            _spriteBatch.Draw(ampm[row][0], new Vector2(0, Speedometer.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    }

                    string time = dateTime.ToString("hhmm");

                    if (Properties.HourVisible[row])
                    {
                        for (int num = 0; num < 2; num++)
                        {
                            int pos = (int)Char.GetNumericValue(time[num]);
                            _spriteBatch.Draw(numbers[row][pos], GetHourPos(row, num), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                        }
                    }

                    if (Properties.MinuteVisible[row])
                    {
                        for (int num = 0; num < 2; num++)
                        {
                            int pos = (int)Char.GetNumericValue(time[num + 2]);
                            _spriteBatch.Draw(numbers[row][pos], GetMinutePos(row, num), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                        }
                    }

                }

                if (Properties.IsTickVisible)
                    _spriteBatch.Draw(TCDTick, new Vector2(0, Speedometer.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                _spriteBatch.Draw(SID, new Vector2(TCD.Width, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                for (int column = 0; column < 10; column++)
                    for(int row = 0; row < 20; row++)
                        if (Properties.LedState[column][row])
                            _spriteBatch.Draw(GetLedColor(row), GetLedOffset(column, row), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            } 
            else
                _spriteBatch.Draw(BTTFVLogo, new Vector2((SID.Width + TCD.Width - BTTFVLogo.Width) / 2, (TCD.Height + Speedometer.Height * speedoScale - BTTFVLogo.Height) / 2), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

#region "Position routines"

        public Vector2 GetLedOffset(int column, int row)
        {
            Vector2 origin = new Vector2(TCD.Width + 152, 972);

            if (column == 0 && row == 0)
                return origin;

            return origin + new Vector2(column * 49.6f , - row * 49.3f);
        }

        public Texture2D GetLedColor(int row)
        {
            if (row < 13)
                return SIDLedGreen;
            else if (row < 19)
                return SIDLedYellow;

            return SIDLedRed;
        }

        public Vector2 GetRowOffset(int row)
        {
            switch (row)
            {
                case 0:
                    return new Vector2(0, 82 + Speedometer.Height * speedoScale);
                case 1:
                    return new Vector2(0, 291) + GetRowOffset(0);
                default:
                    return new Vector2(0, 585) + GetRowOffset(0);
            }
        }

        private Vector2 GetSpeedoPos(int num)
        {
            switch (num)
            {
                case 0:
                    return new Vector2(199 * speedoScale, 41 * speedoScale);
                default:
                    return new Vector2(392 * speedoScale, 41 * speedoScale);
            }
        }

        private Vector2 GetMonthPos(int row)
        {
            return new Vector2(76, 0) + GetRowOffset(row);
        }

        private Vector2 GetDayPos(int row, int num)
        {
            switch (num)
            {
                case 0:
                    return new Vector2(377, 2) + GetRowOffset(row);
                default:
                    return new Vector2(78, 0) + GetDayPos(row, num - 1);
            }
        }

        private Vector2 GetYearPos(int row, int num)
        {
            switch (num)
            {
                case 0:
                    return new Vector2(612, 2) + GetRowOffset(row);
                default:
                    return new Vector2(78, 0) + GetYearPos(row, num - 1);
            }
        }

        private Vector2 GetHourPos(int row, int num)
        {
            switch (num)
            {
                case 0:
                    return new Vector2(1040, 2) + GetRowOffset(row);
                default:
                    return new Vector2(78, 0) + GetHourPos(row, num - 1);
            }
        }

        private Vector2 GetMinutePos(int row, int num)
        {
            switch (num)
            {
                case 0:
                    return new Vector2(1277, 2) + GetRowOffset(row);
                default:
                    return new Vector2(78, 0) + GetMinutePos(row, num - 1);
            }
        }
#endregion
    }
}
