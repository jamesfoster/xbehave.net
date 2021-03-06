#How to build#

These instructions are *only* for building with Rake, which includes compilation, test execution and packaging. Once you have the prerequisites this is the simplest way to build.

You can also build the solution using Visual Studio 2012 or later.

At the time of writing the build is only confirmed to work on Windows.

## Prerequisites ##

1. Ensure you have .NET framework 3.5 and 4.0/4.5 installed.

1. Install Ruby 1.8.7 or later.

 For Windows we recommend using [RubyInstaller](http://rubyinstaller.org/) and selecting 'Add Ruby executables to your PATH' when prompted. For alternatives see the [Ruby download page](http://www.ruby-lang.org/en/downloads/).
1. Using a command prompt, update RubyGems to the latest version:

    `gem update --system`

1. Install/update the Albacore gem (0.3.4 or later is required):

    `gem install albacore`

## Building ##

Using a command prompt, navigate to your clone root folder and execute:

`rake`

This executes the default build tasks. After the build has completed, the build artifacts will be located in `bin`.

##Extras##

* View the full list of build tasks:

    `rake -T`

* Run a specific task:

    `rake spec`

* Run multiple tasks:

    `rake spec pack`
