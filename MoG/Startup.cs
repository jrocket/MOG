﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoG.Startup))]
namespace MoG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}