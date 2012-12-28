using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FilledContainer
{
    public partial class VerticalFilledContainer : UserControl
    {
        public VerticalFilledContainer()
        {
            InitializeComponent();

            //add a resize event
            this.Resize += new EventHandler(MonoFilledContainer_Resize);

            //set the gloss panels' semitransparency
            pnlValueGloss.BackColor = pnlGloss.BackColor = Color.FromArgb(100, 255, 255, 255);
            

            //assume an initially empty container
            this.value = 0;       
        }

        void MonoFilledContainer_Resize(object sender, EventArgs e)
        {
            relayout();
        }

        private float value;

        /// <summary>
        ///     The value for the filled container. Typically less than one; if greater, the 
        ///     filled container will display its 'overflow' state.
        /// </summary>
        public float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                //If we have a valid value, apply it and adjust the layout.
                if (value >= 0)
                {
                    this.value = value;
                    relayout();
                }
            }
        }

        /// <summary>
        /// If true, the background will be rendered with a glossy effect.
        /// </summary>
        public bool GlossyBackground
        {
            get
            {
                return pnlGloss.Visible;
            }
            set
            {
                pnlGloss.Visible = value;
            }
        }

        /// <summary>
        ///     Specifies the background color of the control during its overflowing state.
        /// </summary>
        public Color OverflowColor
        {
            get
            {
                return pnlOverflow.BackColor;
            }
            set
            {
                if(value!=null)
                    pnlOverflow.BackColor = value;
            }
        }


        void relayout()
        {
            //adjust, if the forecolor has been set
            pnlValue.BackColor = this.ForeColor;

            if (value > 1)
            {
                //show the overflow panel, hide the value panel   
                pnlOverflow.Visible = true;
                pnlValue.Visible = false;

                //fill the whole control with the overflow panel
                pnlOverflow.Size = this.Size;
                pnlOverflow.Location = new Point(0, 0);
            }
            else
            {                
                pnlValue.Visible = true;
                pnlOverflow.Visible = false;

                //set the value panel's height, scaled to the current value
                pnlValue.Height = (int)((float)this.Height * value);

                //'dock' the value panel to the bottom
                pnlValue.Width = this.Width;
                pnlValue.Location = new Point(0, this.Height - pnlValue.Height);
            }

            this.Invalidate();
        }

        private void MonoFilledContainer_Load(object sender, EventArgs e)
        {

        }

        private void pnlOverflow_Paint(object sender, PaintEventArgs e)
        {

        }



    }
}
