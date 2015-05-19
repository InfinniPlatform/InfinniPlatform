﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    public enum Priority
    {
        Standard = 0,
        Higher = 1
    }

    public sealed class PathStringProvider
    {
        public PathString PathString { get; set; }

        public Priority Priority { get; set; }
    }

    public static class PathStringExtensions
    {
        public static PathStringProvider Create(this PathString pathString, Priority priority)
        {
            return new PathStringProvider()
            {
                PathString = pathString,
                Priority = priority
            } ;
        }
    }
}
