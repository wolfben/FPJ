using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using FPJ.Model;
using Dapper;

namespace FPJ.DAL
{

    public static class CustomerDapperExtensions
    {
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cnn">数据库连接对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="page">页码</param>
        /// <param name="psize">页数</param>
        /// <param name="param">参数</param>
        /// <param name="order">分页排序字段</param>
        /// <param name="isnext">是否只查询有下一页数据，默认false（不包括分页总数、总数等相关参数）</param>
        /// <returns></returns>
        public static Page<T> PageList<T>(this IDbConnection cnn, string sql, int page, int psize, dynamic param = null, string order = " order by (select null)", bool isnext = false)
        {
            string sqlpage, sqlcount = string.Empty;

            Page<T> pageList = new Page<T>
            {
                page = page,
                psize = psize
            };

            GetSql(sql, out sqlpage, out sqlcount, order, page, psize, isnext);

            pageList.Items = cnn.Query<T>(sqlpage, (object)param).ToList();

            if (!isnext)
            {
                pageList.totalcount = cnn.Query<int>(sqlcount, (object)param, null, true, null, CommandType.Text).FirstOrDefault();
                pageList.pagecount = (int)Math.Ceiling(((double)pageList.totalcount / psize));

                if (pageList.page > pageList.pagecount)
                {
                    pageList.page = pageList.pagecount;
                }
            }
            else
            {
                pageList.hasnext = pageList.Items.Count > psize ? 1 : 0;
                pageList.Items = pageList.Items.Take(psize).ToList();
            }


            return pageList;
        }

        /// <summary>
        /// 双表联合分页查询
        /// </summary>
        /// <typeparam name="TFirst">第一个表实体</typeparam>
        /// <typeparam name="TSecond">第二个表实体</typeparam>
        /// <typeparam name="TReturn">返回表实体</typeparam>
        /// <param name="cnn">数据库连接对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="map">实体模型绑定映射函数</param>
        /// <param name="page">页码</param>
        /// <param name="psize">页数</param>
        /// <param name="param">参数</param>
        /// <param name="order">分页排序字段</param>
        /// <param name="splitOn">多表查询时进行表分割的字段（默认Id）</param>
        /// <param name="isnext">是否只查询有下一页数据，默认false（不包括分页总数、总数等相关参数）</param>
        /// <returns></returns>
        public static Page<TReturn> PageList<TFirst, TSecond, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, int page, int psize, dynamic param = null, string order = " order by (select null)", string splitOn = "Id", bool isnext = false)
        {
            string sqlpage, sqlcount = string.Empty;

            Page<TReturn> pageList = new Page<TReturn>
            {
                page = page,
                psize = psize
            };

            GetSql(sql, out sqlpage, out sqlcount, order, page, psize, isnext);

            pageList.Items = cnn.Query<TFirst, TSecond, TReturn>(sqlpage, map, (object)param, splitOn: splitOn).ToList();

            if (!isnext)
            {
                pageList.totalcount = cnn.Query<int>(sqlcount, (object)param, null, true, null, CommandType.Text).FirstOrDefault();
                pageList.pagecount = (int)Math.Ceiling(((double)pageList.totalcount / psize));

                if (pageList.page > pageList.pagecount)
                {
                    pageList.page = pageList.pagecount;
                }
            }
            else
            {
                pageList.hasnext = pageList.Items.Count > psize ? 1 : 0;
                pageList.Items = pageList.Items.Take(psize).ToList();
            }


            return pageList;
        }

        /// <summary>
        /// 三表联合分页查询
        /// </summary>
        /// <typeparam name="TFirst">第一个表实体</typeparam>
        /// <typeparam name="TSecond">第二个表实体</typeparam>
        /// <typeparam name="TThird">第三个表实体</typeparam>
        /// <typeparam name="TReturn">返回表实体</typeparam>
        /// <param name="cnn">数据库连接对象</param>
        /// <param name="sql">sql语句</param>
        /// <param name="map">实体模型绑定映射函数</param>
        /// <param name="page">页码</param>
        /// <param name="psize">页数</param>
        /// <param name="param">参数</param>
        /// <param name="order">分页排序字段</param>
        /// <param name="splitOn">多表查询时进行表分割的字段（默认Id）</param>
        /// <param name="isnext">是否只查询有下一页数据，默认false（不包括分页总数、总数等相关参数）</param>
        /// <returns></returns>
        public static Page<TReturn> PageList<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, int page, int psize, dynamic param = null, string order = " order by (select null)", string splitOn = "Id", bool isnext = false)
        {
            string sqlpage, sqlcount = string.Empty;

            Page<TReturn> pageList = new Page<TReturn>
            {
                page = page,
                psize = psize
            };

            GetSql2(sql, out sqlpage, out sqlcount, order, page, psize, isnext);

            pageList.Items = cnn.Query<TFirst, TSecond, TThird, TReturn>(sqlpage, map, (object)param, splitOn: splitOn).ToList();

            if (!isnext)
            {
                pageList.totalcount = cnn.Query<int>(sqlcount, (object)param, null, true, null, CommandType.Text).FirstOrDefault();
                pageList.pagecount = (int)Math.Ceiling(((double)pageList.totalcount / psize));

                if (pageList.page > pageList.pagecount)
                {
                    pageList.page = pageList.pagecount;
                }
            }
            else
            {
                pageList.hasnext = pageList.Items.Count > psize ? 1 : 0;
                pageList.Items = pageList.Items.Take(psize).ToList();
            }


            return pageList;
        }


        /// <summary>
        /// 返回两个结果集。一个count 一个常规
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="orderby"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private static void GetSql(string sql, out string sqlpage, out string sqlcount, string orderby = "order by (select null)", int page = 1, int psize = 20, bool isnext = false)
        {

            string first, end;

            if (page < 1) page = 1;
            if (psize < 1) psize = 1;

            #region 查找from的正确位置 （跳过子查询）
            //查找from的位置
            var indexOfSelect = sql.IndexOf(" from ", StringComparison.OrdinalIgnoreCase);
            var selectField = sql.Substring(0, indexOfSelect);
            while (StringCount(selectField, "(") != StringCount(selectField, ")"))
            {
                indexOfSelect = sql.IndexOf(" from ", indexOfSelect + 1, StringComparison.OrdinalIgnoreCase);
                selectField = sql.Substring(0, indexOfSelect);
            }
            #endregion

            first = String.Format("select * from ({1}", psize, sql.Substring(0, indexOfSelect));
            end = sql.Substring(indexOfSelect, sql.Length - indexOfSelect);



            int pageStart = (psize * (page - 1)) + 1;
            int pageEnd = psize * page;

            if (isnext)
            {
                pageEnd += 1;
            }

            sqlpage =
               String.Format(@"
                        {0},row_number() 
                        over({1}) sman_row_number {2})
                            sman_table where sman_row_number BETWEEN {3} and {4} order by sman_row_number",
               first, orderby, end, pageStart, pageEnd
               );

            sqlcount = String.Format("set ROWCOUNT 1;select count(0) {0}", sql.Substring(indexOfSelect));

        }

        /// <summary>
        /// （优化）返回分页所需sql和分页总数sql
        /// </summary>
        /// <param name="sql">原sql</param>
        /// <param name="sqlpage">分页的sql语句</param>
        /// <param name="sqlcount">分页获取总数的sql语句</param>
        /// <param name="orderby">分页排序规则</param>
        /// <param name="page">页码</param>
        /// <param name="psize">页数</param>
        /// <param name="isnext"></param>
        private static void GetSql2(string sql, out string sqlpage, out string sqlcount, string orderby = "order by (select null)", int page = 1, int psize = 20, bool isnext = false)
        {
            if (page < 1) page = 1;
            if (psize < 1) psize = 1;

            //获取分页所需的分页sql语句
            var first = sql.Substring(0, sql.IndexOf("from", StringComparison.OrdinalIgnoreCase));
            var firstFromIndex = sql.IndexOf("from", StringComparison.OrdinalIgnoreCase);
            var end = sql.Substring(firstFromIndex);

            int pageStart = (psize * (page - 1)) + 1;
            int pageEnd = psize * page;

            if (isnext)
            {
                pageEnd += 1;
            }

            sqlpage =
               String.Format(@"
                        {0},row_number() 
                        over({1}) sman_row_number {2})
                            sman_table where sman_row_number BETWEEN {3} and {4} order by sman_row_number",
               first, orderby, end, pageStart, pageEnd
               );

            sqlcount = String.Format("set ROWCOUNT 1;select count(0) {0}", sql.Substring(firstFromIndex));

        }

        private static int StringCount(string value, string find, StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            int count = 0;
            int vlen = value.Length;
            int flen = find.Length;

            for (int i = 0; i < vlen - flen; i++)
            {
                if (value.Substring(i, flen).Equals(find, compare))
                {
                    count++;
                }
            }

            return count;
        }
    }
}