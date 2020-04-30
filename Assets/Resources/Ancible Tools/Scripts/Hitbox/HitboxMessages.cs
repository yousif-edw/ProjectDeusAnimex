using System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Ancible_Tools.Scripts.Hitbox
{
    public class EnterCollisionWithObjectMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }

    public class ExitCollisionWithObjectMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }

    public class HitboxCheckMessage : EventMessage
    {
        public Action<HitboxController> DoAfter { get; set; }
    }

    public class RegisterCollisionMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }

    public class UnregisterCollisionMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }
}