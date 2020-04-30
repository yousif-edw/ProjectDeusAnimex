using MessageBusLib;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Controllers
{
    public class InputController : MonoBehaviour
    {
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";

        private const string DASH = "Dash";
        private const string ATTACK = "Attack";

        private InputState _previous = new InputState();

        private UpdateInputMessage _updateInptMsg = new UpdateInputMessage();

        void Update()
        {
            _updateInptMsg.Previous = _previous;
            var vertical = Input.GetAxisRaw(VERTICAL);
            var horizontal = Input.GetAxisRaw(HORIZONTAL);
            _updateInptMsg.Current = new InputState
            {
                Up = vertical > 0,
                Down = vertical < 0,
                Left = horizontal < 0,
                Right = horizontal > 0,
                Direction = new Vector2(horizontal, vertical),
                Dash = Input.GetButton(DASH),
                Attack = Input.GetButton(ATTACK)
            };
            gameObject.SendMessage(_updateInptMsg);
            _previous = _updateInptMsg.Current;
        }
    }
}