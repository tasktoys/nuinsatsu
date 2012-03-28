using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Motion
{
    interface KeyGenerator
    {
       Key Generate(List<SkeletonTimeline> motionList);
    }
}
