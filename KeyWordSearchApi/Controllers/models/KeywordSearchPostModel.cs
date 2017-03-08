using System.Collections;
using System.Collections.Generic;

namespace KeyWordSearchApi.Controllers.models
{
    public class KeywordSearchPostModel
    {
        public IEnumerable<string> Uris { get; set; }
        public string Keyword { get; set; }
    }
}