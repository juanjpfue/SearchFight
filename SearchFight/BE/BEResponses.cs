using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GoogleResponse
    {
        public String kind { get; set; }

        public g1 queries { get; set;  }

    }

    public class g1
    { 
        public List<g2> request { get; set; }
    }

    public class g2
    {
        public long totalResults { get; set; }
        
    }

    public class BingResponse
    {
        public String _type { get; set; }
        public BingContent webPages { get; set; }
    }

    public class BingContent
    { 
        public long totalEstimatedMatches { get; set; }
    }
}
