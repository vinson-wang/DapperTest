using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace Dapper
{

    public class entitymapper : b_base
    {
        public void entitymapperdemo()
        {
            var _employee = new t_employee {displayname = "Micro",email ="1441299@qq.com",loginname ="Micro",password = "66778899",mobile = "123456789",address = "我的地址，不能添加"};
            using (var _conn =Connection)
            {
                _conn.Open();
                _conn.Insert(_employee);
            }
        }
    }
}
