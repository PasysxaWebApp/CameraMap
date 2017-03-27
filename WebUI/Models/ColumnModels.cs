using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models
{
    public class ColumnsHeader
    {
        private int _width = 100;
        //private string _dataFormatString = "";

        public string headerText { get; set; }
        public int width
        {
            get { return _width; }
            set { _width = value; }
        }
        public string dataType
        {
            get;
            set;
        }
        public string dataFormatString
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the text alignment of data cells. Possible values are "left", "right", "canter".
        /// </summary>
        public string textAlignment { get; set; }
        /// <summary>
        /// Determines whether rows are merged. Possible values are: "none", "free" and "restricted".
        /// </summary>
        public string rowMerge { get; set; }
        public bool readOnly { get; set; }
        public bool visible { get; set; }
        public ColumnsHeader()
        { 
            readOnly = true;
            visible = true;
        }
        public ColumnsHeader(string headertext, int width, string datatype):this()
        {
            this.headerText = headertext;
            this.width = width;
            this.dataType = datatype;
        }

        public ColumnsHeader(string headertext, int width, string datatype, string dataformatstring):this( headertext,  width,  datatype)
        {
            this.dataFormatString = dataformatstring;
        }
    }
}
