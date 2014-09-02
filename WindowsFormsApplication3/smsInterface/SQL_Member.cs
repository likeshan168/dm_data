using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace smsInterface
{
    class SQL_Member
    {
        SqlCommand scom;
        SqlConnection scon;
        SqlDataAdapter sda;
        SqlBulkCopy sbc;
        SqlDataReader sdr;
        SqlTransaction tran;

        public SQL_Member()
        {
            scon = new SqlConnection("server=127.0.0.1;database=EMPOX_DM2;uid=sa;pwd=");
        }

        /// <summary>
        /// 初始化SqlCommand对象
        /// </summary>
        public void initSqlCommand()
        {
            scom = new SqlCommand();
            scom.Connection = scon;
        }

        /// <summary>
        /// 初始化SqlCommand对象
        /// </summary>
        /// <param name="sql">需要执行的SQL语句</param>
        public void initSqlCommand(string sql)
        {
            scom = new SqlCommand(sql, scon);
        }

        /// <summary>
        /// 初始化SqlDataAdapter对象
        /// </summary>
        /// <param name="sql">需要执行的SQL语句</param>
        public void initSqlDataAdapter(string sql)
        {
            sda = new SqlDataAdapter(sql, scon);
        }

        /// <summary>
        /// 打开数据库连接.失败时返回异常信息.
        /// </summary>
        public void openConnection()
        {
            try
            {
                if (scon.State != ConnectionState.Open)
                    scon.Open();
            }
            catch (Exception ex)
            { throw ex; };
        }

        /// <summary>
        /// 关闭数据库连接.
        /// </summary>
        public void closeConnection()
        {
            if (scon.State == ConnectionState.Open)
                scon.Close();
        }

        public object sqlExecuteScalar(string inputString)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand(inputString);
                return this.scom.ExecuteScalar();
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 具有占位符的单对象查询
        /// </summary>
        /// <param name="inputString">SQL查询语句</param>
        /// <param name="parameters">占位符信息表[占位符],[数值],[数据类型]</param>
        /// <returns>返回OBJECT对象</returns>
        public object sqlExecuteScalar(string inputString, DataTable parameters)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand(inputString);
                for (int i = 0; i < parameters.Rows.Count; i++)
                {
                    switch (parameters.Rows[i][2].ToString())
                    {
                        case "int":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Int).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "varchar":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.VarChar).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "datetime":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.DateTime).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "bit":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Bit).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "float":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Float).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "binary":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Binary).Value = parameters.Rows[i][1].ToString();
                            break;
                    }
                }
                return this.scom.ExecuteScalar();
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 以SqlDataAdapter形式返回结果集,适用于多列查询
        /// </summary>
        /// <param name="inputString">SQL语句</param>
        /// <returns>返回DataTable集合</returns>
        public DataTable sqlExcuteQueryTable(string inputString)
        {
            try
            {
                this.openConnection();
                this.initSqlDataAdapter(inputString);
                DataTable tempTable = new DataTable();
                this.sda.Fill(tempTable);
                return tempTable;
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 以SqlDataAdapter存储过程形式返回结果集
        /// </summary>
        /// <param name="inputString">SQL语句</param>
        /// <param name="parTable">输入类参数占位符信息表[占位符],[数值],[数据类型]</param>
        /// <param name="outTable">输出类参数占位符信息表[占位符],[数值],[数据类型]</param>
        /// <returns>返回DataTable集合</returns>
        public DataSet sqlExcuteQueryDataSet(string procText, DataTable parTable, DataTable outTable)
        {
            try
            {
                this.openConnection();
                //this.scon.ChangeDatabase(memberStatic.clientDataBase);
                this.initSqlCommand();
                this.scom.CommandText = procText;
                this.scom.CommandType = CommandType.StoredProcedure;
                this.sda = new SqlDataAdapter();
                this.sda.SelectCommand = this.scom;
                //设置输入参数
                for (int i = 0; i < parTable.Rows.Count; i++)
                {
                    switch (parTable.Rows[i][2].ToString())
                    {
                        case "int":
                            this.scom.Parameters.Add(parTable.Rows[i][0].ToString(), SqlDbType.Int).Value = parTable.Rows[i][1];
                            break;
                        case "varchar":
                            this.scom.Parameters.Add(parTable.Rows[i][0].ToString(), SqlDbType.VarChar).Value = parTable.Rows[i][1].ToString();
                            break;
                        case "datetime":
                            this.scom.Parameters.Add(parTable.Rows[i][0].ToString(), SqlDbType.DateTime).Value = parTable.Rows[i][1].ToString();
                            break;
                        case "bit":
                            this.scom.Parameters.Add(parTable.Rows[i][0].ToString(), SqlDbType.Bit).Value = parTable.Rows[i][1];
                            break;
                        case "float":
                            this.scom.Parameters.Add(parTable.Rows[i][0].ToString(), SqlDbType.Float).Value = parTable.Rows[i][1].ToString();
                            break;
                        case "binary":
                            this.scom.Parameters.Add(parTable.Rows[i][0].ToString(), SqlDbType.Binary).Value = parTable.Rows[i][1];
                            break;
                    }
                }
                //设置输出参数
                for (int i = 0; i < outTable.Rows.Count; i++)
                {
                    switch (outTable.Rows[i][1].ToString())
                    {
                        case "int":
                            this.scom.Parameters.Add(outTable.Rows[i][0].ToString(), SqlDbType.Int).Direction = ParameterDirection.Output;
                            break;
                        case "varchar":
                            this.scom.Parameters.Add(outTable.Rows[i][0].ToString(), SqlDbType.VarChar).Direction = ParameterDirection.Output;
                            break;
                        case "datetime":
                            this.scom.Parameters.Add(outTable.Rows[i][0].ToString(), SqlDbType.DateTime).Direction = ParameterDirection.Output;
                            break;
                        case "bit":
                            this.scom.Parameters.Add(outTable.Rows[i][0].ToString(), SqlDbType.Bit).Direction = ParameterDirection.Output;
                            break;
                        case "float":
                            this.scom.Parameters.Add(outTable.Rows[i][0].ToString(), SqlDbType.Float).Direction = ParameterDirection.Output;
                            break;
                        case "binary":
                            this.sda.SelectCommand.Parameters.Add(outTable.Rows[i][0].ToString(), SqlDbType.Binary).Direction = ParameterDirection.Output;
                            break;
                    }
                }
                DataSet rds = new DataSet();
                DataTable tempTable = new DataTable();
                this.sda.Fill(tempTable);
                rds.Tables.Add(tempTable);

                DataTable output = new DataTable();
                output.Columns.Add("valueCol", typeof(string));
                for (int i = 0; i < outTable.Rows.Count; i++)
                {
                    DataRow dr = output.NewRow();
                    dr[0] = this.scom.Parameters[outTable.Rows[i][0].ToString()].Value;
                    output.Rows.Add(dr);
                }
                rds.Tables.Add(output);
                return rds;
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 以SQLDATAREADER形式返回结果集,适用于单列查询
        /// </summary>
        /// <param name="inputString">SQL语句</param>
        /// <returns>返回ArrayList集合</returns>
        public ArrayList sqlExcuteQueryList(string inputString)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand(inputString);
                this.sdr = this.scom.ExecuteReader();
                ArrayList al = new ArrayList();
                if (this.sdr.Read())
                {
                    do
                    {
                        al.Add(this.sdr.GetValue(0));
                    } while (this.sdr.Read());
                }
                this.sdr.Close();
                return al;
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 返回INT值的SQL语句执行
        /// </summary>
        /// <param name="inputString">SQL语句</param>
        /// <returns>返回所影响的行数</returns>
        public int sqlExcuteNonQueryInt(string inputString)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand(inputString);
                int r = this.scom.ExecuteNonQuery();
                return r;
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// SQL语句执行,无返回值
        /// </summary>
        /// <param name="inputString">SQL语句</param>
        /// <param name="isProc">是否执行存储过程</param>
        public void sqlExcuteNonQuery(string inputString, bool isProc)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand(inputString);
                if (isProc)
                    this.scom.CommandType = CommandType.StoredProcedure;
                this.scom.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 带有占位符的SQL语句执行
        /// </summary>
        /// <param name="inputString">SQL语句</param>
        /// <param name="parameters">占位符信息表[占位符],[数值],[数据类型]</param>
        public void sqlExcuteNonQuery(string inputString, DataTable parameters)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand(inputString);
                for (int i = 0; i < parameters.Rows.Count; i++)
                {
                    switch (parameters.Rows[i][2].ToString())
                    {
                        case "int":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Int).Value = parameters.Rows[i][1];
                            break;
                        case "varchar":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.VarChar).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "datetime":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.DateTime).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "bit":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Bit).Value = parameters.Rows[i][1];
                            break;
                        case "float":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Float).Value = parameters.Rows[i][1].ToString();
                            break;
                        case "binary":
                            this.scom.Parameters.Add(parameters.Rows[i][0].ToString(), SqlDbType.Binary).Value = parameters.Rows[i][1];
                            break;
                    }
                }
                this.scom.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 多SQL语句执行
        /// </summary>
        /// <param name="al">SQL语句集合</param>
        /// <param name="isTrans">是否启用SQL事物</param>
        public void sqlExcuteNonQuery(ArrayList al, bool isTrans)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand();
                if (isTrans)
                {
                    this.tran = this.scon.BeginTransaction();
                    for (int i = 0; i < al.Count; i++)
                    {
                        this.scom.CommandText = al[i].ToString();
                        this.scom.Transaction = this.tran;
                        this.scom.ExecuteNonQuery();
                    }
                    this.tran.Commit();
                }
                else
                {
                    for (int i = 0; i < al.Count; i++)
                    {
                        this.scom.CommandText = al[i].ToString();
                        this.scom.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                if (isTrans)
                    this.tran.Rollback();
                throw ex;
            }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 多SQL语句执行含带事务,存储过程
        /// </summary>
        /// <param name="dt">SQL语句表.第二列为语句是否按存储过程执行</param>
        /// <param name="isTrans">是否启用SQL事物</param>
        public void sqlExcuteNonQuery(DataTable dt, bool isTrans)
        {
            try
            {
                this.openConnection();
                this.initSqlCommand();
                if (isTrans)
                {
                    this.tran = this.scon.BeginTransaction();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        this.scom.CommandText = dt.Rows[i][0].ToString();
                        this.scom.Transaction = this.tran;
                        if (dt.Rows[i][1].ToString() == "True")
                            this.scom.CommandType = CommandType.StoredProcedure;
                        this.scom.ExecuteNonQuery();
                    }
                    this.tran.Commit();
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        this.scom.CommandText = dt.Rows[i][0].ToString();
                        if (dt.Rows[i][1].ToString() == "True")
                            this.scom.CommandType = CommandType.StoredProcedure;
                        this.scom.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                if (isTrans)
                    this.tran.Rollback();
                throw ex;
            }
            finally { this.closeConnection(); }
        }

        /// <summary>
        /// 整表导入数据库
        /// </summary>
        /// <param name="tableName">目标表名</param>
        /// <param name="innerTable">源数据集</param>
        public void sqlExcuteBulkCopy(string tableName, DataTable innerTable)
        {
            try
            {
                this.openConnection();
                sbc = new SqlBulkCopy(this.scon);
                sbc.DestinationTableName = tableName;
                sbc.BulkCopyTimeout = 3600;
                sbc.WriteToServer(innerTable);
            }
            catch (Exception ex)
            { throw ex; }
            finally { this.closeConnection(); }
        }
    }
}
