using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MazeLaser.Model
{
    public enum Lean { Right, Left }
    public enum ReflectiveSide { Right, Left, Both }
    class Mirror
    {
        public Lean Lean { get; set; }
        public ReflectiveSide ReflectiveSide { get; set; }

        public Mirror(Lean lean, ReflectiveSide reflectiveSide)
        {
            Lean = lean;
            ReflectiveSide = reflectiveSide;
        }

    }
}
