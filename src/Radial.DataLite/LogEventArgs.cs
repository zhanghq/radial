using System;
using System.Collections.Generic;
using System.Text;

namespace Radial.DataLite
{
    /// <summary>
    /// 日志事件参数
    /// </summary>
    public  class LogEventArgs:EventArgs
    {
        string _text;

        /// <summary>
        /// 初始化日志事件参数
        /// </summary>
        /// <param name="text">日志内容文本</param>
        public LogEventArgs(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text", "日志内容文本不能为空");
            _text = text.Trim();
        }

        /// <summary>
        /// 获取日志内容文本
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
        }
    }
}
