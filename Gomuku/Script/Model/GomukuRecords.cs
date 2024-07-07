using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;

namespace Gomuku
{
    public class GomukuRecords : EntityForm<int>
    {
        protected class Record : RepositBase<int, int> 
        {
            public Record(int id) : base(id, 0) 
            {

            }
        }

        public GomukuRecords(int size) : base(CreateRecords(size.Pow(2))) 
        {

        }

        public void Reset() 
        {
            this.OfType<IReposit>().ForEach(r => r.Preserve(0));
        }

        protected static IEnumerable<Record> CreateRecords(int length) 
        {
            var list = new List<Record>();

            for (int id = 0; id < length; id++) { list.Add(new Record(id)); }

            return list;
        }
    }
}