using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SampleAppInstaller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var setup = new Setup()
                .CreatePackage("Documentation")
                .AddFiles("/Files/Documentation", "")
                .Exclude("*.tmp");
            
            base.OnStartup(e);
        }
    }
}
