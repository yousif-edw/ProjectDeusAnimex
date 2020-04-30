using Assets.Ancible_Tools.Scripts.Hitbox;
using UnityEngine;

namespace Assets.Resources.Scripts.Game_System.Controllers
{
    public class CollisionLayerFactory : MonoBehaviour
    {
        private static CollisionLayerFactory _instance = null;

        public static CollisionLayer AGGRO { get; private set; }

        [SerializeField] private CollisionLayer _aggro;

        void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            SetupStatics();
        }

        private void SetupStatics()
        {
            AGGRO = _aggro;
        }
    }
}