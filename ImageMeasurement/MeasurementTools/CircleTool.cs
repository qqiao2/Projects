using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageMeasurement.MeasurementTools
{
    public class CircleTool : ITool
    {
        public CircleTool() { }
        public CircleTool(Point center, double radius) { 
            Center = center;
            Radius = radius; 
        }

        public string Name => "CircleTool";

        public double Radius { get; set; }
        public Point Center { get; set; }
    }
}
