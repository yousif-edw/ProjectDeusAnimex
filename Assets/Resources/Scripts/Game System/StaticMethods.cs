using System;
using System.Collections;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System
{
    public static class StaticMethods
    {
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static IEnumerator WaitForFrames(int frames, Action doAfter)
        {
            var totalFrames = Time.frameCount + frames;
            yield return new WaitUntil(() => Time.frameCount >= totalFrames);
            doAfter?.Invoke();
        }
    }
}