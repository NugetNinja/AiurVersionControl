﻿using System;

namespace SnakeGame.Models
{
    public class Position : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Position p)
            {
                return X == p.X && Y == p.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }
        
        public override string ToString()
        {
            return X + "," + Y;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
