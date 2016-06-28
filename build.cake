#addin "Cake.Xamarin"

const string Solution = "XamarinFastlaneDemo.sln";
const string DroidProject = "XamarinFastlaneDemo.Droid";

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");

Task("Clean")
    .Does(() =>
{
    // Clean solution directories.
    var solutions = GetFiles("./**/*.sln");
    var solutionPaths = solutions.Select(solution => solution.GetDirectory());
    foreach(var path in solutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + configuration);
        CleanDirectories(path + "/**/obj/" + configuration);
    }
});

Task("Restore")
    .Does(() =>
{
    // Restore all NuGet packages.
    NuGetRestore("XamarinFastlaneDemo.sln");
});

// Task("Android").Does(() =>
// {
//     // build release, create apk, sign, and zipalign
//     AndroidPackage(
//         new FilePath("./XamarinFastlaneDemo.Droid/XamarinFastlaneDemo.Droid.csproj"),
//         true,
//         settings => { 
//             settings.SetConfiguration("Release");
//             settings.WithTarget("PackageForAndroid");
//             settings.SetVerbosity(Verbosity.Minimal);
//     });
// }).IsDependentOn("Clean")
//     .IsDependentOn("Restore");
    
Task("iOS").Does(() =>
{   
  DotNetBuild(new FilePath(string.Format("./{0}", Solution)), settings => {
        settings.Configuration = "Ad-Hoc";
        settings.WithProperty("Platform", "iPhone");
        settings.WithProperty("Target", "Build");
        settings.WithProperty("BuildIpa", "true");
        settings.WithProperty("IncludeITunesArtwork", "false");		
        settings.WithProperty("CodesignKey", "iPhone Developer: iOS Dev (7V257NZ2YW)");
        settings.WithProperty("CodesignProvision", "iOS Team Provisioning Profile: *");
        //settings.WithProperty("CodesignProvision", "3f0a2399-8bdd-4307-8fb9-14ed1c7ed645");
    });
 
}).IsDependentOn("Clean")
    .IsDependentOn("Restore");

RunTarget(target);