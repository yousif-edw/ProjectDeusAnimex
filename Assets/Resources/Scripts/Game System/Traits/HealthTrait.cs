using Assets.Ancible_Tools.Scripts.Traits;
using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Traits
{
    [CreateAssetMenu(fileName = "Health Trait", menuName = "Deus Animex/Traits/Combat/Health")]
    public class HealthTrait : Trait
    {
        [SerializeField] private int _startingHealth;

        private int _currentHealth = 0;
        private int _maximumHealth = 0;

        private UpdateHealthMessage _updateHealthMsg = new UpdateHealthMessage();

        public override void SetupController(TraitController controller)
        {
            base.SetupController(controller);
            _currentHealth = _startingHealth;
            _maximumHealth = _startingHealth;
            _updateHealthMsg.Current = _currentHealth;
            _updateHealthMsg.Maximum = _maximumHealth;
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestHealthMessage>(RequestHealth, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<TakeDamageMessage>(TakeDamage, _instanceId);
        }

        private void RequestHealth(RequestHealthMessage msg)
        {
            _controller.gameObject.SendMessageTo(_updateHealthMsg, msg.Sender);
        }

        private void TakeDamage(TakeDamageMessage msg)
        {
            _currentHealth -= msg.Damage;
            _updateHealthMsg.Current = _currentHealth;
            _controller.gameObject.SendMessageTo(_updateHealthMsg, _controller.transform.parent.gameObject);
            if (_currentHealth < 0)
            {
                Debug.Log("Dead!");
            }
        }


    }
}