using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Motion
{
    class Skeleton : Dictionary<JointID,Point>
    {
        //private static final long serialVersionUID = 1L;
        public Skeleton()
            : base()
        {
        }
    }
}
