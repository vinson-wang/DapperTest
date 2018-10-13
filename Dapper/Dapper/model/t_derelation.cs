using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper
{
    public class t_derelation
    {
        [Key]
        public int relationid { get; set;}
        public int employeeid { get; set;}
        public int departmentid {get; set;}
    }
}
