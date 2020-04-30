using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Player Trait", menuName = "Deus Animex/Traits/Player")]
    public class PlayerTrait : Trait
    {
        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<PlayerCheckMessage>(PlayerCheck, _instanceId);
        }

        private void PlayerCheck(PlayerCheckMessage msg)
        {
            msg.DoAfter.Invoke();
        }
    }
}