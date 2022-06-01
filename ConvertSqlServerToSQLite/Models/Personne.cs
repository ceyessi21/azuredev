using System;
using System.Collections.Generic;



namespace ConvertSqlServerToSQLite.Models
{
    public partial class Personne
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int Old { get; set; }
    }
}
