using Loyufei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomuku
{
    public class GamingModel
    {
        private class DrawLine
        {
            public DrawLine(Documentary documentary, int delta)
            {
                Delta = delta;
                Documentary = documentary;
            }

            public int Delta { get; }
            public Documentary Documentary { get; }

            public bool Draw(EStoneType type, int id)
            {
                var line = new List<int>() { 0, 0, 0, 0, 1, 0, 0, 0, 0 };

                for (int radius = 1; radius <= 4; radius++)
                {
                    (line[4 - radius], line[4 + radius]) = (Check(type, id, radius));
                }
                
                return CheckLine(line);
            }

            private (int, int) Check(EStoneType type, int id, int radius)
            {
                var size    = Declarations.Size;
                var isOne   = Delta == 1 ? 0 : 1;
                var delta1  = (id + Delta * radius);
                var delta2  = (id - Delta * radius);
                var border1 = (id / size + isOne * radius) * size;
                var border2 = (id / size - isOne * radius) * size;
                
                bool IsClamp(int target, int header) 
                {
                    return target.IsClamp(header, header + size - 1);
                }

                var result1 = IsClamp(delta1, border1) ? Documentary.Query(type, delta1) : 0;
                var result2 = IsClamp(delta2, border2) ? Documentary.Query(type, delta2) : 0;

                return (result1, result2);
            }

            private bool CheckLine(List<int> line) 
            {
                if (line.Count < 9) return false;

                var continuous = 0;

                foreach(var spot in line) 
                {
                    if(spot == 0) 
                    {
                        continuous = 0;

                        continue;
                    }

                    continuous++;

                    if (continuous >= 5) return true;
                }

                return false;
            }
        }

        public GamingModel(Documentary documentary, Report report) 
        {
            Documentary = documentary;
            Report      = report;
            Lines       = new()
            {
                new (Documentary, 1),
                new (Documentary, 15),
                new (Documentary, 14),
                new (Documentary, 16),
            };
        }

        public Documentary    Documentary { get; }
        public Report         Report      { get; }
        public int            StepCount   { get; private set; }

        private List<DrawLine> Lines       { get; }

        public EStoneType Step(EStoneType type, int id) 
        {
            if (!Documentary.Record(type, id)) 
            {
                return EStoneType.None; 
            }

            StepCount++;
            
            foreach(var line in Lines) 
            {
                var end = line.Draw(type, id);
                
                if (end) 
                {
                    Report.Winner    = type;
                    Report.StepCount = StepCount;

                    return type; 
                }
            }

            return EStoneType.None;
        }

        public void Reset() 
        {
            StepCount = 0;

            Documentary.Reset();
        }
    }
}