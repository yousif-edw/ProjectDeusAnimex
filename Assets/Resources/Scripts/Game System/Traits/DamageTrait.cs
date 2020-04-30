using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Damage Trait", menuName = "Deus Animex/Traits/Combat/Damage")]
    public class DamageTrait : Trait
    {
        [SerializeField] private int _damage;

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            _controller.gameObject.SendMessageTo(new TakeDamageMessage{Damage = _damage}, _controller.transform.parent.gameObject );
            _controller.gameObject.SendMessageTo(new RemoveTraitFromUnitByControllerMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }


    }
}