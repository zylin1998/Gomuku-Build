using System.Collections;
using System.Collections.Generic;
using Zenject;
using Loyufei;
using Loyufei.ViewManagement;

namespace Gomuku
{
    public class GomukuView : MenuBase, IUpdateGroup
    {
        public IEnumerable<IUpdateContext> Contexts 
            => GetComponentsInChildren<IUpdateContext>();

        public List<StoneListener> Stones { get; } = new();

        [Inject]
        private void Construct(StonePool stonePool) 
        {
            var length = Declarations.Size.Pow(2);

            for (var id = 0; id < length; id++) { Stones.Add(stonePool.Spawn(id)); }
        }
    }
}