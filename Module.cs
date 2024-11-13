using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using SDG.Framework.Modules;
using SDG.Unturned;

namespace FixLibrary;

[UsedImplicitly]
public class Module : IModuleNexus
{
    private ConcurrentDictionary<string, Assembly>? _loadedAssemblies;
    private List<Library>? _libraries;
    private const string DiscordSupportUrl = "https://discord.gg/2yG5t869uq";
    private const string GitHubUrl = "https://github.com/UnturnedCommunity/FixLibrary";

    public void initialize()
    {
        var assembly = typeof(Module).Assembly;
        var assemblyName = assembly.GetName();

        CommandWindow.Log($"Loading FixLibrary... {assemblyName.Name} {assemblyName.Version} this could take for a while!");
        CommandWindow.Log($"Discord Support: {DiscordSupportUrl}");
        CommandWindow.Log($"GitHub: {GitHubUrl}");

        var moduleDirectory = string.Empty;
        var assemblyFileName = string.Empty;

        foreach (var module in ModuleHook.modules)
        {
            // ReSharper disable once InvertIf
            if (module.assemblies is { Length: > 0 } && module.assemblies[0] == assembly)
            {
                moduleDirectory = Path.GetFullPath(module.config.DirectoryPath);
                assemblyFileName = Path.GetFileName(module.config.Assemblies[0].Path);
                break;
            }
        }

        if (string.IsNullOrEmpty(moduleDirectory))
        {
            throw new Exception("Failed to get FixLibrary module directory");
        }

        _loadedAssemblies = [];
        _libraries = Directory
            .GetFiles(moduleDirectory, "*.dll", SearchOption.TopDirectoryOnly)
            .Where(x => !Path.GetFileName(x).Equals(assemblyFileName, StringComparison.OrdinalIgnoreCase))
            .Select(GetLibrary)
            .ToList();
        ModuleHook.PreVanillaAssemblyResolve += UnturnedPreVanillaAssemblyResolve;

        foreach (var library in _libraries)
        {
            if (_loadedAssemblies.ContainsKey(library.Name))
            {
                // Loaded on Assembly Resolve
                continue;
            }

            CommandWindow.Log($"LOADED: {library.Name} {library.Path}");

            var loadedAssembly = Assembly.Load(library.Content, library.Symbols);
            _loadedAssemblies.TryAdd(loadedAssembly.GetName().Name, loadedAssembly);
        }

        CommandWindow.Log("Loaded FixLibrary!");
    }
    public void shutdown()
    {
        _libraries?.Clear();
        _libraries = null;
        _loadedAssemblies?.Clear();
        _loadedAssemblies = null;
        ModuleHook.PreVanillaAssemblyResolve -= UnturnedPreVanillaAssemblyResolve;
    }

    private static Library GetLibrary(string assemblyFilePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(assemblyFilePath);
        var symbolsFilePath = Path.ChangeExtension(assemblyFilePath, "pdb");
        var symbols = File.Exists(symbolsFilePath) ? File.ReadAllBytes(symbolsFilePath) : null;
        var content = File.ReadAllBytes(assemblyFilePath);
        return new Library
        {
            Path = assemblyFilePath,
            Name = fileName,
            Content = content,
            Symbols = symbols,
        };
    }
    private Assembly? UnturnedPreVanillaAssemblyResolve(object sender, ResolveEventArgs args)
    {
        var assembly = FindLoadedAssembly(args)
                       ?? TryLoadNotLoadedAssembly(args);
        return assembly;
    }
    private Assembly? FindLoadedAssembly(ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        CommandWindow.Log($"FindLoadedAssembly: {assemblyName.Name}");
        return _loadedAssemblies!.TryGetValue(assemblyName.Name, out var loadedAssembly)
            ? loadedAssembly
            : null;
    }
    /// <summary>
    /// This way we solve a problem when Library dependent on another Library.
    /// And its very important to load it on first Assembly Resolve because
    /// somehow if you don't resolve it first time it will break everything.
    /// </summary>
    private Assembly? TryLoadNotLoadedAssembly(ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);
        var library = _libraries!.FirstOrDefault(x =>
            assemblyName.Name == x.Name);
        if (library != null)
        {
            CommandWindow.Log($"TryLoadNotLoadedAssembly: {library.Name} {library.Path}");
            var assembly = Assembly.Load(library.Content);
            _loadedAssemblies!.TryAdd(assemblyName.Name, assembly);
            return assembly;
        }
        return null;
    }

    private class Library
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public byte[]? Symbols { get; set; }
    }
}