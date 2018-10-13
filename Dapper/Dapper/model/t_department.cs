using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper
{
    public  class t_department
    {
        [Key]
        public int departmentid { get; set;}
        public string departmentname { get; set; }
        public string introduce { get; set; }
        /// 1:启用,0:禁用
        public bool enable { get; set; }
        [Editable(false)]
        public IEnumerable<t_employee> ListEmployees { get; set;} 
    }
}
