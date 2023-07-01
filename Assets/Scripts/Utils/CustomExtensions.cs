using System;

namespace Utils {
    public static class Extensions
    {
        public static T Tap<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }
}