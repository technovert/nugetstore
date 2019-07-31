using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ORM.Core
{
    /// <summary>
    /// Extension class
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// The reg parameters
        /// </summary>
        private static readonly Regex regParameters = new Regex(@"@\w+", RegexOptions.Compiled);

        /// <summary>
        /// Datas the reader map to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr">The dr.</param>
        /// <returns>The list of data.</returns>
        public static List<T> DataReaderMapToList<T>(this IDataReader dr) where T: class
        {
           
            List<T> list = new List<T>();
            T obj = default(T);
            try
            {
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    list.Add(obj);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return list;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="parameters">The parameters.</param>
        public static void AddParam(this IDbCommand cmd, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                try
                {
                    MatchCollection cmdParams = regParameters.Matches(cmd.CommandText);
                    List<String> param = new List<String>();
                    foreach (var el in cmdParams)
                    {
                        if (!param.Contains(el.ToString()))
                        {
                            param.Add(el.ToString());
                        }
                    }
                    Int32 i = 0;
                    IDbDataParameter dp;
                    foreach (String el in param)
                    {
                        dp = cmd.CreateParameter();
                        dp.ParameterName = el;
                        dp.Value = parameters[i++].GetType().GetProperty(el.Replace("@", string.Empty)).GetValue(parameters[i++], null);
                        cmd.Parameters.Add(dp);
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
