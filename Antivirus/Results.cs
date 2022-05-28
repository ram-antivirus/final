using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus
{
    public class Results
    {
        private int total_scanned;
        public int TotalScanned
        {
            get { return this.total_scanned; }
            set
            {
                if (this.total_scanned != value)
                {
                    this.total_scanned = value;                   
                }
            }
        }
        private int total_issues;
        public int TotalIssues
        {
            get { return this.total_issues; }
            set
            {
                if (this.total_issues != value)
                {
                    this.total_issues = value;
                }
            }
        }
    }
}
