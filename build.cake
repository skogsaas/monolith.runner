var target = Argument("target", "Default");
var configuration = Argument("buildconfiguration", "Release");
var source = Argument("buildsource", "Z:/NuGet");
var nugetConfig = "./nuget.config";
var packages = "./packages";
var artifacts = "./artifacts/";
var solution = "./Runner.sln";
var project = "./Runner/Runner.csproj";

var nugetRestoreSettings = new NuGetRestoreSettings
	{
		ConfigFile = nugetConfig,
		PackagesDirectory = packages
	};

var nugetUpdateSettings = new NuGetUpdateSettings
	{
		//ConfigFile = nugetConfig
	};

var nugetPackSettings = new NuGetPackSettings
	{
		OutputDirectory = artifacts
	};

var nugetPushSettings = new NuGetPushSettings
	{
		Source = source
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
	NuGetRestore(project, nugetRestoreSettings);
});

Task("Update")
	.IsDependentOn("Restore")
	.Does(() =>
{
	NuGetUpdate(project, nugetUpdateSettings);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.IsDependentOn("Update")
	.Does(() =>
{
	MSBuild(project, msbuildSettings);
});

Task("Package")
	.IsDependentOn("Build")
	.Does(() =>
{
	NuGetPack(project, nugetPackSettings);
});

Task("Push")
	.IsDependentOn("Package")
	.Does(() =>
{
	var nupkgs = GetFiles(System.IO.Path.Combine(artifacts, "*.nupkg"));

	NuGetPush(nupkgs, nugetPushSettings);
});

Task("Default")
	.IsDependentOn("Build")
	.Does(() =>
{
  
});

RunTarget(target);
