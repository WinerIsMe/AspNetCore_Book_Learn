using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Model.Models
{
    public class RootEntityTKey<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// ID 泛型主键 TKey
        /// </summary>
        public TKey Id { get; set; }    
    }
}
