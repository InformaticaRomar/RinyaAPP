﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rinya_app.Calidad.Maestro
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        [WebMethod(EnableSession = true)]
        public static void hola() {
            int a = 0;
            a = 1;

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}