using System;
using Domain;
using UnityEngine;

namespace Utils {
    public static class Extensions
    {
        public static T Tap<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }
    }

    public static class BodyPartExtensions
    {

        public static Transform GetTransform(this BodyPart bodyPart)
        {
            return bodyPart?.cell?.gameObject?.transform;
        }

        public static Transform GetTransform(this Cell cell)
        {
            return cell?.gameObject?.transform;
        }

        public static Vector3 GetPosition(this Cell cell)
        {
            // TODO: figure out the optionals here
            return cell?.gameObject?.transform.position ?? Vector3.zero;
        }
    }
}