using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Ai Attack Trait", menuName = "Deus Animex/Traits/Combat/Ai Attack")]
    public class AiAttackTrait : AttackTrait
    {
        [Header("Ai Attack Settings")]
        [SerializeField] private float _attackDistance = 1f;

        private Vector2 _position = Vector2.zero;

        internal override void SubscribeToMessages()
        {
            base.SubscribeToMessages();
            _controller.transform.parent.gameObject.SubscribeWithFilter<AttackCheckMessage>(AttackCheck, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdatePositionMessage>(UpdatePosition, _instanceId);
        }

        private void AttackCheck(AttackCheckMessage msg)
        {
            var distance = (_position - msg.Position).magnitude;
            if (distance <= _attackDistance)
            {
                msg.DoAfter.Invoke();
            }
        }

        private void UpdatePosition(UpdatePositionMessage msg)
        {
            _position = msg.Position;
        }
    }
}