using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Data;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 文本命令数据类
    /// </summary>
    public class TextCommandData
    {
        string _cmdText;
        object[] _paramValues;
 
        static int S_UniqueParameterIndex = 0;

        /// <summary>
        /// 获取顺序参数查询语句匹配模式
        /// </summary>
        const string SequenceParametersQueryPattern = @"\{\d+\}";

        /// <summary>
        /// 获取动态参数查询语句匹配模式
        /// </summary>
        const string DynamicParametersQueryPattern = @"\{\?\}";
        
        /// <summary>
        /// 获取组参数查询语句匹配模式
        /// </summary>
        const string GroupParametersQueryPattern = @"\{...\}";

        /// <summary>
        /// 初始化文本命令数据对象
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数值</param>
        public TextCommandData(string cmdText, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(cmdText))
                throw new ArgumentNullException("cmdText", "命令文本不能为空");

            _cmdText = cmdText;
            _paramValues = paramValues;

            if (ContainsGroupParametersQuery(cmdText))
            {
                if (paramValues == null)
                    _cmdText = ConvertGroupParametersQueryToSequenceParametersQuery(cmdText, 0);
                else
                    _cmdText = ConvertGroupParametersQueryToSequenceParametersQuery(cmdText, paramValues.Length);
            }
            else
            {
                if (ContainsDynamicParametersQuery(cmdText))
                    _cmdText = ConvertDynamicParametersQueryToSequenceParametersQuery(cmdText);
            }
        }

        #region Check Contains

        /// <summary>
        /// 是否包含动态参数查询
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <returns>包含返回Ture，否则返回False</returns>
        private bool ContainsDynamicParametersQuery(string cmdText)
        {
            Regex regex = new Regex(DynamicParametersQueryPattern);
            return regex.IsMatch(cmdText);
        }

        /// <summary>
        /// 是否包含组参数查询语句
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <returns>包含返回Ture，否则返回False</returns>
        private bool ContainsGroupParametersQuery(string cmdText)
        {
            Regex regex = new Regex(GroupParametersQueryPattern);
            MatchCollection matches = regex.Matches(cmdText);

            if (matches.Count == 1)
                return true;
            else
            {
                if (matches.Count > 1)
                    throw new NotSupportedException("不支持包含多个组参数的查询");
                return false;
            }
        }

        /// <summary>
        /// 是否包含顺序参数查询语句
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <returns>包含返回Ture，否则返回False</returns>
        private bool ContainsSequenceParametersQuery(string cmdText)
        {
            Regex regex = new Regex(SequenceParametersQueryPattern);
            return regex.IsMatch(cmdText);
        }

        #endregion

        /// <summary>
        /// 获取命令文本
        /// </summary>
        public string CommandText
        {
            get
            {
                return _cmdText;
            }
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        public object[] ParameterValues
        {
            get
            {
                return _paramValues;
            }
        }

        #region Parameter Index Query Format

        /// <summary>
        /// 将组参数查询语句转换为顺序参数查询语句
        /// </summary>
        /// <param name="groupParametersQuery">组参数查询语句</param>
        /// <param name="parameterCount">组参数数量</param>
        /// <returns>动态参数查询语句</returns>
        private string ConvertGroupParametersQueryToSequenceParametersQuery(string groupParametersQuery, int parameterCount)
        {
            if (parameterCount < 1)
                throw new ArgumentException("组参数查询至少需要提供一个参数");

            string outString = groupParametersQuery;

            //check if contains SequenceParameterQuery
            if (ContainsSequenceParametersQuery(outString))
                throw new NotSupportedException("不支持顺序参数和组参数并存的查询");

            //check if contains DynamicParameterQuery
            if (ContainsDynamicParametersQuery(outString))
                throw new NotSupportedException("不支持动态参数和组参数并存的查询");

            List<string> dpList = new List<string>();

            for (int i = 0; i < parameterCount; i++)
                dpList.Add("{" + i.ToString() + "}");

            outString = outString.Replace("{...}", string.Join(",", dpList.ToArray()));

            return outString;
        }

        /// <summary>
        /// 将动态参数查询语句转换为顺序参数查询语句
        /// </summary>
        /// <param name="dynamicParametersQuery">动态参数查询语句</param>
        /// <returns>顺序参数查询语句</returns>
        private string ConvertDynamicParametersQueryToSequenceParametersQuery(string dynamicParametersQuery)
        {
            string outString = dynamicParametersQuery;

            //check if contains SequenceParameterQuery
            if (ContainsSequenceParametersQuery(dynamicParametersQuery))
                throw new NotSupportedException("不支持动态参数和顺序参数并存的查询");

            //check if contains GroupParameterQuery
            if (ContainsGroupParametersQuery(dynamicParametersQuery))
                throw new NotSupportedException("不支持动态参数和组参数并存的查询");

            int paramIndex = 0;
            int positionIndex = 0;

            while ((positionIndex = outString.IndexOf("{?}")) >= 0)
            {
                outString = outString.Remove(positionIndex + 1, 1);
                outString = outString.Insert(positionIndex + 1, paramIndex.ToString());

                paramIndex++;
            }

            return outString;
        }

        #endregion

        #region Parameters

        /// <summary>
        /// 向DbCommand添加参数
        /// </summary>
        /// <param name="command">DbCommand对象</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        private void AppendParameter(DbCommand command, string paramName, object paramValue)
        {
            if (command == null)
                throw new ArgumentNullException("command","DbCommand对象不能为空");
            if (string.IsNullOrEmpty(paramName))
                throw new ArgumentNullException("paramName", "paramName不能为空");


            DbParameter param = command.CreateParameter();
            param.ParameterName = paramName;
            if (paramValue == null)
                paramValue = DBNull.Value;
            param.Value = paramValue;

            command.Parameters.Add(param);
        }

        /// <summary>
        /// 创建具有唯一性的参数索引
        /// </summary>
        /// <returns>具有唯一性的参数索引</returns>
        private int CreateUniqueParameterIndex()
        {
            int currentIndex = 0;
            lock (typeof(TextCommandData))
            {
                if (S_UniqueParameterIndex == int.MaxValue)
                    S_UniqueParameterIndex = 0;
                currentIndex = S_UniqueParameterIndex++;
            }

            return currentIndex;
        }

        #endregion

        #region Create DbCommand

        /// <summary>
        /// 创建DbCommand对象
        /// </summary>
        /// <param name="provider">DbProviderFactory对象</param>
        /// <param name="parameterNameBuilder">参数名称构建对象</param>
        /// <returns>DbCommand对象</returns>
        public DbCommand CreateCommand(DbProviderFactory provider, ParameterNameBuilder parameterNameBuilder)
        {
            return CreateCommand(provider, parameterNameBuilder, false);
        }

        /// <summary>
        /// 创建DbCommand对象
        /// </summary>
        /// <param name="provider">DbProviderFactory对象</param>
        /// <param name="parameterNameBuilder">参数名称构建对象</param>
        /// <param name="useUniqueParameterIndex">是否使用唯一性的参数索引</param>
        /// <returns>DbCommand对象</returns>
        public DbCommand CreateCommand(DbProviderFactory provider, ParameterNameBuilder parameterNameBuilder, bool useUniqueParameterIndex)
        {
            if (provider == null)
                throw new ArgumentNullException("provider", "DbProviderFactory对象不能为空");
            if (parameterNameBuilder == null)
                throw new ArgumentNullException("parameterNameBuilder", "参数名称构建对象不能为空");

            DbCommand cmd = provider.CreateCommand();

            string tempCmdText = _cmdText;

            if (ParameterValues != null && ParameterValues.Length > 0)
            {
                Regex regex = new Regex(SequenceParametersQueryPattern);

                MatchCollection matchs = regex.Matches(tempCmdText);

                List<string> matchList = new List<string>();

                foreach (Match mat in matchs)
                {
                    if (!matchList.Contains(mat.Value))
                        matchList.Add(mat.Value);
                }

                int matchCount = matchList.Count;

                if (matchCount != ParameterValues.Length)
                    throw new InvalidOperationException(string.Format("提供的参数数量({0})和实际所需的数量({1})不一致", ParameterValues.Length, matchCount));

                List<string> paramNameList = new List<string>();

                for (int i = 0; i < ParameterValues.Length; i++)
                {
                    string paramName = parameterNameBuilder(i);
                    if (useUniqueParameterIndex)
                        paramName = parameterNameBuilder(CreateUniqueParameterIndex());
                    AppendParameter(cmd, paramName, ParameterValues[i]);
                    paramNameList.Add(paramName);
                }
                tempCmdText = string.Format(_cmdText, paramNameList.ToArray());
            }

            cmd.CommandText = tempCmdText;

            return cmd;
        }

        #endregion
    }
}
