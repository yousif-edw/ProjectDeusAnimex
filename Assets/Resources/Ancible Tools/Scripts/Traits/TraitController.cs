using UnityEngine;

namespace Assets.Ancible_Tools.Scripts.Traits
{
    public class TraitController : MonoBehaviour
    {
        public Trait Trait { get; private set; }

        public void Setup(Trait trait)
        {
            Trait = Instantiate(trait, transform);
            Trait.name = trait.name;
            name = $"{Trait.name} Controller";
            Trait.SetupController(this);
        }

        public void Destroy()
        {
            Trait.Destroy();
            Destroy(Trait);
        }
    }
}
