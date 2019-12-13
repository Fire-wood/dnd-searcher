using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SpellSearch
{
    public partial class Expander : UserControl
    {
        public Expander()
        {
            InitializeComponent();
            this.Expanded = true;
        }

        #region Events

        public event EventHandler StateChanged;
        public event CancelEventHandler StateChanging;

        #endregion

        #region Properties

        public bool Expanded { get; private set; }

        public Control Header
        {
            get { return this.header; }

            set
            {
                if (this.header != null)
                    this.Controls.Remove(this.header);

                this.header = value;
                this.header.Dock = DockStyle.Top;
                this.Controls.Add(this.header);
                this.Controls.SetChildIndex(this.header, this.Controls.Count > 1 ? 1 : 0);
            }
        }

        public Control Content
        {
            get { return this.content; }

            set
            {
                if (this.content != null)
                    this.Controls.Remove(this.content);

                this.content = value;
                this.Size = new Size(this.Width, this.header.Height + this.content.Height);
                this.content.Top = this.header.Height;

                this.Controls.Add(this.content);
                this.Controls.SetChildIndex(this.content, 0);
            }
        }

        #endregion

        #region Public methods

        public void Expand()
        {
            if (this.Expanded)
                return;

            if (StateChanging != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                StateChanging(this, args);
                if (args.Cancel)
                    return;
            }

            this.Expanded = true;
            ArrangeLayout();

            if (StateChanged != null)
                StateChanged(this, null);
        }

        public void Collapse()
        {
            if (!this.Expanded)
                return;

            if (StateChanging != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                StateChanging(this, args);
                if (args.Cancel)
                    return;
            }

            if (this.Content != null)
                this.contentHeight = this.Content.Height;
            this.Expanded = false;
            ArrangeLayout();

            if (StateChanged != null)
                StateChanged(this, null);
        }

        public void Toggle()
        {
            if (this.Expanded)
                Collapse();
            else
                Expand();
        }

        #endregion

        #region Private methods
        
        private void ArrangeLayout()
        {
            int h = 0;
            if (this.header != null)
                h += this.header.Height;
            if (this.Expanded && this.content != null)
                h += this.content.Height;
            this.Size = new Size(this.Width, h);
        }

        #endregion
        
        #region Priate fields

        private Control header;
        private Control content;
        private int contentHeight = 0;

        #endregion
    }

    public static class ExpanderHelper
    {
        public static Label CreateLabelHeader(Expander expander, string text, Color backColor, Image collapsedImage = null, Image expandedImage = null, int height = 25, Font font = null)
        {
            Label headerLabel = new Label();
            headerLabel.Text = text;
            headerLabel.AutoSize = false;
            headerLabel.Height = height;
            if (font != null)
                headerLabel.Font = font;
            headerLabel.TextAlign = ContentAlignment.MiddleLeft;
            if (collapsedImage != null && expandedImage != null)
            {
                headerLabel.Image = collapsedImage;
                headerLabel.ImageAlign = ContentAlignment.MiddleRight;
            }
            headerLabel.BackColor = backColor;

            if (collapsedImage != null && expandedImage != null)
            {
                expander.StateChanged += delegate { headerLabel.Image = expander.Expanded ? collapsedImage : expandedImage; };
            }

            headerLabel.Click += delegate { expander.Toggle(); };

            expander.Header = headerLabel;

            return headerLabel;
        }


        public static Panel CreateSpellExpander(Expander expander, Spell spell, Image collapsedImage = null, Image expandedImage = null, int height = 25)
        {
            Font labelFont = new Font("Consolas", 10, FontStyle.Regular);
            Size labelSize = new Size(690, 20);


            // Header Panel Parts
            Panel headerPanel = new Panel();
            headerPanel.Size = new Size(690, 40);
            headerPanel.AutoSize = false;
            headerPanel.Font = new Font("Consolas", 16, FontStyle.Regular);

            // Spell Name
            Label headerName = new Label();
            headerName.Text = spell.GetName();
            headerName.AutoSize = true;
            headerPanel.Controls.Add(headerName);

            // Spell Level
            Label headerLevel = new Label();
            headerLevel.Text = spell.GetLevel().ToString();
            headerLevel.Location = new Point(250, 0);
            headerLevel.Size = new Size(50, 80);
            headerPanel.Controls.Add(headerLevel);

            // Spell School
            Label headerSchool = new Label();
            headerSchool.Text = spell.GetSchool().ToString();
            headerSchool.Location = new Point(300, 0);
            headerSchool.AutoSize = true;
            headerPanel.Controls.Add(headerSchool);

            // Edition
            Label headerEdition = new Label();
            headerEdition.Text = spell.GetEdition();
            headerEdition.Location = new Point(450, 0);
            headerEdition.AutoSize = true;
            headerPanel.Controls.Add(headerEdition);

            // Reference Book
            Label headerBook = new Label();
            headerBook.Text = spell.GetBook();
            headerBook.Location = new Point(500, 0);
            headerBook.AutoSize = true;
            headerPanel.Controls.Add(headerBook);


            // Collapse/Expanded Icon
            Label headerIcon = new Label();
            if (collapsedImage != null && expandedImage != null)
            {
                headerIcon.Image = collapsedImage;
                headerIcon.ImageAlign = ContentAlignment.MiddleRight;
            }

            //spellExpander.BackColor = backColor;
            
            if (collapsedImage != null && expandedImage != null)
            {
                expander.StateChanged += delegate { headerIcon.Image = expander.Expanded ? collapsedImage : expandedImage; };
            }

            headerPanel.Click += delegate { expander.Toggle(); };
            expander.Header = headerPanel;
            headerPanel.Controls.Add(headerIcon);


            // Body Panel Components
            Panel bodyPanel = new Panel();
            bodyPanel.Font = new Font("Consolas", 10, FontStyle.Regular);
            bodyPanel.BorderStyle = BorderStyle.FixedSingle;
            bodyPanel.AutoSize = true;
            bodyPanel.MaximumSize = new Size(expander.Width, 0);

            int cumulativeY = 0;
            
            // Spell Range
            Label bodyRange = GenerateLabel(labelFont, cumulativeY, expander.Width);
            bodyRange.Text = "Range: " + spell.GetRange();
            bodyPanel.Controls.Add(bodyRange);
            cumulativeY += bodyRange.Height;

            // Spell Casting Time
            Label bodyCasting = GenerateLabel(labelFont, cumulativeY, expander.Width);
            bodyCasting.Text = "Casting Time: " + spell.GetCastingTime();
            bodyPanel.Controls.Add(bodyCasting);
            cumulativeY += bodyCasting.Height;


            // Spell Duration
            Label bodyDuration = GenerateLabel(labelFont, cumulativeY, expander.Width);
            bodyDuration.Text = "Duration: " + spell.GetDuration();
            bodyPanel.Controls.Add(bodyDuration);
            cumulativeY += bodyDuration.Height;

            // Components
            Label bodyComponents = GenerateLabel(labelFont, cumulativeY, expander.Width);
            bodyComponents.Text = "Components: " + spell.GetComponents();
            bodyPanel.Controls.Add(bodyComponents);
            cumulativeY += bodyComponents.Height;

            // Available Classes
            Label bodyClasses = GenerateLabel(labelFont, cumulativeY, expander.Width);
            bodyClasses.Text = "Available Classes: ";
            if (!spell.GetClasses().Equals(null) && spell.GetClasses().Count > 0)
            {
                foreach (Classes c in spell.GetClasses())
                {
                    bodyClasses.Text += c.ToString().Trim();
                    bodyClasses.Text += ", ";
                }
            }
            bodyClasses.Text = bodyClasses.Text.Substring(0, bodyClasses.Text.Length - 2);
            bodyPanel.Controls.Add(bodyClasses);
            cumulativeY += bodyClasses.Height;

            // Spell Description
            Label bodyDescription = GenerateLabel(labelFont, cumulativeY, expander.Width);
            bodyDescription.Text = "\n" + spell.GetDescription().Replace("\\n", "\n") + "\n";

            bodyPanel.Controls.Add(bodyDescription);
            cumulativeY += bodyDescription.Height + 5;

            // Spell Higher Description
            if (spell.GetHigherDescription() != "")
            {
                Label bodyHigher = GenerateLabel(labelFont, cumulativeY, expander.Width);
                bodyHigher.Text = "At higher Levels: " + spell.GetHigherDescription() + "\n";
                bodyPanel.Controls.Add(bodyHigher);
                cumulativeY += bodyHigher.Height + 5;
            }

            // DM Note
            if(spell.GetDMNote() != "")
            {
                Label bodyDM = GenerateLabel(labelFont, cumulativeY, expander.Width);
                bodyDM.Text = "Game Master's Note: " + spell.GetDMNote() + "\n";
                bodyPanel.Controls.Add(bodyDM);
                cumulativeY += bodyDM.Height + 5;
            }

            expander.Content = bodyPanel;
            //spellsPanel.Controls.Add(expander);




            return headerPanel;
        }

        static private Label GenerateLabel(Font f, int ypos, int expanderWidth)
        {
            Label l = new Label();
            l.Font = f;
            //l.Size = s;
            l.Location = new Point(0, ypos);
            l.AutoSize = true;
            l.MaximumSize = new Size(expanderWidth, 0);
            l.Margin = new Padding(5);

            return l;
        }
    }
}
