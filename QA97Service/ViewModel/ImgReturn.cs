using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.ViewModel
{
  public  class ImgReturn
    {
        public string imgUrl { get; set; }
        public HttpStatusCode response { get; set; }
        public string msg { get; set; }

    }
}
