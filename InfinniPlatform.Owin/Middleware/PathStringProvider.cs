using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    public enum Priority
    {
        Standard = 0,
        Higher = 1,
        Concrete = 2
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
            return new PathStringProvider
            {
                PathString = pathString,
                Priority = priority
            };
        }
    }
}