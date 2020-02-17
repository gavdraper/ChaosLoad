using System.Collections.Generic;

namespace ChaosDemo
{
    public class ChaosScript
    {
        public string ConnectionString { get; set; }
        public TemplateType Type { get; set; } = TemplateType.Sql;
        public List<ChaosTemplate> Templates { get; set; }
    }

}
