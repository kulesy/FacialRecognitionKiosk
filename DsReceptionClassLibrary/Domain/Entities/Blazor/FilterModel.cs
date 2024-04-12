using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.Domain.Entities.Blazor
{
    public class FilterModel
    {
        private DateTime _startDate;
        private DateTime _endDate;

        [Display(Name = "Start Date")]
        public DateTime StartDate
        {
            get
            {
                if (_startDate == DateTime.MinValue)
                {
                    return DateTime.Now;
                }
                else
                {
                    return _startDate;
                }
            }
            set
            {
                _startDate = value;
            }
        }

        [Display(Name = "End Date")]
        public DateTime EndDate
        {
            get
            {
                if (_endDate == DateTime.MinValue)
                {
                    return DateTime.Now;
                }
                else
                {
                    return _endDate;
                }
            }
            set
            {
                if (value < _startDate)
                {
                    _endDate = _startDate;
                }
                else
                {
                    _endDate = value;
                }
            }
        }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
    }
}
