using System;
using System.Windows.Forms;

/* RoboGang Team Configurator - A small editor for visual RoboCup2D startup configuration - Made for use with the RoboGang project which is based on Crapi*/

namespace RoboGangTeamConfigurator
{
    //All the options which are possible with this toolbox as enum
    public enum Option { None, Move, Rotate, Makegoalie, MakePlayer, Load, Loaded}

    //The toolbox
    public partial class Properties : Form
    {

        //Current chosen option
        private Option mode = Option.None;
        public Option Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        //Current team properties instance to hold and save/load all the data
        private TeamProperties teamProperties = new TeamProperties();
        public TeamProperties TeamProperties
        {
            get { return teamProperties; }
            set { teamProperties = value; }
        }

        //Current selected player
        private int selectedPlayer = 1;
        public int SelectedPlayer
        {
            get { return selectedPlayer; }
            set { selectedPlayer = value; }
        }

        //Has this window been closed or do we want to close it?
        private bool closed = false;
        public bool Closed1
        {
            get { return closed; }
            set { closed = value; }
        }

        //Initialize the form components (WinForm default)
        public Properties()
        {
            InitializeComponent();
        }

        //On load of the form, set the selected player index in the comboBox to 0
        private void Properties_Load(object sender, EventArgs e)
        {
            comboBoxPlayers.SelectedIndex = 0;
        }

        //If the selected player index changed, we update the position boxes and also update the selected player value and the personality box to have the values of the selected player in there
        private void comboBoxPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.maskedTextBoxPositionX.Text = teamProperties.Properties[comboBoxPlayers.SelectedIndex].Startpoint_x.ToString();
            this.maskedTextBoxPositionY.Text = teamProperties.Properties[comboBoxPlayers.SelectedIndex].Startpoint_y.ToString();
            this.selectedPlayer = comboBoxPlayers.SelectedIndex+1;
            comboBox1.Text = teamProperties.Properties[comboBoxPlayers.SelectedIndex].Personality;
        }

        //If we click this button, we want to move the selected player
        private void button1_Click(object sender, EventArgs e)
        {
            mode = Option.Move;
        }

        //The timer event to update the position box to be up to date in case we move a player with the mouse
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.closed)
            {
                Application.Exit();
            }
            this.maskedTextBoxPositionX.Text = teamProperties.Properties[comboBoxPlayers.SelectedIndex].Startpoint_x.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
            this.maskedTextBoxPositionY.Text = teamProperties.Properties[comboBoxPlayers.SelectedIndex].Startpoint_y.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
        }

        //Make the selected player a goalie with this button
        private void button4_Click(object sender, EventArgs e)
        {
            this.teamProperties.Properties[comboBoxPlayers.SelectedIndex].IsGoalie = true;
        }

        //Make the selected player a regular player with this button
        private void button3_Click(object sender, EventArgs e)
        {
            this.teamProperties.Properties[comboBoxPlayers.SelectedIndex].IsGoalie = false;
        }

        //Save as a binary file menu item: Open the save dialog and write the team properties to the resulting file as binary format
        private void binaryFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RoboGang Config File|*.rgc";
            if(sfd.ShowDialog()==DialogResult.OK && sfd.FileName!=null)
                teamProperties.writeToBinaryFile(sfd.FileName);  
        }

        //Save as a text file menu item: Open the save dialog and write the team properties to the resulting file as text format
        private void textFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Textfile|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName != null)
                teamProperties.writeToTextFile(sfd.FileName);
        }

        //Load from a binary file menu item: Open the load dialog and load the team properties from the resulting file as binary format. Then update all the form items with the values loaded
        private void binaryFormatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RoboGang Config File|*.rgc";
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != null)
            {
                this.mode = Option.Load;
                this.teamProperties.loadFromBinaryFile(ofd.FileName);
                comboBoxPlayers.SelectedIndex = 0;
                comboBox1.Text = teamProperties.Properties[0].Personality;
                maskedTextBoxPositionX.Text = teamProperties.Properties[0].Startpoint_x.ToString();
                maskedTextBoxPositionY.Text = teamProperties.Properties[0].Startpoint_y.ToString();

                this.mode = Option.Loaded;
            }
        }

        //If we change the personality text, we update the text in the properties to be always up to date there
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            teamProperties.Properties[comboBoxPlayers.SelectedIndex].Personality = comboBox1.Text;
        }

        //Load from a text file menu item: Open the load dialog and load the team properties from the resulting file as text format. Then update all the form items with the values loaded
        private void textFormatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Textfile|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName != null)
            {
                this.mode = Option.Load;
                this.teamProperties.loadFromTextFile(ofd.FileName);

                comboBoxPlayers.SelectedIndex = 0;
                comboBox1.Text = teamProperties.Properties[0].Personality;
                maskedTextBoxPositionX.Text = teamProperties.Properties[0].Startpoint_x.ToString();
                maskedTextBoxPositionY.Text = teamProperties.Properties[0].Startpoint_y.ToString();

                this.mode = Option.Loaded;
            }
        }

        //Handle the form close: Set closed to true to be able to update the rendering window
        private void Properties_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.closed = true;
        }
    }
}
