﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EIRLSSAssignment1.Startup))]
namespace EIRLSSAssignment1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
