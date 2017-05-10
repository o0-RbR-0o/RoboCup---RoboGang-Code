using System;
using System.IO;

namespace RoboGang.RoboGang.Team
{
    public class TeamProperties
    {
        //Array with property structs of each player
        private TeamProperty[] _teamProperties = new TeamProperty[11];

        public TeamProperty[] Properties
        {
            get { return _teamProperties; }
            set { _teamProperties = value; }
        }


        //Convert a pixel coordinate to playfield coordinate
        public double PixelToField(double pixel, double fieldMax, int pixelMax)
        {
            var centeredPixel = pixel - pixelMax / 2.0;
            return centeredPixel * (fieldMax * 2.0 / pixelMax);
        }


        //Convert field coordinate to world coordinate
        public double FieldToPixel(double fieldpos, double fieldMax, int pixelMax)
        {
            var multiplicator = pixelMax / ((float)fieldMax * 2.0);
            var scaledPixel = fieldpos * multiplicator;
            return scaledPixel + pixelMax / 2.0;

        }


        //Write current config to a binary file
        public void WriteToBinaryFile(string filename)
        {
            try
            {
                var bw = new BinaryWriter(new FileStream(filename, FileMode.Create));
                foreach (var t in Properties)
                {
                    bw.Write(t.Personality ?? "Renegade");
                    bw.Write(t.StartpointX);
                    bw.Write(t.StartpointY);
                    bw.Write(t.Rotation);
                    bw.Write(t.IsGoalie);
                }
                bw.Close();
            }

            catch
            {
                // ignored
            }
        }


        //Write current config to a text file
        public void WriteToTextFile(string filename)
        {
            try
            {
                var sw = new StreamWriter(filename);
                for (var i = 0; i < Properties.Length; i++)
                {
                    sw.WriteLine("//---------------Player " + (i + 1) + "---------------");
                    if (Properties[i].Personality != null)
                        sw.WriteLine("Personality:" + Properties[i].Personality);
                    else
                        sw.WriteLine("Personality:Renegade");
                    sw.WriteLine("StartpointX:" + Properties[i].StartpointX);
                    sw.WriteLine("StartpointY:" + Properties[i].StartpointY);
                    sw.WriteLine("Rotation:" + Properties[i].Rotation);
                    sw.WriteLine("IsGoalie:" + Properties[i].IsGoalie);
                }
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }


        //Load a startup configuration from a binary file
        public void LoadFromBinaryFile(string filename)
        {
            try
            {
                var br = new BinaryReader(new FileStream(filename, FileMode.Open));
                for (var i = 0; i < Properties.Length; i++)
                {
                    Properties[i].Personality = br.ReadString();
                    Properties[i].StartpointX = br.ReadDouble();
                    Properties[i].StartpointY = br.ReadDouble();

                    Properties[i].Rotation = br.ReadDouble();
                    Properties[i].IsGoalie = br.ReadBoolean();
                }
                br.Close();
            }
            catch
            {
                // ignored
            }
        }


        //Load a startup configuration from a text file
        public void LoadFromTextFile(string filename)
        {
            try
            {
                var br = new StreamReader(filename);

                string currentline;
                var i = -1;
                while ((currentline = br.ReadLine()) != null)
                {
                    if (!currentline.StartsWith("//"))
                    {
                        if (currentline.StartsWith("Personality:"))
                            Properties[i].Personality = currentline.Split(':')[1];
                        if (currentline.StartsWith("StartpointX:"))
                            Properties[i].StartpointX = Convert.ToDouble(currentline.Split(':')[1]);
                        if (currentline.StartsWith("StartpointY:"))
                            Properties[i].StartpointY = Convert.ToDouble(currentline.Split(':')[1]);
                        if (currentline.StartsWith("Rotation:"))
                            Properties[i].Rotation = Convert.ToDouble(currentline.Split(':')[1]);
                        if (currentline.StartsWith("IsGoalie:"))
                            Properties[i].IsGoalie = Convert.ToBoolean(currentline.Split(':')[1]);
                    }
                    else
                    {
                        i++;
                    }
                }
                br.Close();
            }
            catch
            {
                // ignored
            }
        }
    }


    //Property struct: Holding all properties for one player
    public struct TeamProperty
    {
        //Start coordinate X-Axis in Field coordinates
        public double StartpointX { get; set; }

        //Start coordinate Y-Axis in Field coordinates
        public double StartpointY { get; set; }

        //Rotation of the player
        public double Rotation { get; set; }

        //Personality of the player
        public string Personality { get; set; }

        //Is the player a goalie?
        public bool IsGoalie { get; set; }
    }
}
