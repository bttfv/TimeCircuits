using System;
using System.Collections.Generic;
using System.Text;

namespace BackToTheFutureV.HUD.Core
{
    [Serializable]
    public class HUDProperties
    {
        public bool[][] LedState;

        public int[] CurrentHeight = new int[10];
        public int[] NewHeight = new int[10];
        public int[] LedDelay = new int[10];

        public DateTime[] Date = new DateTime[3];

        public bool IsHUDVisible { get; set; } = false;
        public bool IsTickVisible { get; set; } = false;
        public EmptyType Empty { get; set; } = EmptyType.Hide;
        public int Speed { get; set; } = 0;

        public bool[] MonthVisible { get; set; } = new bool[3];
        public bool[] DayVisible { get; set; } = new bool[3];
        public bool[] YearVisible { get; set; } = new bool[3];
        public bool[] HourVisible { get; set; } = new bool[3];
        public bool[] MinuteVisible { get; set; } = new bool[3];
        public bool[] AmPmVisible { get; set; } = new bool[3];

        public HUDProperties()
        {
            LedState = new bool[10][];

            for (int column = 0; column < 10; column++)
                LedState[column] = new bool[20];

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

            Date[row] = date;

            MonthVisible[row] = true;
            DayVisible[row] = true;
            YearVisible[row] = true;
            HourVisible[row] = true;
            MinuteVisible[row] = true;
            AmPmVisible[row] = true;
        }

        public void SetVisible(string type, bool toggle, bool month = true, bool day = true, bool year = true, bool hour = true, bool minute = true, bool amPm = true)
        {
            int row = RowNameToInt(type);

            if (toggle)
            {
                if (!month)
                    MonthVisible[row] = false;

                if (!day)
                    DayVisible[row] = false;

                if (!year)
                    YearVisible[row] = false;

                if (!hour)
                    HourVisible[row] = false;

                if (!minute)
                    MinuteVisible[row] = false;

                if (!amPm)
                    AmPmVisible[row] = false;
            }
            else
            {
                if (month)
                    MonthVisible[row] = false;

                if (day)
                    DayVisible[row] = false;

                if (year)
                    YearVisible[row] = false;

                if (hour)
                    HourVisible[row] = false;

                if (minute)
                    MinuteVisible[row] = false;

                if (amPm)
                    AmPmVisible[row] = false;
            }
        }

        public void SetOff()
        {
            IsHUDVisible = false;
            IsTickVisible = false;
            Empty = EmptyType.Hide;

            for (int row = 0; row < 3; row++)
                Date[row] = default;

            for (int column = 0; column < 10; column++)
                for (int row = 0; row < 20; row++)
                    LedState[column][row] = false;
        }

        public void SetLedState(bool[][] _ledState)
        {
            LedState = _ledState;
        }
    }
}
