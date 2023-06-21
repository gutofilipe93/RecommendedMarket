
var target = Argument("target", "ExecuteBuild");
var configuration = Argument("configuration", "Release");
var project = ".";
//var project = "C:\\Users\\Gustavo Barbosa\\Documents\\Cursos\\RecommendedMarket";
var projectTest = "./RM.Test/RM.Test";
var outputFolder = "Public";


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        CleanDirectory(outputFolder);        
    });

Task("Restore")
    .Does(() => {
        DotNetCoreRestore(project);
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild(project, new DotNetCoreBuildSettings
        {
            NoRestore = true,
            Configuration = configuration
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest(project, new DotNetCoreTestSettings
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true
        });
    });

Task("Publish")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePublish(project, new DotNetCorePublishSettings
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true,
            OutputDirectory = outputFolder
        });
    });


Task("ExecuteBuild")
    .IsDependentOn("Publish");    

RunTarget(target);