using System;
using System.IO;

/* RoboGang Team Configurator - A small editor for visual RoboCup2D startup configuration - Made for use with the RoboGang project which is based on Crapi*/

namespace RoboGangTeamConfigurator
{
    public class TeamProperties
    {
        //Array with property structs of each player
        TeamProperty[] team_properties = new TeamProperty[11];
        public TeamProperty[] Properties
        {
            get { return team_properties; }
            set { team_properties = value; }
        }

        //Convert a pixel coordinate to playfield coordinate
        public double pixelToField(double pixel, double field_max, int pixel_max)
        {
            double centered_pixel = pixel - ((float)pixel_max / 2.0);
            return (centered_pixel * ((double)(field_max * 2.0) / (double)pixel_max));
        }

        //Convert field coordinate to world coordinate
        public double fieldToPixel(double fieldpos, double field_max, int pixel_max)
        {
          double multiplicator= (double)pixel_max / (double)((float)field_max * 2.0);
          double scaled_pixel=(fieldpos*multiplicator);
          return scaled_pixel+(pixel_max/2.0);
         
        }
        
        //Write current config to a binary file
        public void writeToBinaryFile(string filename)
        {
            try
            {
                BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create));
                for (int i = 0; i < Properties.Length; i++)
                {
                    if (Properties[i].Personality != null)
                        bw.Write(Properties[i].Personality);
                    else
                        bw.Write("Renegade");
                    bw.Write(Properties[i].Startpoint_x);
                    bw.Write(Properties[i].Startpoint_y);
                    bw.Write(Properties[i].Rotation);
                    bw.Write(Properties[i].IsGoalie);
                }
                bw.Close();
            }
            catch
            { }
        }


        //Write current config to a text file
        public void writeToTextFile(string filename)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filename);
                for (int i = 0; i < Properties.Length; i++)
                {
                    sw.WriteLine("//---------------Player " + (i + 1) + "---------------");
                    if (Properties[i].Personality != null)
                        sw.WriteLine("Personality:" + Properties[i].Personality);
                    else
                        sw.WriteLine("Personality:Renegade");
                    sw.WriteLine("StartpointX:" + Properties[i].Startpoint_x);
                    sw.WriteLine("StartpointY:" + Properties[i].Startpoint_y);
                    sw.WriteLine("Rotation:" + Properties[i].Rotation);
                    sw.WriteLine("IsGoalie:" + Properties[i].IsGoalie);
                }
                sw.Close();
            }
            catch { }
        }


        //Load a startup configuration from a binary file
        public void loadFromBinaryFile(string filename)
        {
            try
            {
                BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));
                for (int i = 0; i < Properties.Length; i++)
                {
                    Properties[i].Personality = br.ReadString();
                    Properties[i].Startpoint_x = br.ReadDouble();
                    Properties[i].Startpoint_y = br.ReadDouble();

                    Properties[i].Rotation = br.ReadDouble();
                    Properties[i].IsGoalie = br.ReadBoolean();
                }
                br.Close();
            }
            catch
            { }
        }


        //Load a startup configuration from a text file
        public void loadFromTextFile(string filename)
        {
            try
            {
                StreamReader br = new StreamReader(filename);

                string currentline = "";
                int i = -1;
                while ((currentline = br.ReadLine()) != null)
                {
                    if (!currentline.StartsWith("//"))
                    {
                        if (currentline.StartsWith("Personality:"))
                            Properties[i].Personality = currentline.Split(':')[1];
                        if (currentline.StartsWith("StartpointX:"))
                            Properties[i].Startpoint_x = Convert.ToDouble(currentline.Split(':')[1]);
                        if (currentline.StartsWith("StartpointY:"))
                            Properties[i].Startpoint_y = Convert.ToDouble(currentline.Split(':')[1]);
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
            catch { }
        }
    }


    //Property struct: Holding all properties for one player
    public struct TeamProperty
    {
        //Start coordinate X-Axis in Field coordinates
        double startpoint_x;
        public double Startpoint_x
        {
            get { return startpoint_x; }
            set { startpoint_x = value; }
        }

        //Start coordinate Y-Axis in Field coordinates
        double startpoint_y;
        public double Startpoint_y
        {
            get { return startpoint_y; }
            set { startpoint_y = value; }
        }

        //Rotation of the player
        double rotation;
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        //Personality of the player
        string personality;
        public string Personality
        {
            get { return personality; }
            set { personality = value; }
        }

        //Is the player a goalie?
        bool isGoalie;
        public bool IsGoalie
        {
            get { return isGoalie; }
            set { isGoalie = value; }
        }
    }
}
