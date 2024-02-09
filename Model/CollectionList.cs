using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hv.Sum200.Stargazer.Model
{
    public class CollectionList
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
