using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TimeCircuits
{
    public enum EmptyType
    {
        Hide,
        Off,
        On
    }

    public class Display : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D logo;

        private Texture2D background;
        private Texture2D speedo;
        private Texture2D tick;

        private Texture2D empty;
        private Texture2D emptyGlow;

        private bool[] monthVisible = new bool[3];
        private bool[] dayVisible = new bool[3];
        private bool[] yearVisible = new bool[3];
        private bool[] hourVisible = new bool[3];
        private bool[] minuteVisible = new bool[3];
        private bool[] ampmVisible = new bool[3];

        private Texture2D[] speedoNumbers = new Texture2D[10];

        private Texture2D[][] numbers;
        private Texture2D[][] months;
        private Texture2D[][] ampm;

        private DateTime[] dates = new DateTime[3];

        private float speedoScale = 0.7f;

        public bool IsHUDVisible { get; set; } = false;
        public bool IsTickVisible { get; set; } = false;
        public EmptyType Empty { get; set; } = EmptyType.Hide;
        public int Speed { get; set; } = 0;

        public Display()
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

            logo = Content.Load<Texture2D>("Backgrounds/BTTFV");

            background = Content.Load<Texture2D>("Backgrounds/tcd");
            tick = Content.Load<Texture2D>("Backgrounds/tcd_tick");

            empty = Content.Load<Texture2D>("Empty/empty_off");
            emptyGlow = Content.Load<Texture2D>("Empty/empty_glow");

            speedo = Content.Load<Texture2D>("Backgrounds/speedo");

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

            //_graphics.ToggleFullScreen();
            _graphics.PreferredBackBufferWidth = background.Width / 2;
            _graphics.PreferredBackBufferHeight = (int)((background.Height + speedo.Height * speedoScale) / 2);
            _graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private int RowNameToInt(string name)
        {
            name = name.ToLower();

            switch (name)
            {
                case "red":
                    return 0;
                case "green":
                    return 1;
                default:
                    return 2;
            }
        }

        public void SetDate(string type, DateTime date)
        {
            int row = RowNameToInt(type);

            dates[row] = date;

            monthVisible[row] = true;
            dayVisible[row] = true;
            yearVisible[row] = true;
            hourVisible[row] = true;
            minuteVisible[row] = true;
            ampmVisible[row] = true;
        }

        public void SetVisible(string type, bool toggle, bool month = true, bool day = true, bool year = true, bool hour = true, bool minute = true, bool amPm = true)
        {
            int row = RowNameToInt(type);

            if (toggle)
            {
                if (!month)
                    monthVisible[row] = false;

                if (!day)
                    dayVisible[row] = false;

                if (!year)
                    yearVisible[row] = false;

                if (!hour)
                    hourVisible[row] = false;

                if (!minute)
                    minuteVisible[row] = false;

                if (!amPm)
                    ampmVisible[row] = false;
            }
            else
            {
                if (month)
                    monthVisible[row] = false;

                if (day)
                    dayVisible[row] = false;

                if (year)
                    yearVisible[row] = false;

                if (hour)
                    hourVisible[row] = false;

                if (minute)
                    minuteVisible[row] = false;

                if (amPm)
                    ampmVisible[row] = false;
            }
        }

        public void SetOff()
        {
            IsHUDVisible = false;
            IsTickVisible = false;
            Empty = EmptyType.Hide;

            for (int row = 0; row < 3; row++)
                dates[row] = default;
        }

        private Matrix CurrentScale()
        {
            var gameWorldSize = new Vector2(background.Width, background.Height + speedo.Height * speedoScale);
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

            _spriteBatch.Draw(logo, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            if (IsHUDVisible)
            {                
                _spriteBatch.Draw(speedo, Vector2.Zero, null, Color.White, 0, Vector2.Zero, speedoScale, SpriteEffects.None, 1);

                if (Speed < 10)
                    _spriteBatch.Draw(speedoNumbers[Speed], GetSpeedoPos(1), null, Color.White, 0, Vector2.Zero, speedoScale, SpriteEffects.None, 1);
                else
                {
                    for (int num = 0; num < 2; num++)
                    {
                        int pos = (int)Char.GetNumericValue(Speed.ToString()[num]);
                        _spriteBatch.Draw(speedoNumbers[pos], GetSpeedoPos(num), null, Color.White, 0, Vector2.Zero, speedoScale, SpriteEffects.None, 1);
                    }
                }

                if (Empty != EmptyType.Hide)
                {
                    float emptyX = speedo.Width * speedoScale + (background.Width - speedo.Width * speedoScale) / 2 - (empty.Width * 1.5f) / 2;
                    float emptyY = (speedo.Height * speedoScale) / 2 - (empty.Height * 1.5f) / 2;

                    _spriteBatch.Draw(Empty == EmptyType.Off ? empty : emptyGlow, new Vector2(emptyX, emptyY), null, Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 1);
                }

                _spriteBatch.Draw(background, new Vector2(0, speedo.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                for (int row = 0; row < 3; row++)
                {
                    DateTime dateTime = dates[row];

                    if (dateTime == null || dateTime == default)
                        break;

                    if (monthVisible[row])
                        _spriteBatch.Draw(months[row][dateTime.Month - 1], GetMonthPos(row), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);

                    if (dayVisible[row])
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

                    if (yearVisible[row])
                    {
                        for (int num = 0; num < 4; num++)
                        {
                            int pos = (int)Char.GetNumericValue(dateTime.Year.ToString()[num]);
                            _spriteBatch.Draw(numbers[row][pos], GetYearPos(row, num), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                        }
                    }

                    if (ampmVisible[row])
                    {
                        if (dateTime.Hour > 12)
                            _spriteBatch.Draw(ampm[row][1], new Vector2(0, speedo.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                        else
                            _spriteBatch.Draw(ampm[row][0], new Vector2(0, speedo.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    }

                    string time = dateTime.ToString("hhmm");

                    if (hourVisible[row])
                    {
                        for (int num = 0; num < 2; num++)
                        {
                            int pos = (int)Char.GetNumericValue(time[num]);
                            _spriteBatch.Draw(numbers[row][pos], GetHourPos(row, num), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                        }
                    }

                    if (minuteVisible[row])
                    {
                        for (int num = 0; num < 2; num++)
                        {
                            int pos = (int)Char.GetNumericValue(time[num + 2]);
                            _spriteBatch.Draw(numbers[row][pos], GetMinutePos(row, num), null, Color.White, 0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
                        }
                    }

                }

                if (IsTickVisible)
                    _spriteBatch.Draw(tick, new Vector2(0, speedo.Height * speedoScale), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public Vector2 GetRowOffset(int row)
        {
            switch (row)
            {
                case 0:
                    return new Vector2(0, 82 + speedo.Height * speedoScale);
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
    }
}
