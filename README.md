# VS Test platform

This is modified version of original VS Test platform that support callbacks on various stages of execution. It was created as part of the [Cloud.Tests](https://github.com/ncr-swt-retail/emerald1-cloud.tests) project.

## Getting Started

* Clone this repo: `git clone git@github.com:AF250329/vstest.git`
* Restore nuget packages
* Build
* Navigate to [\scripts](https://github.com/AF250329/vstest/tree/master/scripts) folder and run [build.ps1](https://github.com/AF250329/vstest/blob/00a88c96ad53ea49f98d199002683b0a05c553de/scripts/build.ps1) script
> This script will publish all *vstest* projects - ready to use. Right now it configured to publish into `\artifacts\Debug` (or `\artifacts\Release`) folder
* Copy everything from `\artifacts\Debug\net451\win7-x64` folder into output folder of `vstests.console` project: `src\vstest.console\bin\Debug\net451\win7-x64`.
> VSTest platform is complicated beast that has a lot of moving parts. It has special assemblies for test discovery, tests running, logs collectors, etc.. Not all of them exist in this solution - some of them downloaded by `build.ps1` script. Some projects must exist in `win7-x64\Extensions` folder and some goes to `win7-x64\x86` or `win7-x64\x64` folders... So the best way to arrange the files it to publish it with `build.ps1` script and then just overwrite the files in `vstest.console` output folder. Then - when `vstest.console` rebuilds it will overwrite only needed files and you won't have any files **missing** while VS Tests platform tries to load it dynamically.
* Make sure that 'test' project that you gonna work on it has Nuget of `MSTest.TestAdapter` minimum version **`2.2.8`** (on lower versions you could run into `Could not load type 'Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupBehavior` exception) and actual `MSTest.TestFramework` with version `2.2.8` also.
  
Original README of this project (including links to documentation exist at \wiki on top)

Enjoy ! :wink:
