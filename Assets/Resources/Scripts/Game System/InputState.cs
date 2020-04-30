using System.Net.Configuration;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System
{
    public struct InputState
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public Vector2 Direction;
        public bool Dash;
        public bool Attack;
    }
}