
var target = Argument("target", "ExecuteBuild");
var configuration = Argument("configuration", "Release");
var project = "G:\\Cursos\\RecommendedMarket";
//var project = "C:\\Users\\Gustavo Barbosa\\Documents\\Cursos\\RecommendedMarket";
var projectTest = "G:\\Cursos\\RecommendedMarket\\RM.Test";
var outputFolder = "C:\\Users\\gutof\\OneDrive\\Área de Trabalho\\Teste_de_automacao_Cake\\Deploy\\RM";


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        CleanDirectory(outputFolder);        
    });

Task("Restore")
    .Does(() => {
        DotNetRestore(project);
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetBuild(project, new DotNetBuildSettings
        {
            NoRestore = true,
            Configuration = configuration
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetTest(projectTest, new DotNetTestSettings
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true
        });
    });

Task("Publish")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetPublish(project, new DotNetPublishSettings
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