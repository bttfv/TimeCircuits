using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BackToTheFutureV.HUD.Core
{
    public enum EmptyType
    {
        Hide,
        Off,
        On
    }

    [Serializable]
    public class HUDProperties
    {
        private static BinaryFormatter formatter = new BinaryFormatter();

        public bool[][] LedState;

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

        public static HUDProperties FromData(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                try
                {
                    return (HUDProperties)formatter.Deserialize(stream);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static implicit operator byte[](HUDProperties command)
        {
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, command);
                return stream.ToArray();
            }
        }
    }
}
