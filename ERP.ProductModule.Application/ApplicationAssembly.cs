using System.Reflection;

namespace ERP.ProductModule.Application;

public static class ApplicationAssembly
{
    public static Assembly Value => typeof(ApplicationAssembly).Assembly;
}
