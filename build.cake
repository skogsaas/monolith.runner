var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetConfig = "./nuget.config";
var artifacts = "./artifacts/";
var solution = "./Runner.sln";

var nugetRestoreSettings = new NuGetRestoreSettings
	{
		ConfigFile = nugetConfig
	};

var nugetUpdateSettings = new NuGetUpdateSettings
	{
		//ConfigFile = nugetConfig
	};

var msbuildSettings = new MSBuildSettings 
	{
		Verbosity = Verbosity.Minimal,
		ToolVersion = MSBuildToolVersion.VS2015,
		Configuration = configuration,
		PlatformTarget = PlatformTarget.MSIL
	};

Task("Clean")
	.Does(() =>
{
	CleanDirectory(artifacts);
});

Task("Restore")
	.IsDependentOn("Clean")
	.Does(() => 
{
	NuGetRestore(solution, nugetRestoreSettings);
});

Task("Update")
	.IsDependentOn("Restore")
	.Does(() =>
{
	NuGetUpdate(solution, nugetUpdateSettings);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.IsDependentOn("Update")
	.Does(() =>
{
	MSBuild(solution, msbuildSettings);
});

Task("Default")
	.IsDependentOn("Build")
	.Does(() =>
{
  
});

RunTarget(target);
