using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeamYaffa;
using TeamYaffa.CRaPI;
using System.Drawing.Drawing2D;
using System.Numerics;


namespace RoboGang.Visualization
{
    //Form for visualizing the playfield as seen by the players for debug visualization
    public partial class FieldVisualizer : Form
    {
        //2 Lists (Index for Team) holding the players on the playfield
        private List<Player>[] players = new List<Player>[2];
       
        
        

        //Get / Set the whole Array
        public List<Player>[] Players
        {
            get { return players; }
            set { players = value; }
        }

        //Get / Set the playerlist of the first team
        public List<Player> TeamA {
            get { return players[0]; }
            set { players[0] = value; }
        }

        //Get / Set the playerlist of the first team
        public List<Player> TeamB
        {
            get { return players[1]; }
            set { players[1] = value; }
        }

        //Add a player to the list of Side A/B (Left/Right)
        public void addPlayer(Side s, Player p) {
            if (s == Side.Left)
                players[0].Add(p);
            else
                players[1].Add(p);
        }

        public FieldVisualizer()
        {
            //Create list instances for 2 Lists holding player references of Team 1 and 2 (Index 0 and 1)
            for (int i = 0; i < players.Length; i++) {
                players[i] = new List<Player>();
            }
            InitializeComponent();
            
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            //Put drawing update here...


            //Clear the listboxes
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            //For each team
            for (int i = 0; i < this.players.Length; i++) { 
                
                //For each player in team
                foreach (Player p in players[i]){

                    //Update everything for each player

                    //Add the players to the list boxes in the form like "TeamName : TeamSide [Left|Right] - PlayerObjectName.[G if goalie]"
                    switch (i) {
                        case 0: listBox1.Items.Add(p.TeamName+" : "+(p.TeamSide==Side.Left?"Left":"Right")+" - "+p + (p.IsGoalie?".G":"")); break;
                        case 1: listBox2.Items.Add(p.TeamName + " : " + (p.TeamSide == Side.Left ? "Left" : "Right") + " - " + p + (p.IsGoalie ? ".G" : "")); break;
                        default: break;
                    }
                }
            }
        }

        private void FieldVisualizer_Load(object sender, EventArgs e)
        {

        }
    }
}
