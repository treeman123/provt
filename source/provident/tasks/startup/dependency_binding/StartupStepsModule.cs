using System;
using System.Collections.Generic;
using Ninject.Modules;
using System.Linq;
using provident.tasks.startup.steps;
using provident.utility;

namespace provident.tasks.startup.dependency_binding
{
  public class StartupStepsModule : NinjectModule
  {
    public override void Load()
    {
      all_startup_steps().for_each(step => Kernel.Bind(step).ToSelf().InTransientScope());

      Kernel.Bind<IAmTheFinalStepInStartup>().To<Launch<Shell>>().InTransientScope();
    }

    IEnumerable<Type> all_startup_steps()
    {
      return AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(potential_type => typeof(IRunAStartupStep).IsAssignableFrom(potential_type)
        && potential_type.IsClass);
    }
  }
}