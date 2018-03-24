using System;
using System.Reflection;
using Acr.Settings.Tests;
using Xunit.Runners.UI;


namespace Acr.Settings.Uwp.Tests
{
    sealed partial class App : RunnerApplication
    {
        protected override void OnInitializeRunner()
        {
            this.AddTestAssembly(typeof(DefaultSettingTests).GetTypeInfo().Assembly);
        }
    }
}
