using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Teamspeak.Sdk
{
    /// <summary>
    ///  A vector in 3D space
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        /// <summary>
        /// The x-coordinate of this <see cref="Vector"/> structure.
        /// </summary>
        public float X;
        /// <summary>
        /// The y-coordinate of this <see cref="Vector"/> structure.
        /// </summary>
        public float Y;
        /// <summary>
        /// The z-coordinate of this <see cref="Vector"/> structure.
        /// </summary>
        public float Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> structure. 
        /// </summary>
        /// <param name="x">The <see cref="X"/> value</param>
        /// <param name="y">The <see cref="Y"/> value</param>
        /// <param name="z">The <see cref="Z"/> value</param>
        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", X, Y, Z);
        }
    }
}
