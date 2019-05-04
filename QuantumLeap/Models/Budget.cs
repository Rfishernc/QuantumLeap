using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumLeap.Models
{
    public class Budget
    {
        public double Capital { get; set; }

        public bool LeapIsAuthorized(Leap nextLeap)
        {
            if (Capital >= nextLeap.Cost)
            {
                return true;
            }
            return false;
        }
    }
}
