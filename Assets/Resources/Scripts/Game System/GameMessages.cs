using System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System
{
    public class UpdateInputMessage : EventMessage
    {
        public InputState Previous { get; set; }
        public InputState Current { get; set; }
    }

    public class SetDirectionMessage : EventMessage
    {
        public Vector2 Direction { get; set; }
    }

    public class UpdateDirectionMessage : EventMessage
    {
        public Vector2 Direction { get; set; }
    }

    public class RequestDirectionMessage : EventMessage
    {
        public static RequestDirectionMessage INSTANCE => new RequestDirectionMessage();
    }

    public class UnitFixedUpdateMessage : EventMessage
    {
        public static UnitFixedUpdateMessage INSTANCE => new UnitFixedUpdateMessage();
    }

    public class SetUnitAnimationStateMessage : EventMessage
    {
        public UnitAnimationState State { get; set; }
    }

    public class SetCameraPositionMessage : EventMessage
    {
        public Vector2 Position { get; set; }
    }

    public class UpdatePositionMessage : EventMessage
    {
        public Vector2 Position { get; set; }
    }

    public class RequestPositionMessage : EventMessage
    {
        public static RequestPositionMessage INSTANCE => new RequestPositionMessage();
    }

    public class SetPositionMessage : EventMessage
    {
        public Vector2 Position { get; set; }
    }

    public class SetUnitStateMessage : EventMessage
    {
        public UnitState State { get; set; }
    }

    public class UpdateUnitStateMessage : EventMessage
    {
        public UnitState State { get; set; }
    }

    public class RequestUnitStateMessage : EventMessage
    {
        public static RequestUnitStateMessage INSTANCE => new RequestUnitStateMessage();
    }

    public class ActivateDashMessage : EventMessage
    {
        public static ActivateDashMessage INSTANCE => new ActivateDashMessage();
    }

    public class DeactivateDashMessage : EventMessage
    {
        public static DeactivateDashMessage INSTANCE => new DeactivateDashMessage();
    }

    public class AttackStartedMessage : EventMessage
    {
        public static AttackStartedMessage INSTANCE => new AttackStartedMessage();
    }

    public class AttackFinishedMessage : EventMessage
    {
        public static AttackFinishedMessage INSTANCE => new AttackFinishedMessage();
    }

    public class ActivateAttackMessage : EventMessage
    {
        public static ActivateAttackMessage INSTANCE => new ActivateAttackMessage();
    }

    public class DeactivateAttackMessage : EventMessage
    {
        public static DeactivateAttackMessage INSTANCE => new DeactivateAttackMessage();
    }

    public class PlayerCheckMessage : EventMessage
    {
        public Action DoAfter { get; set; }
    }

    public class UnitUpdateMessage : EventMessage
    {
        public static UnitUpdateMessage INSTANCE => new UnitUpdateMessage();
    }

    public class AttackCheckMessage : EventMessage
    {
        public Vector2 Position { get; set; }
        public Action DoAfter { get; set; }
    }

    public class UpdateHealthMessage : EventMessage
    {
        public int Current { get; set; }
        public int Maximum { get; set; }
    }

    public class RequestHealthMessage : EventMessage
    {
        public static RequestHealthMessage INSTANCE => new RequestHealthMessage();
    }

    public class TakeDamageMessage : EventMessage
    {
        public int Damage { get; set; }
    }

    public class SetOwnerMessage : EventMessage
    {
        public GameObject Owner { get; set; }
    }

    public class UpdateOwnerMessage : EventMessage
    {
        public GameObject Owner { get; set; }
    }

    public class RequestOwnerMessage : EventMessage
    {
        public static RequestOwnerMessage INSTANCE => new RequestOwnerMessage();
    }

}