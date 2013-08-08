using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XEO.Print;

namespace Participrint
{
    /// <summary>
    /// Set of values denominating the format and size and position of lables on a page.
    /// </summary>
    public struct LabelPageFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelPageFormat"/> class.
        /// </summary>
        /// <param name="pageSize">The page format.</param>
        public LabelPageFormat(PageSize pageSize)
            : this()
        {
            PageWidth = pageSize.Width;
            PageHeight = pageSize.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelPageFormat"/> class.
        /// </summary>
        /// <param name="pageWidthInMM">The page width.</param>
        /// <param name="pageHeightInMM">The page height.</param>
        public LabelPageFormat(double pageWidthInMM, double pageHeightInMM)
            : this()
        {
            PageWidth = pageWidthInMM;
            PageHeight = pageHeightInMM;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LabelPageFormat"/> class.
        /// </summary>
        public LabelPageFormat()
        {
            PageWidth = 0.0d;
            PageHeight = 0.0d;
            LabelsPerRow = 1;
            LabelsPerCol = 1;
            PageBorderHorizontal = 0.0d;
            PageBorderVertical = 0.0d;

            LabelDistanceHorizontal = 0.0d;
            LabelDistanceVertical = 0.0d;
        }

        /// <summary>
        /// Gets or sets the page height.
        /// </summary>
        /// <value>The page height in MM.</value>
        public double PageHeight { get; set; }

        /// <summary>
        /// Gets or sets the page width.
        /// </summary>
        /// <value>The page width in.</value>
        public double PageWidth { get; set; }

        /// <summary>
        /// Gets or sets the labels per row.
        /// </summary>
        /// <value>The labels per row.</value>
        public int LabelsPerRow { get; set; }

        /// <summary>
        /// Gets or sets the labels per col.
        /// </summary>
        /// <value>The labels per col.</value>
        public int LabelsPerCol { get; set; }

        /// <summary>
        /// Gets or sets the horizontal page border.
        /// </summary>
        /// <value>The horizontal page border in MM.</value>
        public double PageBorderHorizontal { get; set; }

        /// <summary>
        /// Gets or sets the vertical page border.
        /// </summary>
        /// <value>The vertical page border.</value>
        public double PageBorderVertical { get; set; }

        /// <summary>
        /// Gets or sets the horizontal label distance.
        /// This is the distance from the end of one lable to the start of the next label.
        /// </summary>
        /// <value>The horizontal label distance.</value>
        public double LabelDistanceHorizontal { get; set; }

        /// <summary>
        /// Gets or sets the vertical label distance.
        /// This is the distance from the end of one lable to the start of the next label.
        /// </summary>
        /// <value>The vertical label distance.</value>
        public double LabelDistanceVertical { get; set; }
    }
}