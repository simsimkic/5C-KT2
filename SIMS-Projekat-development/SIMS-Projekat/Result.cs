using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat
{
    public class Result<T>
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public T ReturnValue { get; set; } = default(T);
    }
}
