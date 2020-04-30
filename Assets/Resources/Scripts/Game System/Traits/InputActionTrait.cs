using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Input Action Trait", menuName = "Deus Animex/Traits/Input/Input Action")]
    public class InputActionTrait : Trait
    {
        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.gameObject.Subscribe<UpdateInputMessage>(UpdateInput);
        }

        private void UpdateInput(UpdateInputMessage msg)
        {
            if (!msg.Previous.Dash && msg.Current.Dash)
            {
                _controller.gameObject.SendMessageTo(ActivateDashMessage.INSTANCE, _controller.transform.parent.gameObject);
            }
            else if (!msg.Previous.Attack && msg.Current.Attack)
            {
                _controller.gameObject.SendMessageTo(ActivateAttackMessage.INSTANCE, _controller.transform.parent.gameObject);
            }
        }
    }
}