using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Unit State Trait", menuName = "Deus Animex/Traits/Unit State")]
    public class UnitStateTrait : Trait
    {
        private UnitState _state = UnitState.Active;

        private UpdateUnitStateMessage _updateUnitStateMsg = new UpdateUnitStateMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            _updateUnitStateMsg.State = _state;
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetUnitStateMessage>(SetUnitState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestUnitStateMessage>(RequestUnitState, _instanceId);
        }

        private void SetUnitState(SetUnitStateMessage msg)
        {
            _state = msg.State;
            _updateUnitStateMsg.State = _state;
            _controller.gameObject.SendMessageTo(_updateUnitStateMsg, _controller.transform.parent.gameObject);
        }

        private void RequestUnitState(RequestUnitStateMessage msg)
        {
            _controller.gameObject.SendMessageTo(_updateUnitStateMsg, msg.Sender);
        }
    }
}