using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace SpellSearch
{
    public partial class SpellSearcher : Form
    {
        private List<Spell> spellList;

        public SpellSearcher()
        {
            InitializeComponent();
            LoadSpells();
        }

        private void LoadSpells()
        {
            spellList = new List<Spell>();
            string[] files = Directory.GetFiles(@"I:\Code\SpellSearch\SpellSearch\Spells", "*.txt", SearchOption.TopDirectoryOnly);
            Console.WriteLine("Loading Spells...");
            foreach(string file in files)
            {
                Console.WriteLine("Loading " + Path.GetFileName(file));

                string text = File.ReadAllText(file);

                //spellList.Add(Path.GetFileName(file));
                string fileName = Path.GetFileName(file);
                if(fileName.EndsWith(".txt"))
                {
                    fileName = fileName.Substring(0, fileName.Length - 4);
                }


                spellList.Add(new Spell(fileName, text));
                
            }
        }

        private void SpellSearcher_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.Focus();
            updateSpellList();
            /*
            Expander expander = new Expander();
            expander.Size = new Size(250, 400);
            expander.Left = 350;
            expander.Top = 10;
            expander.BorderStyle = BorderStyle.FixedSingle;

            ExpanderHelper.CreateLabelHeader(expander, "Header", SystemColors.ActiveBorder, Properties.Resources.Collapse, Properties.Resources.Expand);

            Label labelContent = new Label();
            labelContent.Text = "This is the content part.\r\n\r\nYou can put any Controls here. You can use a Panel, a CustomControl, basically, anything you want.";
            labelContent.Size = new System.Drawing.Size(expander.Width, 80);
            expander.Content = labelContent;

            this.Controls.Add(expander);
            */
        }

        private void updateSpellList()
        {
            List<Spell> matches = new List<Spell>();

            try
            {
                string currentString = spellSearchBox.Text.ToString().ToLower();

                for (int i = 0; i < spellList.Count; i++)
                {
                    string curr = spellList[i].GetName().ToLower();
                    if (curr.Contains(currentString))
                    {
                        matches.Add(spellList[i]);
                    }
                }

                // Create Entries for each match
                spellsPanel.Controls.Clear();
                int y = 5;
                int boxHeight = 40;

                for (int j = 0; j < matches.Count; j++)
                {

                    Expander spellEntry = new Expander();
                    spellEntry.Size = new Size(690, 100);
                    spellEntry.Location = new Point(20, y);
                    spellEntry.BorderStyle = BorderStyle.Fixed3D;
                    //spellEntry.BackColor = Color.Aqua;
                    spellEntry.Anchor = (AnchorStyles.Top | AnchorStyles.Left);

                    ExpanderHelper.CreateSpellExpander(spellEntry, matches[j], Properties.Resources.Collapse, Properties.Resources.Expand);
                    spellEntry.Collapse();

                    switch (matches[j].GetSchool())
                    {
                        case School.Abjuration:
                            spellEntry.BackColor = Color.AliceBlue;
                            break;
                        case School.Conjuration:
                            spellEntry.BackColor = Color.Wheat;
                            break;
                        case School.Divination:
                            spellEntry.BackColor = Color.DarkBlue;
                            break;
                        case School.Illusion:
                            spellEntry.BackColor = Color.DeepSkyBlue;
                            break;
                        case School.Necromancy:
                            spellEntry.BackColor = Color.Purple;
                            break;
                        case School.Transmutation:
                            spellEntry.BackColor = Color.LawnGreen;
                            break;
                        default:
                            spellEntry.BackColor = Color.Beige;
                            break;
                    }

                    spellsPanel.Controls.Add(spellEntry);

                    spellsPanel.Show();
                    y += boxHeight / 2;

                }
            }

            catch (Exception)
            {
                Console.WriteLine("Unknown Input");
                return;
            }
        }


        private void spellSearchBox_TextChanged(object sender, EventArgs e)
        {
            updateSpellList();
        }

    }
}
