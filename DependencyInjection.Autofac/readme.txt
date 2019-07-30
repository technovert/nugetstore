Follow these steps to use the package

1. Install the package from nuget
2. Open Global.asax.cs file
3. Add the following namespace
	using DependencyInjection.Autofac.content;
4. Add following line in Application_Start() method
	DependencyBuilder.Init();
5. Open the DependencyBuilder.cs file and add all your dependencies

	Done. You have succesfully added dependency injection to your project.
	