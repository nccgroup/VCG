# VCG (VisualCodeGrepper)

* Current version: [V2.3.0](#v230) (January 2023)

* Send comments and bug reports to: vcgapplication@gmail.com

---

## Contents

1. [Overview](#overview)
1. [Usage](#usage)
   * [Windows Application](#windows-application)
   * [Console (CLI) Usage](#console-cli-usage)
1. [Version History](#version-history)
   * [V2.3.0](#v230)
      * [CLI Changes](#cli-changes)
      * [Windows Application Changes](#windows-application-changes)
      * [Development-relevant changes](#development-relevant-changes)
      * [Other project changes](#other-project-changes)
      * [Known issues](#known-issues)
   * [V2.2.0](#v220)

---

## Overview

VCG is an automated code security review tool that handles C/C++, Java, C#, VB and PL/SQL. It has a few features that should hopefully make it useful to anyone conducting code security reviews, particularly where time is at a premium:

1. In addition to performing some more complex checks it also has a config file for each language that basically allows you to add any bad functions (or other text) that you want to search for
1. It attempts to find a range of around 20 phrases within comments that can indicate broken code (“ToDo”, “FixMe”, “Kludge”, etc.)
1. It provides a nice pie chart (for the entire codebase and for individual files) showing relative proportions of code, whitespace, comments, ‘ToDo’ style comments and bad code

I’ve tried to produce something which doesn’t return the large number of false positives that are returned by some tools and which also searches intelligently to identify buffer overflows and signed/unsigned comparisons.

---

## Usage

VCG works in both [Console (CLI) Usage](#console-cli-usage) and [Windows Application](#windows-application) modes. 

The Windows Application has the benefit of immediate data exploration and analysis, and user interaction. If you want something less interactive, and scriptable from other applications, use the CLI. All data exported from it is importable into the Windows Application.

VCG works in two phases: targetting and scanning. *Targetting* identifies the files required for the assessment. *Scanning* searches thetargetted analysis for various coding errors.

### Windows Application

#### Selecting a Target

Once opened, enter the Target Directory (or click File -> New Target Directory) to search for relevant files. Then choose the Language filter, and press Enter. All matching files will then be listed. After which, its time to initiate [Scanning](#scanning).

The languages choices are shown below. If you work with a specific language for a majority, you can set a Startup Language using the [Settings](#settings) -> Options dialog:

| Language | Filter | Notes |
|-|-|-|
|C/C++|.c .h .cpp .hpp||
|Java|.java .jsp web.xml config.xml|The XML files are included to check for input validation by Struts, etc.|
|PL/SQL|.pls .sql .pkb .pks||
|C#| .cs .aspx web.config| The web.config file is included to check for input validation, debug settings, etc.)|
|VB|.vb .asp .aspx web.config| The web.config file is included to check for input validation, debug settings, etc.)|
|PHP|.php php.ini|The php.ini file is included to check for bad configs such as `register_globals`|
|COBOL|.cob .cbl .clt .cl2 .cics||

------------------------------------------------------------------------------------

#### Scanning

The scan be carried out in a number of ways:
* **Comments Only**: VCG attempts to identify any comments that indicate broken or unfinished code based on a list of around 16 phrases that typically feature in such comments ('ToDo', 'FixMe', etc.)
* **Code Only**: VCG scans and reports on potential code security issues and any dangerous functions etc. from the config file that are located in the code.
* **Dangerous Functions Only**: VCG scans and reports only on any dangerous functions etc. from the config file that are found in the code.
* **Code, Dangerous Functions & Comments**: Also known as a Full Scan in the Scan menu, this is a combined scan of both code and comments covering all of the above.

------------------------------------------------------------------------------------

#### Advanced

##### Settings -> Options

VCG has preferences to match your workflow and specific assessment. These include:

* **File types**: Use this to alter the types of file that VCG will scan for each code type. To scan all files in a directory add .* to the list or delete all types and submit an empty string.

* **Config files**: Specify a configuration file for each language. This holds a listing of any functions or code fragments that may be considered a risk and require reporting. This feature essentially adds an additional layer of checks on top of the more complex operations carried out as part of the code scan.

* **Severity**: VCG can be set to only report errors above a certain level of severity. e.g. Select 'Medium' to only get Medium, High and Critical in the report.

* **COBOL settings**: The initial column should be specified - i.e. the first column after the line numbers. This will generally be 1 for a listing with no line numbers or 7 for a listing which includes line numbers. A different setting may be required if the code/comment lines in the listing begin in a different column. The z/OS setting can be used to include checks for safe use of the CICS API, etc.

* **OWASP Settings (Java only)**: If selected these will identify two violations of OWASP best practice for Java programming listed on the OWASP secure coding pages. Nested classes and non-final public classes will be reported on - as there are likely to be large numbers of these violations without a great deal of risk the option is given to turn off either of these scans.
* **Output file**: ASCII output will be written to this file if selected.

##### Configuration Files

Configuration files exist for each of the six languages that VCG scans. These provide an additional layer of scanning to supplement the built-in complex scans for each language.

The content of the configuration files consists of a list of functions/code fragments to scan for, along with an associated description to appear in the results. The description includes an optional severity setting in square braces and is separated from the function by ``'=>'`` with the following format:

> `function name=>[<severity>]<description>`

Where `<severity>` is a severity rating of 1 (Critical) to 3 (Medium), or 0 (Normal)

For example:

> `strcat=>[3]String concatenation function which facilitates buffer overflow conditions. Appears in Microsoft's banned function list.`

------------------------------------------------------------------------------------

#### Analysing Data

The Visual Code Breakdown will be shown when scanning has finished.

Results are written to the results pane in the order they have been located. Results have the following format:

```
SEVERITY: Code issue
Line number - File name
Description
[code fragment]
```

The issue title has the following colour codes for clarity:

* Critical - Magenta
* High - Red
* Medium - Orange
* Standard/Normal - Yellow/Sepia
* Low - Grey-Blue
* Potential Issue/Best Practice - Green
* Suspicious comment indicating broken code - Dark Blue

Results are also written to the summary table in an abbreviated form, where they can be ordered by clicking on column headings. Double-clicking an item in the results table results in the file being loaded in it's associated application.

#### Exporting data

VCG exports in several formats, such as XML, CSV and plain ASCII text. 

* ASCII and CSV are exported from File -> Save Results...
* XML is exported by from  File-> Export Results as XML...

A set of results filtered on severity can be saved as XML by right-clicking in the Results window and clicking Export Filtered XML Results...

#### Importing data

Any data exported from VCG can be imported back in, using the equivelant File -> Import menu options.

---

### Console (CLI) Usage

VCG can be run from the CLI. Ensure you provide the `--console` flag, otherwise it will run as a GUI application.

```
Command Line Parameters:

Usage:  VisualCodeGrepper [Options]

STARTUP OPTIONS:
	(Set desired starting point for GUI. If using console mode these options will set target(s) to be scanned.)
	-t, --target <Filename|DirectoryName>:	Set target file or directory. Use this option either to load target immediately into GUI or to provide the target for console mode.
	-l, --language <CPP|PLSQL|JAVA|CS|VB|PHP|COBOL>:	Set target language (Default is C/C++).
	-e, --extensions <ext1|ext2|ext3>:	Set file extensions to be analysed (See ReadMe or Options screen for language-specific defaults).
	-i, --import <Filename>:	Import XML/CSV results to GUI.

OUTPUT OPTIONS:
	(Automagically export results to a file in the specified format. Use XML or CSV output if you wish to reload results into the GUI later on.)
	-x, --export <Filename>:	Automatically export results to XML file.
	-f, --csv-export <Filename>:	Automatically export results to CSV file.
	-r, --results <Filename>:	Automatically export results to flat text file.

CONSOLE OPTIONS:
	-c, --console:		Run application in console only (hide GUI).
	-v, --verbose:		Set console output to verbose mode.
	-h, --help:		Show help.
```


---

## Version History

### V2.3.0
2.3.0 has been focussed on stability and usability aspects. The core functionality remains exactly the same. These include:

#### CLI changes
* `--console --help` doesn't hang AND displays console-based help
* Incomplete CLI parameters will now generate a `--help` section.
* Help messages shown when any file locations specified don't exist, e.g. imported results files or target directories
* Added extra CLI messages on guidance not to interfere with the CLI when the tool is running.
* CLI will not process anything unless you specify an output file type (XML, CSV or Flat File).
* Logging has been enhanced showing the following messages:
	* `[+]` - Informational (normal output)
	* `[*]` - Verbose, using `-v` flag
	* `[!]` - Errors

#### Windows Application changes
* UI redesign includes:
	* Target Directory box is now auto-complete
	* Language selection is via dropdown instead of menu
	* Press Enter when both Target Directory and Language is chosen, and it will run a Target Scan
	* Various new MsgBox's to confirm your actions where results need to be re-generated
	* Progress bars are now more informative on file counts
	* Visual Styles have been enabled, making the UI look more modern.
* Running this app from the console with just `--help` and without `--console`  will display a UI-based help both (a la SysInternals).
* Removed popup to remind you to select a language when starting up.
* Progress bar window allows aborting when running scans
* Fixed UI form bug that prevented the app being closed.
* Fixed other UI bug relating to the progress bar's being displayed.
* Fixed bug where the GUI-version crashed, if a user loaded it from the Console, and was not in the working directory of the .exe

#### Development-relevant changes

* Project updated to .NET Framework 4.8.1
* Installer has new pre-requisite of .NET 4.8.1
* Centralised logging for the CLI, honouring any `--console` and `--verbose` flags provided.
* Code tidied up to remove inline `asAppSettings.IsConsole = True`, as these are honoured when printing.
* Moved COM call for `AttachProcess()` to a NativeMethods module, per MS guidance
* Removed unused `Imports` across the solution
* Added a `--debug` flag debugging the CLI. When in DEBUG mode, you can attach the debuger using `--console --debug`

#### Other project changes

* README.txt is now README.md
* Docs updated to reflect new UI
* .ASP filter now only applies to VB.

#### Known issues

##### The CLI isn't locked from user input when using the `--console` mode.

 In short, VCG runs natively as a Windows Application. By its nature, it releases control back to the console when started. VCG is therefore writing to a console session thats already ended.
 
 From a development side, VS 2022 (and earlier) generates scaffold code for Windows Applications, though the `Application.Designer.vb` class. It gets re-generated on every build, so any direct modifications to it will be wiped out. If the project type is switched to a Console Application, it modifies this auto-generated file to the point it no longer builds at all. Consider also, that if this was possible, the console window will remain open for the duration of the GUI session as well.
 
 Also explored was creating entirely new projects but reflecting a Console build type. Trials included: 
 * Adding `VisualCodeGrepper.vbproj` as a project reference. This doesn't work , as the referenced project generates an .exe. Its can't reference assemblies that result in .exe's
 * Modifying the settings with the .vbproj file to create a console output. As mentioned, the scaffolding file will generated an incompatible file, resulting in dozens of build errors relating to variable scoping. 
 * Creating new compilation profiles, but these only change the Configuration/Platform. The settings for the most part are consistent throughout all builds.
 
 The overall resolution to cleanly separate the GUI from the Console is a re-architecture of the application. For a minor release anyway, this isn't possible. Perhaps look to implement an MVP-style app?

 -- Jan 2023

---

### V2.2.0

New features include:
1. COBOL added to list of supported languages.
1. Save metadata as XML: Use File -> Export Code Metadata as XML... menu option. The resulting XML holds details for number of lines of code, comments, whitespace, etc.
