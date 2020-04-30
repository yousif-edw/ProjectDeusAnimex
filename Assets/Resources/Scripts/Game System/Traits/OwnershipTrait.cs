using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Ownership Trait", menuName = "Deus Animex/Traits/Ownership Trait")]
    public class OwnershipTrait : Trait
    {
        private GameObject _owner = null;

        private UpdateOwnerMessage _updateOwnerMsg = new UpdateOwnerMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetOwnerMessage>(SetOwner, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestOwnerMessage>(RequestOwner, _instanceId);
        }

        private void SetOwner(SetOwnerMessage msg)
        {
            _owner = msg.Owner;
            _updateOwnerMsg.Owner = _owner;
            _controller.gameObject.SendMessageTo(_updateOwnerMsg, _controller.transform.parent.gameObject);
        }

        private void RequestOwner(RequestOwnerMessage msg)
        {
            if (_owner)
            {
                _controller.gameObject.SendMessageTo(_updateOwnerMsg, _controller.transform.parent.gameObject);
            }
        }
    }
}