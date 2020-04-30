using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Controllers
{
    public class UnitAnimationController : MonoBehaviour
    {
        public Animator Animator;
        public SpriteRenderer SpriteRenderer;

        public void StartAttack()
        {
            gameObject.SendMessageTo(AttackStartedMessage.INSTANCE, transform.parent.gameObject);
        }

        public void FinishAttack()
        {
            gameObject.SendMessageTo(AttackFinishedMessage.INSTANCE, transform.parent.gameObject);
        }
    }
}