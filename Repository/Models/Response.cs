

using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public class Response
    {
        public Response()
        {
            Rates = new List<Rate>();
        }

        bool _success;
        public bool Success
        {
            get { return _success; }
            set { this._success = value; }
        }

        DateTime _timeStamp;
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { this._timeStamp = value; }
        }

        public List<Rate> Rates { get; set; }

    }

    public class Rate
    {
        string _currency;
        public string Currency
        {
            get { return _currency; }
            set { this._currency = value; }
        }

        string _code;
        public string Code
        {
            get { return _code; }
            set { this._code = value; }
        }

        float _bid;
        public float Bid
        {
            get { return _bid; }
            set { this._bid = value; }
        }

        float _ask;
        public float Ask
        {
            get { return _ask; }
            set { this._ask = value; }
        }
    }
}
