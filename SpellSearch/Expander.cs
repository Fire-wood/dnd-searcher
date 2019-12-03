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
            bodyPanel.Size = new System.Drawing.Size(expander.Width, 200);
            bodyPanel.Font = new Font("Consolas", 10, FontStyle.Regular);
            bodyPanel.BorderStyle = BorderStyle.FixedSingle;

            // Spell Range
            Label bodyRange = new Label();
            bodyRange.Text = "Range: " + spell.GetRange();
            //bodyRange.Font = new Font("Consolas", 10, FontStyle.Regular);
            bodyRange.Location = new Point(0, 0);
            bodyRange.Size = new Size(690, 20);
            bodyPanel.Controls.Add(bodyRange);

            // Spell Casting Time
            Label labelContent = new Label();
            labelContent.Text = "Casting Time: " + spell.GetCastingTime();
            labelContent.Font = new Font("Consolas", 10, FontStyle.Regular);
            labelContent.Location = new Point(0, 21);
            labelContent.Size = new Size(690, 20);
            bodyPanel.Controls.Add(labelContent);

            // Spell Duration
            Label bodyDuration = new Label();
            bodyDuration.Text = "Duration: " + spell.GetDuration();
            bodyDuration.Font = new Font("Consolas", 10, FontStyle.Regular);
            bodyDuration.Location = new Point(0, 42);
            bodyDuration.Size = new Size(690, 20);
            bodyPanel.Controls.Add(bodyDuration);

            // Components
            Label bodyComponents = new Label();
            bodyComponents.Text = "Components: " + spell.GetComponents();
            bodyComponents.Font = new Font("Consolas", 10, FontStyle.Regular);
            bodyComponents.Location = new Point(0, 63);
            bodyComponents.Size = new Size(690, 20);
            bodyPanel.Controls.Add(bodyComponents);

            // Available Classes
            Label bodyClasses = new Label();
            bodyClasses.Text = "Available Classes: ";
            if(!spell.GetClasses().Equals(null) && spell.GetClasses().Count > 0)
            {
                foreach (Classes c in spell.GetClasses())
                {
                    bodyClasses.Text += c.ToString().Trim();
                    bodyClasses.Text += ", ";
                }
            }
            
            bodyClasses.Text = bodyClasses.Text.Substring(0, bodyClasses.Text.Length - 2);
            bodyClasses.Font = new Font("Consolas", 10, FontStyle.Regular);
            bodyClasses.Location = new Point(0, 84);
            bodyClasses.Size = new Size(690, 20);
            //bodyClasses.BorderStyle = BorderStyle.FixedSingle;
            bodyPanel.Controls.Add(bodyClasses);

            // Spell Description
            Label bodyDescription = new Label();
            bodyDescription.Text = spell.GetDescription();
            bodyDescription.Font = new Font("Consolas", 10, FontStyle.Regular);
            bodyDescription.Size = new Size(690, 120);
            //bodyDescription.BorderStyle = BorderStyle.FixedSingle;
            bodyDescription.Location = new Point(0, 105);
            bodyPanel.Controls.Add(bodyDescription);


            expander.Content = bodyPanel;
            //spellsPanel.Controls.Add(expander);




            return headerPanel;
        }

    }
}
