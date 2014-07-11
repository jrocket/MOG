using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models.TimeLine
{
    public class Asset
    {
        public string media { get; set; }
        public string credit { get; set; }
        public string caption { get; set; }
        public string thumbnail { get; set; }
    }



    public class Date
    {
        private DateTime _startDate;
        public DateTime StartDate
        {
            set
            {
                _startDate = value;
            }
        }
        public string startDate
        {
            get
            {

                return _startDate.ToString("yyyy,MM,dd,hh,mm");
            }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            set
            {
                _endDate = value;
            }
        }
        public string endDate
        {
            get
            {

                return _endDate.ToString("yyyy,MM,dd,hh,mm");
            }
        }
        public string headline { get; set; }
        public string text { get; set; }
        public string tag { get; set; }
        public string classname { get; set; }
        public Asset asset { get; set; }
    }

    public class Era
    {
        private DateTime _startDate;
        public DateTime StartDate
        {
            set
            {
                _startDate = value;
            }
        }
        public string startDate
        {
            get
            {

                return _startDate.ToString("yyyy,MM,dd");
            }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            set
            {
                _endDate = value;
            }
        }
        public string endDate
        {
            get
            {

                return _endDate.ToString("yyyy,MM,dd");
            }
        }
        public string headline { get; set; }
        public string tag { get; set; }
    }

    public class Chart
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string headline { get; set; }
        public string value { get; set; }
    }

    public class Timeline
    {
        public string headline { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public Asset asset { get; set; }
        public List<Date> date { get; set; }
        public List<Era> era { get; set; }
        public List<Chart> chart { get; set; }
    }

    public class Root
    {
        public Timeline timeline { get; set; }
    }
}
