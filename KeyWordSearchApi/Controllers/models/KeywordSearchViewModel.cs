using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyWordSearchApi.Controllers.models
{
    public class KeywordSearchViewModel
    {
        public int TotalCount { get; set; }
        public string Keyword { get; set; }
        public List<Tuple<string,int>> HitsByUri { get; set; }

        public KeywordSearchViewModel()
        {
            HitsByUri = new List<Tuple<string, int>>();
        }
    }
}
