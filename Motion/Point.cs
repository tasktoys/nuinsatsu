﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Motion
{
    class Point
    {
        public Point(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
