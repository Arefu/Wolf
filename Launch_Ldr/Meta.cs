using System.Collections.Generic;

namespace Launch_Ldr
{
    public class Meta
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Injection_Addresses { get; set; }
        public List<string> Depends { get; set; }
    }
}
