# FixLibrary

Still adding libraries manually? That's old school!

FixLibrary is a Module, which solves a problem with libraries, so you don't need to install libraries in `Rocket/Libraries` or use [OpenMod](https://github.com/openmod/openmod), or even ask on [Unturned GitHub](https://github.com/SmartlyDressedGames/Legally-Distinct-Missile/issues/68#issuecomment-1763227409) to get a needed library to resolve libraries conflicts/problems!

## Installation

1. **Stop the Server**: If your server is running, stop it.
2. **(If installed before)**: Delete `FixLibrary` from `Modules`.
3. **Download the Latest `FixLibrary`**: Go to the [FixLibrary releases page](https://github.com/RocketModFix/RocketModFix/releases).
4. **Access the Assets**: Open the "Assets" section if it's not already expanded.
5. **Download the Module**: Click `FixLibrary.Module.zip` to download the latest module.
6. **Final**: Extract the downloaded archive, open the `FixLibrary` folder, and copy the `FixLibrary` folder to `Modules` (copy the folder, not it's content).

## Discord
Feel free to join our [Discord Server](https://discord.gg/2yG5t869uq), ask questions, talk, and have fun!

## Update Libraries

This is the most important part of the FixLibrary project, we will have to update the libraries as fast as possible, of course people will need to reinstall FixLibrary manually, doing it automatically doesn't sound like a good idea, our options:

1. Copy some part of the Libraries from OpenMod (but sure, OpenMod is not that fast about updates as practice shows, probably we will have to do everything ourselves, and set our own rules so everyone will adapt to us).
2. Once Unturned Updates Unity Engine version, we have to install Unity and copy libraries from there.
3. Maybe make kind of CI on GitHub that installs needed Unity's Unturned version and replaces the libraries with latest ones. (but I don't think that gonna happen)
4. Auto-install Libraries from GitHub or somewhere else?!

For sure, we will need to add new libraries as more attraction to the project comes.

## The goal

Is so simple, fix problems with libraries and create a "Golden Standard" for Libraries, so everyone will use the same libraries to solve that problem.

## How it works

FixLibrary fixes the problems with libraries because:

1. No Libraries used in FixLibrary Module, we use only what is already in Unturned.
2. We ship all the needed libraries inside the Module, so Unturned will load it first.
3. We Target The same Framework version as Unturned does.
4. We keep things simple.

## For Plugin Devs

If you're a plugin dev, this is for you:

Imagine a user installs your plugin along with some libraries. But then they add more libraries to their server, and suddenly boom! There's a conflict. You might say, "Just replace those with mine," but that can unload your plugin/other-plugins, break commands, and cause all sorts of weird issues. Sure, you might eventually fix it, but do you really want to deal with that mess?

What if the user decides to remove your plugin? Now other plugins break because they were relying on your bundled libraries. The cycle repeats.

Even if you create your own loader and package everything there like with ImperialPluginLoader, Feli Loader, or others it just shifts the problem. If the user's license expires, or they stop using your loader, everything falls apart again. Why go through this hassle?

Would also recommend to read this [post](https://sunnamed434.github.io/posts/assemblyresolve-and-mono/), about libraries, loaders, and Mono.

### Safe Plugin

To make your plugin safe to use, here are your options:

- **Don't include any libraries** in your plugin. But for complex plugins, that's not realistic.
- **Vendor libraries**: Copy the library's source code directly into your project. However, you still face issues like shipping the library with your plugin or maintaining its source code.
- **ONLY .NET 461 or higher/lower**: You can't use .NET Standard normally.
- **Put Libraries in Unturned_Data\Managed**: Lol, really, nah.

**OR** simply skip all that hassle and use **FixLibrary**.

## A Little History

Initially, [me, sunnamed](https://github.com/sunnamed434), planned a larger-scale project with patches for common issues and the ability to build plugins like Rocket, but on a module level. However, that turned out to be overly complex. The most useful part of my original idea focused on **libraries**, because nearly everyone uses plugins, and the biggest pain point is dealing with libraries.

You install a plugin, only to find you need to remove or replace libraries, install others, and spend tons of time troubleshooting. As a plugin dev, you might even end up editing the plugin, changing library versions, rebuilding, and sending it back and forth with users just to test. Exhausting, right?

I considered integrating this into [RocketModFix](https://github.com/RocketModFix/RocketModFix) directly, but it would have made that project too complicated. My interest in **FixLibrary** comes from wanting to solve the headaches I've faced with libraries in **UnturnedGuard** and my other plugins.