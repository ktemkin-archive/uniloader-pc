using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.Diagnostics;

namespace FilledContainer
{
    public partial class FilledContainer : UserControl
    {
        #region Local Fields

        FillLevel[] levels;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets an array of FillLevels to be plotted.
        /// </summary>
        public FillLevel[] Levels
        {
            get
            {
                return levels;
            }
            set
            {
                setFillLevel(value);
            }
        }

        /// <summary>
        ///     Gets or sets a single FillLevel, to be plotted. If multiple levels are present, this _gets_ the first level, and overwrites all levels.
        /// </summary>
        public FillLevel Level
        {
            get
            {
                return levels[0];
            }
            set
            {
                setFillLevel(value);
            }
        }
        

        #endregion

        public FilledContainer()
        {
            //Init WinForms designed components
            InitializeComponent();

            //Bind events...
            this.Load += new EventHandler(CapacityControl_Load);
        }
        
        void CapacityControl_Load(object sender, EventArgs e)
        {
            initializePlot();   
        }

        void initializePlot()
        {
            GraphPane graph = plot.GraphPane;

            graph.Title.FontSpec.Border.IsVisible = false;

            //remove the framing from the graph
            graph.Chart.Fill = graph.Fill = new Fill(this.BackColor);
            graph.Chart.Border.IsVisible = false;
            graph.Border.IsVisible = false;
            graph.YAxis.IsVisible = graph.Y2Axis.IsVisible = false;
            graph.YAxis.IsVisible = graph.Y2Axis.IsVisible = false;
            
            //hide the title/legend
            graph.Title.IsVisible = false;
            graph.Legend.IsVisible = false;
            graph.Legend.IsShowLegendSymbols = false;
            
            
            //set up the axes
            graph.XAxis.IsVisible = graph.YAxis.IsVisible = graph.X2Axis.IsVisible = graph.Y2Axis.IsVisible = false;
            graph.XAxis.Type = AxisType.Text;
            graph.YAxis.Type = AxisType.Linear;
            graph.YAxis.MinSpace = 1;
            graph.XAxis.Title.IsVisible = false;

            plot.GraphPane.Border.IsVisible = false;
            plot.BorderStyle = BorderStyle.None;
            plot.IsAntiAlias = true;
            plot.IsAutoScrollRange = false;

            

            
        }

        /// <summary>
        ///     Sets the fill levels of the container and updates the plot.
        /// </summary>
        /// <param name="level">The levels to plot.</param>
        public void setFillLevel(FillLevel[] level)
        {
            levels = level;

            int maxHeight = 0;
            
            if (level == null)
                return;

            //Shortcut to the graph pane
            GraphPane graph = plot.GraphPane;

            //Clear existing bars
            graph.CurveList.Clear();
            
            //track the used space, and the remaining space
            PointPairList free = new PointPairList();
            PointPairList used = new PointPairList();
            PointPairList over = new PointPairList();

            //Add the current used and free portions to the known capacity.
            for (int i = 0; i < level.Length; ++i)
            {
                used.Add(i, Math.Min(level[i].Actual, level[i].Max));
                free.Add(i, level[i].Remaining);
                over.Add(i, level[i].Overflow);
                
                //find the maximum heigh of any given bar
                maxHeight = Math.Max(level[i].Max, Math.Max(level[i].Actual, maxHeight));
            }

            //Create bar sections representing the used and free space
            BarItem usedBar = graph.AddBar(level[0].LabelActual, used, Color.Blue);
            BarItem freeBar = graph.AddBar(level[0].LabelRemainder, free, Color.Tan);
            BarItem overBar = graph.AddBar(level[0].LabelOverflow, over, Color.Red);

            usedBar.Label.FontSpec = new FontSpec("Arial", 24, Color.White, false, false, false);

            freeBar.Bar.Border = new Border(Color.Black, 1);
            usedBar.Bar.Border = new Border(Color.Black, 2);
            overBar.Bar.Border = new Border(Color.Black, 1);

            usedBar.Bar.Fill = new Fill(Color.FromArgb(0xf1, 0x7c, 0x0e));//Color.FromArgb(0x00, 0xb5, 0xe5));
            freeBar.Bar.Fill = new Fill(Color.White); //Color.FromArgb(210, 255, 210));
            overBar.Bar.Fill = new Fill(Color.Red);

            /*
            //Fill any overfull regions with 'warning tape' colors.
            Color[] warningTape = new Color[80];
            for (int i = 0; i < warningTape.Length; ++i)
                warningTape[i] = ((i / 4) % 2 == 1) ? Color.FromArgb(0xFF, 0xEE, 0x9F) : Color.FromArgb(0x4f, 0x4f, 0x46);
            overBar.Bar.Fill = new Fill(warningTape, 45);
             */
            
            //set the graph mode
            graph.BarSettings.Base = BarBase.X;
            graph.BarSettings.Type = BarType.Stack;

            //handle axes
            graph.YAxis.Scale.Min = -2;
            graph.YAxis.Scale.Max = maxHeight + 2;


            graph.Margin.All = 0;

            //Handle graph title           
            if (level[0].Description != null)
                graph.XAxis.Title.Text = level[0].Description;

            //update the graph
            plot.AxisChange();
            plot.Invalidate();

        
          
        }

        public void setFillLevel(FillLevel level)
        {
            //wrap the single item in an array, and then process
            setFillLevel(new FillLevel[1] { level });
        }
        

        private void plot_Load(object sender, EventArgs e)
        {

        }
    }
}
