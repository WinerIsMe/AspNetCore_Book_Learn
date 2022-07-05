using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Model
{
    /// <summary>
    /// 表格数据，支持分页
    /// </summary>
    public class TableModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; } = 200;
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool msg { get; set; } = false;
        /// <summary>
        /// 记录总数
        /// </summary>
        public string count { get; set; } = "服务器异常";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> data { get; set; }
    }
}
