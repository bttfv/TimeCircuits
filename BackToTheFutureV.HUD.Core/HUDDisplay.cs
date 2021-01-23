using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BackToTheFutureV.HUD.Core
{    
    public class HUDDisplay : Game
    {
        private DateTime _lastUpdate;
        private HUDProperties _properties = new HUDProperties();
        public HUDProperties Properties
        {
            get => _properties;
            set
            {
                _properties = value;
                _lastUpdate = DateTime.Now;
            }
        }

        public GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch;

        private Texture2D MetalBackground;
        private Texture2D BTTFVLogo;

        private Texture2D TCD;
        private Texture2D Speedometer;
        private Texture2D TCDTick;

        private Texture2D EmptyLed;
        private Texture2D EmptyLedGlow;

        private Texture2D[] SpeedoNumber = new Texture2D[10];

        private Texture2D SID;

        private Texture2D SIDLedGreen;
        private Texture2D SIDLedRed;
        private Texture2D SIDLedYellow;

        private Texture2D[][] TCDNumber;
        private Texture2D[][] TCDMonth;
        private Texture2D[][] TCDAmPm;

        private const float SpeedometerScale = 0.7f;
        private const float TCDElementScale = 0.9f;
        private const float EmptyLedScale = 1.5f;
        private Vector2 EmptyPosition;

        private float SpeedometerY => SID.Height - TCD.Height - Speedometer.Height * SpeedometerScale;
        private float TCDY => SID.Height - TCD.Height;

        public HUDDisplay()
        {
            Graphics = new GraphicsDeviceManager(this);
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

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            MetalBackground = Content.Load<Texture2D>("Backgrounds/metal_background");
            BTTFVLogo = Content.Load<Texture2D>("Backgrounds/BTTFV");

            TCD = Content.Load<Texture2D>("Backgrounds/tcd");
            TCDTick = Content.Load<Texture2D>("Backgrounds/tcd_tick");

            EmptyLed = Content.Load<Texture2D>("Empty/empty_off");
            EmptyLedGlow = Content.Load<Texture2D>("Empty/empty_glow");

            Speedometer = Content.Load<Texture2D>("Backgrounds/speedo");

            for (int num = 0; num < 10; num++)
                SpeedoNumber[num] = Content.Load<Texture2D>($"Speedo/{num}");

            TCDNumber = new Texture2D[3][];
            TCDMonth = new Texture2D[3][];
            TCDAmPm = new Texture2D[3][];

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

                TCDNumber[row] = new Texture2D[10];
                TCDMonth[row] = new Texture2D[12];
                TCDAmPm[row] = new Texture2D[2];

                for (int num = 0; num < 10; num++)
                    TCDNumber[row][num] = Content.Load<Texture2D>($"TimeCircuits/{color}/{num}");

                for (int num = 1; num < 13; num++)
                    TCDMonth[row][num - 1] = Content.Load<Texture2D>($"TimeCircuits/{color}/Months/{num}");

                TCDAmPm[row][0] = Content.Load<Texture2D>($"TimeCircuits/{color}/am");
                TCDAmPm[row][1] = Content.Load<Texture2D>($"TimeCircuits/{color}/pm");
            }

            SID = Content.Load<Texture2D>("Backgrounds/sid_background");
            SIDLedGreen = Content.Load<Texture2D>("SID/sid_led_green");
            SIDLedRed = Content.Load<Texture2D>("SID/sid_led_red");
            SIDLedYellow = Content.Load<Texture2D>("SID/sid_led_yellow");

            float emptyX = Speedometer.Width * SpeedometerScale + (TCD.Width - Speedometer.Width * SpeedometerScale - EmptyLed.Width * EmptyLedScale) / 2;
            float emptyY = SpeedometerY + (Speedometer.Height * SpeedometerScale - EmptyLed.Height * EmptyLedScale) / 2;

            EmptyPosition = new Vector2(emptyX, emptyY);

            Graphics.PreferredBackBufferWidth = (TCD.Width + SID.Width) / 2;
            Graphics.PreferredBackBufferHeight = SID.Height / 2;
            Graphics.ApplyChanges();
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
            var gameWorldSize = new Vector2(TCD.Width + SID.Width, SID.Height);
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

#if RELEASE
            if (_lastUpdate < DateTime.Now.AddSeconds(-3))
            {
                Properties = new HUDProperties();
                _lastUpdate = DateTime.Now.AddHours(1);
            }
#endif

            GraphicsDevice.Clear(Color.Transparent);

            // TODO: Add your drawing code here
            SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, CurrentScale());            

            if (Properties.IsHUDVisible)
            {
                SpriteBatch.Draw(MetalBackground, Vector2.Zero, Color.White);

                SpriteBatch.Draw(Speedometer, new Vector2(0, SpeedometerY), null, Color.White, 0, Vector2.Zero, SpeedometerScale, SpriteEffects.None, 1);

                if (Properties.Speed < 10)
                    SpriteBatch.Draw(SpeedoNumber[Properties.Speed], GetSpeedoPos(1), null, Color.White, 0, Vector2.Zero, SpeedometerScale, SpriteEffects.None, 1);
                else
                {
                    for (int num = 0; num < 2; num++)
                    {
                        int pos = (int)Char.GetNumericValue(Properties.Speed.ToString()[num]);
                        SpriteBatch.Draw(SpeedoNumber[pos], GetSpeedoPos(num), null, Color.White, 0, Vector2.Zero, SpeedometerScale, SpriteEffects.None, 1);
                    }
                }

                if (Properties.Empty != EmptyType.Hide)
                    SpriteBatch.Draw(Properties.Empty == EmptyType.Off ? EmptyLed : EmptyLedGlow, EmptyPosition, null, Color.White, 0, Vector2.Zero, EmptyLedScale, SpriteEffects.None, 1);

                SpriteBatch.Draw(TCD, new Vector2(0, TCDY), Color.White);

                for (int row = 0; row < 3; row++)
                {
                    DateTime dateTime = Properties.Date[row];

                    if (dateTime == null || dateTime == default)
                        break;

                    if (Properties.MonthVisible[row])
                        SpriteBatch.Draw(TCDMonth[row][dateTime.Month - 1], GetMonthPos(row), null, Color.White, 0, Vector2.Zero, TCDElementScale, SpriteEffects.None, 1);

                    if (Properties.DayVisible[row])
                    {
                        if (dateTime.Day < 10)
                        {
                            SpriteBatch.Draw(TCDNumber[row][0], GetDayPos(row, 0), null, Color.White, 0, Vector2.Zero, TCDElementScale, SpriteEffects.None, 1);
                            SpriteBatch.Draw(TCDNumber[row][dateTime.Day], GetDayPos(row, 1), null, Color.White, 0, Vector2.Zero, TCDElementScale, SpriteEffects.None, 1);
                        }
                        else
                        {
                            for (int num = 0; num < 2; num++)
                            {
                                int pos = (int)Char.GetNumericValue(dateTime.Day.ToString()[num]);
                                SpriteBatch.Draw(TCDNumber[row][pos], GetDayPos(row, num), null, Color.White, 0, Vector2.Zero, TCDElementScale, SpriteEffects.None, 1);
                            }
                        }
                    }

                    if (Properties.YearVisible[row])
                    {
                        for (int num = 0; num < 4; num++)
                        {
                            int pos = (int)Char.GetNumericValue(dateTime.Year.ToString()[num]);
                            SpriteBatch.Draw(TCDNumber[row][pos], GetYearPos(row, num), null, Color.White, 0, Vector2.Zero, TCDElementScale, SpriteEffects.None, 1);
                        }
                    }

                    if (Properties.AmPmVisible[row])
                    {
                        if (dateTime.Hour > 12)
                            SpriteBatch.Draw(TCDAmPm[row][1], new Vector2(0, TCDY), Color.White);
                        else
                            SpriteBatch.Draw(TCDAmPm[row][0], new Vector2(0, TCDY), Color.White);
                    }

                    string time = dateTime.ToString("hhmm");

                    if (Properties.HourVisible[row])
                    {
                        for (int num = 0; num < 2; num++)
                        {
                            int pos = (int)Char.GetNumericValue(time[num]);
                            SpriteBatch.Draw(TCDNumber[row][pos], GetHourPos(row, num), null, Color.White, 0, Vector2.Zero, TCDElementScale, SpriteEffects.None, 1);
                        }
                    }

                    if (Properties.MinuteVisible[row])
                    {
                        for (int num = 0; num < 2; num++)
                        {
                            int pos = (int)Char.GetNumericValue(time[num + 2]);
                            SpriteBatch.Draw(TCDNumber[row][pos], GetMinutePos(row, num), null, Color.White, 0, Vector2.Zero, TCDElementScale, SpriteEffects.None, 1);
                        }
                    }

                }

                if (Properties.IsTickVisible)
                    SpriteBatch.Draw(TCDTick, new Vector2(0, TCDY), Color.White);
                
                SpriteBatch.Draw(SID, new Vector2(TCD.Width, 0), Color.White);

                for (int column = 0; column < 10; column++)
                    for(int row = 0; row < 20; row++)
                        if (Properties.LedState[column][row])
                            SpriteBatch.Draw(GetLedColor(row), GetLedOffset(column, row), Color.White);
            } 
            else
                SpriteBatch.Draw(BTTFVLogo, new Vector2((SID.Width + TCD.Width - BTTFVLogo.Width) / 2, (SID.Height - BTTFVLogo.Height) / 2), Color.White);

            SpriteBatch.End();

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
                    return new Vector2(0, 85 + TCDY);
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
                    return new Vector2(199 * SpeedometerScale, 41 * SpeedometerScale + SpeedometerY);
                default:
                    return new Vector2(392 * SpeedometerScale, 41 * SpeedometerScale + SpeedometerY);
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
