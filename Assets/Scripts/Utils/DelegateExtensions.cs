using System;
using UnityEngine;

namespace Utils.Extensions
{
    public static class DelegateExtensions
    {
        public static T SafeInvoke<T>(this Func<T> func)
        {
            try
            {
                if (func == null)
                {
                    throw new Exception("Func is null");
                }

                return func();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }

        public static TOUT SafeInvoke<T1, TOUT>(this Func<T1, TOUT> func, T1 value)
        {
            try
            {
                if (func == null)
                {
                    throw new Exception("Func is null");
                }

                return func(value);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }

        public static TOUT SafeInvoke<T1, T2, TOUT>(this Func<T1, T2, TOUT> func, T1 value1, T2 value2)
        {
            try
            {
                if (func == null)
                {
                    throw new Exception("Func is null");
                }

                return func(value1, value2);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }

        public static void SafeInvoke(this Action action)
        {
            try
            {
                if (action == null)
                {
                    return;
                }

                action.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public static void SafeInvoke<T>(this Action<T> action, T value)
        {
            try
            {
                if (action == null)
                {
                    return;
                }

                action.Invoke(value);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 value1, T2 value2)
        {
            try
            {
                if (action == null)
                {
                    return;
                }

                action.Invoke(value1, value2);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
