using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Dapper;

namespace Dapper
{
    public class crud:b_base
    {
        public crud()
        {
                
        }

        #region 手写Sql插入数据
        /// <summary>
        /// 手写Sql插入数据
        /// </summary>
        public int InsertWithSql()
        {
            using (var conn = Connection)
            {
                string _sql ="INSERT INTO t_department(departmentname,introduce,[enable])VALUES('应用开发部SQL','应用开发部主要开始公司的应用平台',1)";
                conn.Open();
                return conn.Execute(_sql);
            }
        }
        #endregion

        #region 实体插入数据
        /// <summary>
        /// 实体插入数据
        /// </summary>
        public int? InsertWithEntity()
        {
            using (var conn = Connection)
            {
                var _entity = new t_department { departmentname = "应用开发部ENTITY", introduce = "应用开发部主要开始公司的应用平台"};
                conn.Open();
                return conn.Insert(_entity);
            }
        }
        #endregion

        #region 在IDBconnection中使用事务
        /// <summary>
        /// 在IDBconnection中使用事务
        /// </summary>
        /// <returns></returns>
        public bool InsertWithTran()
        {
            using (var conn = Connection)
            {
               int _departmentid = 0, _employeeid = 0,_rnum=0;
                var _departmentname = new t_department { departmentname = "应用开发部ENTITY", introduce = "应用开发部主要开始公司的应用平台" };
                var _employee = new t_employee {displayname = "Micro",email ="1441299@qq.com",loginname ="Micro",password = "66778899",mobile = "123456789"};
                conn.Open();
                var _tran=conn.BeginTransaction();
                try
                {
                    _departmentid=conn.Insert(_departmentname, transaction: _tran).Value;
                    ++_rnum;
                    _employeeid = conn.Insert(_employee, transaction: _tran).Value;
                    ++_rnum;
                    conn.Insert(new t_derelation { departmentid = _departmentid, employeeid = _employeeid }, transaction: _tran);
                    ++_rnum;
                    _tran.Commit();
                }
                catch
                {
                    _rnum = 0;
                    _tran.Rollback();
                }
                return _rnum > 0;
            }
        }
        #endregion

        #region 在存储过程中使用事务
        /// <summary>
        /// 在存储过程中使用事务
        /// </summary>
        /// <returns></returns>
        public bool InsertWithProcTran()
        {
            var _parameter = new DynamicParameters();
            _parameter.Add("departmentname","外网开发部门");
            _parameter.Add("introduce","外网开发部门负责外部网站的更新");
            _parameter.Add("displayname","夏季冰点");
            _parameter.Add("loginname","Micro");
            _parameter.Add("password","123456789");
            _parameter.Add("mobile","1122334455");
            _parameter.Add("email","123456789@qq.com");
            using (var _conn = Connection)
            {
                _conn.Open();
                return
                    _conn.Query<bool>("p_Insertdata", _parameter, commandType: CommandType.StoredProcedure)
                        .FirstOrDefault();
            }
        }
        #endregion

        #region 查询所有员工信息方法一
        /// <summary>
        /// 查询所有员工信息方法一
        /// </summary>
        /// <returns></returns>
        public IEnumerable<t_employee> GetemployeeListFirst()
        {
            string _sql = "SELECT * FROM t_employee";
            using (var _conn = Connection)
            {
                _conn.Open();
                return _conn.Query<t_employee>(_sql);
            }
        }
        #endregion 

        #region 查询所有员工信息方法二
        /// <summary>
        /// 查询所有员工信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<t_employee> GetemployeeListSecond()
        {
            using (var _conn = Connection)
            {
                _conn.Open();
                return _conn.GetList<t_employee>();
            }
        }
        #endregion 

        #region 获取某位员工的信息方法一
        /// <summary>
        /// 获取某位员工的信息方法一
        /// </summary>
        /// <param name="employeeid"></param>
        /// <returns></returns>
        public t_employee GetemployeeFirst(int employeeid)
        {
            string _sql = "SELECT * FROM t_employee where employeeid=@pemployeeid";
            using (var _conn = Connection)
            {
                _conn.Open();
                return _conn.Query<t_employee>(_sql, new { pemployeeid = employeeid }).FirstOrDefault();
            }
        }
        #endregion 

        #region 获取某位员工的信息方法二
        /// <summary>
        /// 获取某位员工的信息方法二
        /// </summary>
        /// <param name="employeeid"></param>
        /// <returns></returns>
        public t_employee GetemployeetSecond(int employeeid)
        {
            using (var _conn = Connection)
            {
                _conn.Open();
                return _conn.Get<t_employee>(employeeid);
            }
        }
        #endregion 

        #region 获取某位员工的信息方法三
        /// <summary>
        /// 获取某位员工的信息方法三
        /// </summary>
        /// <param name="employeeid"></param>
        /// <returns></returns>
        public t_employee Getemployeethird(int pemployeeid)
        {
            using (var _conn = Connection)
            {
                _conn.Open();
                return _conn.GetList<t_employee>(new { employeeid = pemployeeid }).FirstOrDefault();
            }
        }
        #endregion

        #region 多表查询(获取部门&员工信息)
        /// <summary>
        /// 多表查询(获取部门&员工信息)
        /// </summary>
        public void GetMultiEntity()
        {
            string _sql = "SELECT * FROM t_department AS a;SELECT * FROM t_employee AS a";
            using (var _conn = Connection)
            {
                var _grid = _conn.QueryMultiple(_sql);
                var _department = _grid.Read<t_department>();
                var _employee = _grid.Read<t_employee>();
            }
        }
        #endregion

        #region 父子关系查询
        /// <summary>
        /// 父子关系查询
        /// </summary>
        public IEnumerable<t_department> GetPCEntity()
        {
            string _sql = "SELECT * FROM t_department AS a;SELECT * FROM t_employee AS a;SELECT * FROM t_derelation;";
            using (var _conn = Connection)
            {
                var _grid = _conn.QueryMultiple(_sql);
                var _department = _grid.Read<t_department>();
                var _employee = _grid.Read<t_employee>();
                var _derelation = _grid.Read<t_derelation>();
                foreach (var tDepartment in _department)
                {
                    tDepartment.ListEmployees = _employee.Join(_derelation.Where(v=>v.departmentid==tDepartment.departmentid),p=>p.employeeid,r=>r.employeeid,(p,r)=>p);
                }
                return _department;
            }
        }
        #endregion

        #region 简单分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pstart"></param>
        /// <param name="pend"></param>
        /// <returns></returns>
        public IEnumerable<t_employee> GetPaging(int pstart=0,int pend=5)
        {
            string _sql = "SELECT * FROM (SELECT a.*, ROW_NUMBER() OVER (ORDER BY a.employeeid) rownum FROM t_employee as a ) b WHERE b.rownum BETWEEN @start AND @end ORDER BY b.rownum";
            using (var _conn = Connection)
            {
                return _conn.Query<t_employee>(_sql, new {start = pstart, end = pend});
            }
        }
        #endregion

        #region 通用分页
        /// <summary>
        /// 通用分页
        /// </summary>
        /// <returns></returns>
        public int GetPaging()
        {
            ////实际开发可以独立出来处理/////////////
            var _ppaging = new p_PageList<t_employee>();
            _ppaging.Tables = "t_employee";
            _ppaging.OrderFields = "employeeid asc";
            ///////////////////////////////////////
            var _dy = new DynamicParameters();
            _dy.Add("Tables", _ppaging.Tables);
            _dy.Add("OrderFields", _ppaging.OrderFields);
            _dy.Add("TotalCount",dbType:DbType.Int32,direction: ParameterDirection.Output);
            using (var _conn= Connection)
            {
                _conn.Open();
                _ppaging.DataList=_conn.Query<t_employee>("p_PageList", _dy, commandType: CommandType.StoredProcedure);
            }
            _ppaging.TotalCount = _dy.Get<int>("TotalCount");
            return _ppaging.PageCount;
        }
        #endregion

        #region 存储过程Demo
        /// <summary>
        /// 存储过程Demo
        /// </summary>
        public Tuple<string,string> ProceDemo()
        {
            int employeeid = 1;
            var _mobile = "";
            var _dy = new DynamicParameters();
            _dy.Add("employeeid", employeeid);
            _dy.Add("displayname", string.Empty, dbType: DbType.String, direction: ParameterDirection.Output);
            using (var _conn = Connection)
            {
                _conn.Open();
                _mobile= _conn.Query<string>("p_Procedemo", _dy, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return Tuple.Create(_mobile, _dy.Get<string>("displayname"));
        }
        #endregion

    }
}
