using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageMeasurement.MeasurementTools
{
    public class LineTool : ITool
    {
        public LineTool() { }

        public LineTool(Point s, Point e) {
            Start = s;
            End = e;
        }

        public string Name => "LineTool";

        public Point Start {  get; set; }
        public Point End { get; set; }
    }
}
