using Loyufei;
using System.Collections;
using System.Collections.Generic;

namespace Gomuku
{
    public class Documentary
    {
        public Documentary() 
        {
            Records = new()
            {
                { EStoneType.Black, _Black },
                { EStoneType.White, _White }
            };
        }

        private GomukuRecords _Black = new(Declarations.Size);
        private GomukuRecords _White = new(Declarations.Size);

        public Dictionary<EStoneType, GomukuRecords> Records { get; }

        public bool Record(EStoneType type, int id) 
        {
            var reposit   = Records[type][id].To<IReposit>();

            if (reposit.IsDefault()) { return false; }

            var carRecord = Equals(reposit.Data, 0);

            if (carRecord) { reposit.Preserve(1); }

            return carRecord;
        }

        public int Query(EStoneType type, int id) 
        {
            var reposit = Records[type][id].To<IReposit>();

            return reposit.IsDefault() ? 0 : reposit.Data.To<int>();
        }

        public void Reset() 
        {
            _Black.Reset();
            _White.Reset();
        }
    }
}