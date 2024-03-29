====================================================================================
VCG (VisualCodeGrepper)

Current version: 2.3.2

====================================================================================

Contents:

1. Overview
2. Latest additions to VCG V2.3.2
3. Using VCG
	Input files/codebase
	Options & settings
	Scanning
	Configuration files
	Output
	Command Line Parameters

====================================================================================
Overview
------------------------------------------------------------------------------------

VCG is an automated code security review tool that handles C/C++, Java, C#, VB, PL/SQL, PHP and COBOL. It has a few features that should hopefully make it useful to anyone conducting code security reviews, particularly where time is at a premium:
1.	In addition to performing some more complex checks it also has a config file for each language that basically allows you to add any bad functions (or other text) that you want to search for
2.	It attempts to find a range of around 20 phrases within comments that can indicate broken code (�ToDo�, �FixMe�, �Kludge�, etc.)
3.	It provides a nice pie chart (for the entire codebase and for individual files) showing relative proportions of code, whitespace, comments, �ToDo� style comments and bad code

I�ve tried to produce something which doesn�t return the large number of false positives that are returned by some tools and which also searches intelligently to identify buffer overflows and signed/unsigned comparisons.


====================================================================================
Latest additions to VCG V2.3.2
------------------------------------------------------------------------------------

New features:
	1)	GUI change means that target language selection is via a dropdown list instead of menu (as in V2.3.1).
	2)	Beta functionality now includes the scanning of R Script (as in V2.3.1).
	3)	Minor bug fixes to deal with various issues and restore the allowing of multiple application instances.


====================================================================================
Using VCG
------------------------------------------------------------------------------------

Input Files/Codebase:

Before scanning the code, ensure that the correct language is selected as VCG will only test for issues related to the selected language and only scan the relevant file types. The language can be selected in the settings menu menu or the options dialog.
Select the directory which contains the code to be scanned using File=>New Target...
VCG will then load all files that have the specified endings. 

The defaults are below but these can be modified using the Options dialog:
C/C++:	.c .h .cpp .hpp
Java:	.java .jsp web.xml config.xml	(the xml files are included to check for input validation by Struts, etc.)
PL/SQL:	.pls .sql .pkb .pks
C#:	.cs .asp .aspx web.config	(the web.config file is included to check for input validation, debug settings, etc.)
VB:	.vb .asp .aspx web.config	(the web.config file is included to check for input validation, debug settings, etc.)
PHP:	.php php.ini			(the php.ini file is included to check for bad configs such as register_globals)
COBOL:	.cob .cbl .clt .cl2 .cics
R:	.r

------------------------------------------------------------------------------------

Options and Settings:

File types - 	Use this to alter the types of file that VCG will scan for each code type. To scan all files in a directory add .* to the list or delete all types and submit an empty string.
Config files -	Specify a configuration file for each language. This holds a listing of any functions or code fragments that may be considered a risk and require reporting. This feature essentially adds an additional layer of checks on top of the more complex operations carried out as part of the code scan.
Severity -	VCG can be set to only report errors above a certain level of severity. e.g. Select 'Medium' to only get Medium, High and Critical in the report.
COBOL settings - The initial column should be specified - i.e. the first column after the line numbers. This will generally be 1 for a listing with no line numbers or 7 for a listing which includes line numbers. A different setting may be required if the code/comment lines in the listing begin in a different column. The z/OS setting can be used to include checks for safe use of the CICS API, etc.
OWASP Settings (Java only) - If selected these will identify two violations of OWASP best practice for Java programming listed on the OWASP secure coding pages. Nested classes and non-final public classes will be reported on - as there are likely to be large numbers of these violations without a great deal of risk the option is given to turn off either of these scans.
Output file -	ASCII output will be written to this file if selected.
Beta functionality -	Select whether to use any beta functionality by clicking the appropriate checkbox in Options -> Beta Functionality. Currently avaialable beta functionality is location of Signed/Unsigned Comparison in C (very unreliable), and scanning of R (tested in working reasonably well - close to inclusion in next release).

------------------------------------------------------------------------------------

Scanning:

The scan be carried out in three ways:
1. Comments Only -	VCG attempts to identify any comments that indicate broken or unfinished code based on a list of around 16 phrases that typically feature in such comments ('ToDo', 'FixMe', etc.)
2. Code Only - 		VCG scans and reports on potential code security issues and any dangerous functions etc. from the config file that are located in the code.
3. Dangerous Functions Only -	VCG scans and reports only on any dangerous functions etc. from the config file that are found in the code.
4. Code, Dangerous Functions & Comments -	Also known as a Full Scan in the Scan menu, this is a combined scan of both code and comments covering all of the above.

------------------------------------------------------------------------------------

Configuration Files:

Configuration files exist for each of the six languages that VCG scans. These provide an additional layer of scanning to supplement the built-in complex scans for each language.
The content of the configuration files consists of a list of functions/code fragments to scan for, along with an associated description to appear in the results. The description includes an optional severity setting in square braces and is separated from the function by '=>' with the following format:
function name[=>][[N]][description]
(where N is a severity rating of 1 (Critical) to 3 (Medium) (or optionally, 0 for 'normal'))

For example:
strcat=>[3]String concatenation function which facilitates buffer overflow conditions. Appears in Microsoft's banned function list.


------------------------------------------------------------------------------------

Output:

The Visual Code Breakdown will be shown when scanning has finished.

Results are written to the results pane in the order they have been located. Results have the following format:
SEVERITY: Code issue
Line number - File name
Description
[code fragment]


The issue title has the following colour codes for clarity:
Critical - Magenta
High - Red
Medium - Orange
Standard/Normal - Yellow/Sepia
Low - Grey-Blue
Potential Issue/Best Practice - Green
Suspicious comment indicating broken code - Dark Blue


Results are also written to the summary table in an abbreviated form, where they can be ordered by clicking on column headings. Double-clicking an item in the results table results in the file being loaded in it's associated application.


These results can be saved as ASCII text by clicking File=>Save Results...
The results can be saved as XML by clicking File=>Export Results as XML...
A set of results filtered on severity can be saved as XML by right-clicking in the Results window and clicking Export Filtered XML Results...
The results can be saved to a CSV file by clicking File=>Export Results as CSV...


------------------------------------------------------------------------------------

Command Line Parameters:

Usage:  VisualCodeGrepper [Options]

STARTUP OPTIONS:
	(Set desired starting point for GUI. If using console mode these options will set target(s) to be scanned.)
	-t, --target <Filename|DirectoryName>:	Set target file or directory. Use this option either to load target immediately into GUI or to provide the target for console mode.
	-l, --language <CPP|PLSQL|JAVA|CS|VB|PHP|COBOL|R>:	Set target language (Default is C/C++).
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

====================================================================================
